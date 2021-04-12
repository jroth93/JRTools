using Autodesk.Revit.DB;
using Autodesk.Revit.UI;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using XL = Microsoft.Office.Interop.Excel;

namespace Proficient
{
    [Autodesk.Revit.Attributes.Transaction(Autodesk.Revit.Attributes.TransactionMode.Manual)]
    class ExcelAssign : IExternalCommand
    {
        private static Document doc;
        private static XL.Application xl;
        private static XL.Workbook wb;
        private static XL.Worksheet ws;
        private static bool xlReadOnly = false;
        public Result Execute(ExternalCommandData revit, ref string message, ElementSet elements)
        {
            doc = revit.Application.ActiveUIDocument.Document;
            ExcelAssignFrm eafrm = new ExcelAssignFrm();
            eafrm.ShowDialog();

            if (xlReadOnly) File.Delete(wb.FullName);
            else wb.Close(false);

            return Result.Succeeded;
        }

        public static String[] GetExcelColumns(int wsIndex, int hdrRow)
        {
            List<string> cols = new List<string>();
            ws = wb.Worksheets.Item[wsIndex];

            int totCols = ws.UsedRange.Columns.Count;
            string cellVal = String.Empty;

            if (totCols > 0)
            {
                for (int i = 1; i <= totCols; i++)
                {
                    cellVal = ws.Cells[hdrRow, i].Value == null ? "" : Convert.ToString(ws.Cells[hdrRow, i].Value);
                    cols.Add(cellVal);
                }
            }

            return cols.ToArray();
        }

        public static String[] OpenExcel(string xlPath)
        {
            xl = new XL.Application();

            try
            {
                FileStream stream = File.Open(xlPath, FileMode.Open, FileAccess.Read);
                stream.Close();
            }
            catch (IOException ex)
            {
                if (ex.Message.Contains("being used by another process"))
                {
                    string newPath =
                        Path.GetExtension(xlPath) == ".xlsx" ?
                        Path.GetDirectoryName(xlPath) + @"\" + Path.GetFileNameWithoutExtension(xlPath) + "-temp.xlsx" :
                        Path.GetDirectoryName(xlPath) + @"\" + Path.GetFileNameWithoutExtension(xlPath) + "-temp.xlsm";
                    File.Copy(xlPath, newPath);
                    xlPath = newPath;
                    xlReadOnly = true;
                }
            }

            wb = xl.Workbooks.Open(Filename: xlPath, ReadOnly: true);

            return (wb.Worksheets as IEnumerable<XL.Worksheet>).Select(ws => ws.Name).ToArray();

        }

        private static dynamic GetCellVal(int row, int col, string parType, DisplayUnitType dispUnit)
        {
            switch (parType)
            {
                case "String":
                    return Convert.ToString(ws.Cells[row, col].Value);
                case "Integer":
                    try
                    {
                        int val = Convert.ToInt32(ws.Cells[row, col].Value);
                        val = (int)UnitUtils.ConvertToInternalUnits(val, dispUnit);
                        return val;
                    }
                    catch
                    {
                        return null;
                    }
                case "Double":
                    try
                    {
                        double val = Convert.ToDouble(ws.Cells[row, col].Value);
                        val = UnitUtils.ConvertToInternalUnits(val, dispUnit);
                        return val;
                    }
                    catch
                    {
                        return null;
                    }

                case "ElementID":
                    try
                    {
                        return new ElementId(Convert.ToInt32(ws.Cells[row, col].Value));
                    }
                    catch
                    {
                        return null;
                    }
            }
            return null;
        }

        private static dynamic GetCellVal(int row, int col, string parType)
        {
            switch (parType)
            {
                case "String":
                    return Convert.ToString(ws.Cells[row, col].Value);
                case "Integer":
                    try
                    {
                        return Convert.ToInt32(ws.Cells[row, col].Value);
                    }
                    catch
                    {
                        return null;
                    }
                case "Double":
                    try
                    {
                        return Convert.ToDouble(ws.Cells[row, col].Value);
                    }
                    catch
                    {
                        return null;
                    }

                case "ElementID":
                    try
                    {
                        return new ElementId(Convert.ToInt32(ws.Cells[row, col].Value));
                    }
                    catch
                    {
                        return null;
                    }
            }
            return null;
        }

        public static void WriteErrorFile(string errorLog)
        {
            string logFilePath = wb.Path + "\\ExcelToRevitErrorLog.txt";

            using (StreamWriter sw = File.CreateText(logFilePath))
            {
                sw.Write(errorLog);
            }
        }

        public static string[] GetCategories()
        {
            List<string> cList = new List<string>();

            foreach (Category c in doc.Settings.Categories)
            {
                int catCount = new FilteredElementCollector(doc).OfCategoryId(c.Id).Count();

                if (c.AllowsBoundParameters && catCount > 0)
                {
                    cList.Add(c.Name);
                }
            }
            cList.Sort();

            return cList.ToArray();
        }

        public static String[] GetFamiliesOfCategory(string catName)
        {
            string[] fn = (new FilteredElementCollector(doc)
            .OfClass(typeof(Family))
            .Where(q => (q as Family).FamilyCategory.Name == catName)
            .Select(q => q.Name) as IEnumerable<string>).ToArray();

            return fn;
        }

