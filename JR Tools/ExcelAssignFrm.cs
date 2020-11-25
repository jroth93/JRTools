using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.IO;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Runtime.InteropServices;
using Excel = Microsoft.Office.Interop.Excel;
using Autodesk.Revit.DB;

namespace JR_Tools
{
    [Autodesk.Revit.Attributes.Transaction(Autodesk.Revit.Attributes.TransactionMode.Manual)]
    public partial class ExcelAssignFrm : System.Windows.Forms.Form
    {
        private Document rvtdoc;

        private Excel.Application xl;
        private Excel.Workbook xlwb;
        private Excel.Worksheet xlws;
        private List<string> cols = new List<string>();

        private bool byType;
        private int parCnt=1;
        private bool xlIsOpen = false;

        private List<ComboBox> colDrops = new List<ComboBox>();
        private List<ComboBox> parDrops = new List<ComboBox>();

        public ExcelAssignFrm(Document doc)
        {
            InitializeComponent();

            rvtdoc = doc;
            List<string> clist = new List<string>();
            foreach(Category c in doc.Settings.Categories)
            {
                if (c.AllowsBoundParameters)
                {
                    clist.Add(c.Name);
                }
            }
            clist.Sort();
            catDrop.Items.AddRange(clist.ToArray());
            catDrop.SelectedIndex = 0;
            colDrops.Add(sc1);
            parDrops.Add(dp1);

        }

        private void CleanupExcel()
        {
            GC.Collect();
            GC.WaitForPendingFinalizers();

            if (xlIsOpen)
            {
                File.Delete(xlwb.FullName);
            }
            else
            {
                xlwb.Close(false);
            }

            Marshal.FinalReleaseComObject(xlwb);
            xl.Quit();
            Marshal.FinalReleaseComObject(xl);
        }

        private int DropDownWidth(ComboBox myCombo)
        {
            int maxWidth = 0;
            int temp = 0;
            Label label1 = new Label();

            foreach (var obj in myCombo.Items)
            {
                label1.Text = obj.ToString();
                temp = label1.PreferredWidth;
                if (temp > maxWidth)
                {
                    maxWidth = temp;
                }
            }
            label1.Dispose();
            return maxWidth + SystemInformation.VerticalScrollBarWidth;
        }

        private void assignParameterValues(int i)
        {
            string parName = parDrops[i - 1].SelectedItem.ToString();
            parName = parName.Substring(0, parName.Length - 7);
            int keyCol = keyColDrop.SelectedIndex + 1;
            int parCol = keyColDrop.Items.IndexOf(colDrops[i - 1].SelectedItem) + 1;
            int startRow = Convert.ToInt32(hdrRowCtrl.Value) + 1;
            int totRows = xlws.UsedRange.Rows.Count;
            string keyCellVal = null;

            if (byType)
            {
                var fslist = new FilteredElementCollector(rvtdoc)
                    .OfClass(typeof(FamilySymbol))
                    .Where(q => (q as FamilySymbol).Family.Name == familyDrop.SelectedItem.ToString())
                    .OrderBy(q => q.get_Parameter(BuiltInParameter.ALL_MODEL_TYPE_MARK).AsString());

                string typeMark = null;
                string parType = fslist.First().LookupParameter(parName).StorageType.ToString();

                foreach (FamilySymbol fs in fslist)
                {
                    typeMark = fs.get_Parameter(BuiltInParameter.ALL_MODEL_TYPE_MARK).AsString();
                    for (int r = startRow; r <= totRows; r++)
                    {
                        keyCellVal = xlws.Cells[r, keyCol].Value.ToString();
                        if (typeMark == keyCellVal)
                        {
                            using (Transaction tx = new Transaction(rvtdoc, "commandname"))
                            {
                                if (tx.Start() == TransactionStatus.Started)
                                {
                                    var newVal = GetCellVal(r, parCol, parType);
                                    try
                                    {
                                        newVal = GetCellVal(r, parCol, parType, fs.LookupParameter(parName).DisplayUnitType);
                                    }
                                    catch { }

                                    if (newVal != null)
                                    {
                                        fs.LookupParameter(parName).Set(newVal);
                                        tx.Commit();
                                    }
                                    else
                                    {
                                        string msg = $"Incorrect data type in Excel File for element '{typeMark}' parameter '{parName}.' Parameter will not be assigned.";
                                        MessageBox.Show(msg, "Incorrect Format", MessageBoxButtons.OK);
                                    }
                                }
                            }
                        }
                    }
                }
            }
            else
            {
                List<Element> filist = new FilteredElementCollector(rvtdoc)
                    .OfClass(typeof(FamilyInstance))
                    .Where(q => (q as FamilyInstance).Symbol.Family.Name == familyDrop.SelectedItem.ToString()).ToList();

                string mark = null;
                string parType = filist.First().LookupParameter(parName).StorageType.ToString();

                foreach (FamilyInstance fi in filist)
                {
                    mark = fi.get_Parameter(BuiltInParameter.ALL_MODEL_MARK).AsString();
                    for (int r = startRow; r <= totRows; r++)
                    {
                        keyCellVal = xlws.Cells[r, keyCol].Value.ToString();
                        if (mark == keyCellVal)
                        {
                            using (Transaction tx = new Transaction(rvtdoc, "commandname"))
                            {
                                if (tx.Start() == TransactionStatus.Started)
                                {
                                    var newVal = GetCellVal(r, parCol, parType);
                                    try
                                    {
                                        newVal = GetCellVal(r, parCol, parType, fi.LookupParameter(parName).DisplayUnitType);
                                    }
                                    catch { }

                                    if (newVal != null)
                                    {
                                        fi.LookupParameter(parName).Set(newVal);
                                        tx.Commit();
                                    }
                                    else
                                    {
                                        string msg = $"Incorrect data type in Excel File for element '{mark}' parameter '{parName}.' Parameter will not be assigned.";
                                        MessageBox.Show(msg, "Incorrect Format", MessageBoxButtons.OK);
                                    }
                                }
                            }
                        }
                    }
                }
            }
        }

        private dynamic GetCellVal(int row, int col, string parType, DisplayUnitType dispUnit)
        {
            switch(parType)
            {
                case "String":
                    return Convert.ToString(xlws.Cells[row, col].Value);
                case "Integer":
                    try
                    {
                        int val = Convert.ToInt32(xlws.Cells[row, col].Value);
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
                        double val = Convert.ToDouble(xlws.Cells[row, col].Value);
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
                        return new ElementId(Convert.ToInt32(xlws.Cells[row, col].Value));
                    }
                    catch
                    {
                        return null;
                    }
            }
            return null;
        }

        private dynamic GetCellVal(int row, int col, string parType)
        {
            switch (parType)
            {
                case "String":
                    return Convert.ToString(xlws.Cells[row, col].Value);
                case "Integer":
                    try
                    {
                        return Convert.ToInt32(xlws.Cells[row, col].Value);
                    }
                    catch
                    {
                        return null;
                    }
                case "Double":
                    try
                    {
                        return Convert.ToDouble(xlws.Cells[row, col].Value);
                    }
                    catch
                    {
                        return null;
                    }

                case "ElementID":
                    try
                    {
                        return new ElementId(Convert.ToInt32(xlws.Cells[row, col].Value));
                    }
                    catch
                    {
                        return null;
                    }
            }
            return null;
        }

        private void xlfilebtn_Click(object sender, EventArgs e)
        {
            OpenFileDialog fd = new OpenFileDialog();
            fd.Title = "Browse to Excel file.";
            fd.ValidateNames = false;
            fd.ShowDialog();
            string xlPath = fd.FileName;
            filelocationtxt.Text = xlPath;

            xl = new Excel.Application();

            FileStream stream = null;
            try
            {
                stream = File.Open(xlPath, System.IO.FileMode.Open, System.IO.FileAccess.Read);
            }
            catch (IOException ex)
            {
                if (ex.Message.Contains("being used by another process"))
                {
                    File.Copy(xlPath, xlPath + "temp.xlsx");
                    xlPath += "temp.xlsx";
                    xlIsOpen = true;
                }
            }
            finally
            {
                if (stream != null)
                    stream.Close();
            }

            xl.Workbooks.Open(Filename: xlPath, ReadOnly: true);
            xlwb = xl.ActiveWorkbook;

            String[] xlsheets = new String[xlwb.Worksheets.Count];
            int i = 0;
            foreach (Excel.Worksheet wSheet in xlwb.Worksheets)
            {
                xlsheets[i] = wSheet.Name;
                i++;
            }
            wkshtDrop.Items.AddRange(xlsheets);
            wkshtDrop.SelectedIndex = 0;
            
        }