        public static String[] GetFamilyParameters(string familyName)
        {
            List<string> pars = new List<string>();

            FamilySymbol fs = new FilteredElementCollector(doc)
            .OfClass(typeof(FamilySymbol))
            .Where(q => (q as FamilySymbol).Family.Name == familyName)
            .FirstOrDefault() as FamilySymbol;
            foreach (Parameter par in fs.Parameters)
            {
                if (!par.IsReadOnly)
                {
                    pars.Add(par.Definition.Name + " (type)");
                }
            }

            FamilyInstance fi = new FilteredElementCollector(doc)
                .OfClass(typeof(FamilyInstance))
                .Where(q => (q as FamilyInstance).Symbol.Family.Name == Convert.ToString(familyName))
                .FirstOrDefault() as FamilyInstance;
            if (fi != null)
            {
                foreach (Parameter par in fi.Parameters)
                {
                    if (!par.IsReadOnly)
                    {
                        pars.Add(par.Definition.Name + " (inst)");
                    }
                }
            }

            return pars.ToArray();
        }

        public static string AssignParameterValuesType(string familyName, string parName, int keyCol, int startRow, int parCol)
        {
            string errorLog = String.Empty;

            int totRows = ws.UsedRange.Rows.Count;
            string keyCellVal = null;

            var fslist = new FilteredElementCollector(doc)
                .OfClass(typeof(FamilySymbol))
                .Where(q => (q as FamilySymbol).Family.Name == familyName)
                .OrderBy(q => Convert.ToString(q.get_Parameter(BuiltInParameter.ALL_MODEL_TYPE_MARK)));

            string typeMark = null;
            string parType = Convert.ToString(fslist.First().LookupParameter(parName).StorageType);

            foreach (FamilySymbol fs in fslist)
            {
                typeMark = fs.get_Parameter(BuiltInParameter.ALL_MODEL_TYPE_MARK).AsString();
                for (int r = startRow; r <= totRows; r++)
                {
                    keyCellVal = Convert.ToString(ws.Cells[r, keyCol].Value);
                    if (typeMark == keyCellVal)
                    {
                        bool hasUnits;
                        try
                        {
                            DisplayUnitType dut = fs.LookupParameter(parName).DisplayUnitType;
                            hasUnits = true;
                        }
                        catch (Autodesk.Revit.Exceptions.InvalidOperationException)
                        {
                            hasUnits = false;
                        }

                        using (Transaction tx = new Transaction(doc, "Assign Type Parameter"))
                        {
                            if (tx.Start() == TransactionStatus.Started)
                            {
                                var newVal = hasUnits ? GetCellVal(r, parCol, parType, fs.LookupParameter(parName).DisplayUnitType) : GetCellVal(r, parCol, parType);

                                if (newVal != null)
                                {
                                    fs.LookupParameter(parName).Set(newVal);
                                    tx.Commit();
                                }
                                else
                                {
                                    errorLog += $"Incorrect data type in Excel File for element '{typeMark}' parameter '{parName}.' Parameter will not be assigned.\n";
                                }
                            }
                        }
                    }
                }
            }


            return errorLog;
        }

        public static string AssignParameterValuesInst(string familyName, string parName, int keyCol, int startRow, int parCol)
        {
            string errorLog = String.Empty;

            int totRows = ws.UsedRange.Rows.Count;
            string keyCellVal = null;

            List<Element> filist = new FilteredElementCollector(doc)
                .OfClass(typeof(FamilyInstance))
                .Where(q => (q as FamilyInstance).Symbol.Family.Name == familyName).ToList();

            string mark = null;
            string parType = Convert.ToString(filist.First().LookupParameter(parName).StorageType);

            foreach (FamilyInstance fi in filist)
            {
                mark = fi.get_Parameter(BuiltInParameter.ALL_MODEL_MARK).AsString();
                for (int r = startRow; r <= totRows; r++)
                {
                    keyCellVal = Convert.ToString(ws.Cells[r, keyCol].Value);
                    if (mark == keyCellVal)
                    {
                        bool hasUnits;
                        try
                        {
                            DisplayUnitType dut = fi.LookupParameter(parName).DisplayUnitType;
                            hasUnits = true;
                        }
                        catch (Autodesk.Revit.Exceptions.InvalidOperationException)
                        {
                            hasUnits = false;
                        }

                        var newVal = hasUnits ? GetCellVal(r, parCol, parType, fi.LookupParameter(parName).DisplayUnitType) : GetCellVal(r, parCol, parType);

                        using (Transaction tx = new Transaction(doc, "Assign Instance Parameter"))
                        {
                            if (tx.Start() == TransactionStatus.Started)
                            {
                                if (newVal != null)
                                {
                                    fi.LookupParameter(parName).Set(newVal);
                                    tx.Commit();
                                }
                                else
                                {
                                    errorLog += $"Incorrect data type in Excel File for element '{mark}' parameter '{parName}.' Parameter will not be assigned.\n";
                                }
                            }
                        }
                    }
                }
            }


            return errorLog;
        }
    }
}