        private void assnbtn_Click(object sender, EventArgs e)
        {  
            for(int i = 1; i <= parCnt; i++) 
                assignParameterValues(i);
        }

        private void closebtn_Click(object sender, EventArgs e)
        {
            CleanupExcel();
            this.Close();
        }

        private void wkshtDrop_SelectedIndexChanged(object sender, EventArgs e)
        {
            cols.Clear();
            xlws = xlwb.Worksheets.Item[wkshtDrop.SelectedIndex + 1];
            int totCols = xlws.UsedRange.Columns.Count;
            int hdrRow = Convert.ToInt32(hdrRowCtrl.Value);
            string cellVal = "";

            if (totCols > 0)
            {
                for (int i = 1; i <= totCols; i++)
                {
                    cellVal = xlws.Cells[hdrRow, i].Value == null ? "" : Convert.ToString(xlws.Cells[hdrRow, i].Value);
                    cols.Add(cellVal);
                }

                keyColDrop.Items.Clear();
                keyColDrop.Items.AddRange(cols.ToArray());
                keyColDrop.SelectedIndex = 0; 
            } 
        }

        private void catDrop_SelectedIndexChanged(object sender, EventArgs e)
        {
            string[] fn = (new FilteredElementCollector(rvtdoc)
                .OfClass(typeof(Family))
                .Where(q => (q as Family).FamilyCategory.Name == catDrop.SelectedItem.ToString())
                .Select(q => q.Name) as IEnumerable<string>).ToArray();

            familyDrop.Items.Clear();
            familyDrop.Items.AddRange(fn);
        }

        private void keycolumndrop_SelectedIndexChanged(object sender, EventArgs e)
        {
            string[] kcditems = new string[keyColDrop.Items.Count];
            keyColDrop.Items.CopyTo(kcditems, 0);
            sc1.Items.Clear();
            sc1.Items.AddRange(kcditems);
            sc1.Items.Remove(keyColDrop.SelectedItem);
            if(kcditems.Length > 1)
                sc1.SelectedIndex = 0;
        }

        private void familydrop_SelectedIndexChanged(object sender, EventArgs e)
        {
            dp1.Items.Clear();

            FamilySymbol fs = new FilteredElementCollector(rvtdoc)
                .OfClass(typeof(FamilySymbol))
                .Where(q => (q as FamilySymbol).Family.Name == familyDrop.SelectedItem.ToString())
                .FirstOrDefault() as FamilySymbol;
            foreach(Parameter par in fs.Parameters)
            {
                if (!par.IsReadOnly)
                {
                    dp1.Items.Add(par.Definition.Name + " (type)");
                }
            }

            FamilyInstance fi = new FilteredElementCollector(rvtdoc)
                .OfClass(typeof(FamilyInstance))
                .Where(q => (q as FamilyInstance).Symbol.Family.Name == familyDrop.SelectedItem.ToString())
                .FirstOrDefault() as FamilyInstance;
            if (fi != null)
            {
                foreach (Parameter par in fi.Parameters)
                {
                    if (!par.IsReadOnly)
                    {
                        dp1.Items.Add(par.Definition.Name + " (inst)");
                    }
                } 
            }
            dp1.SelectedIndex = 0;
            dp1.DropDownWidth = DropDownWidth(dp1);
        }

        private void dp1_SelectedIndexChanged(object sender, EventArgs e)
        {
            string curItem = dp1.SelectedItem.ToString();
            string typeInst = curItem.Substring(curItem.Length - 5, 4);
            if(typeInst == "inst")
            {
                typeInstLbl.Text = "Assigning by Instance";
                byType = false;
            }
            else
            {
                typeInstLbl.Text = "Assigning by Type";
                byType = true;
            }
        }

    }
}
