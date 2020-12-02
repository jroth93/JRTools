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
using Microsoft.Office.Interop.Excel;

namespace JR_Tools
{
    [Autodesk.Revit.Attributes.Transaction(Autodesk.Revit.Attributes.TransactionMode.Manual)]
    public partial class ExcelAssignFrm : System.Windows.Forms.Form
    {
        private Document rvtDoc;

        private Excel.Application xl;
        private Excel.Workbook xlwb;
        private Excel.Worksheet xlws;
        private List<string> cols = new List<string>();

        private bool byType;
        private int parCnt=1;
        private bool xlIsOpen = false;
        private string errorLog = "";

        private List<ComboBox> colDrops = new List<ComboBox>();
        private List<ComboBox> parDrops = new List<ComboBox>();

        public ExcelAssignFrm(Document doc)
        {
            InitializeComponent();

            getColsBtn.Visible = false;
            rvtDoc = doc;
            List<string> cList = new List<string>();
            foreach(Category c in doc.Settings.Categories)
            {
                FilteredElementCollector cElCol = new FilteredElementCollector(rvtDoc).OfCategoryId(c.Id);

                if (c.AllowsBoundParameters && cElCol.Count() > 0)
                {
                    cList.Add(c.Name);
                }
            }
            cList.Sort();
            catDrop.Items.AddRange(cList.ToArray());
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

            Marshal.FinalReleaseComObject(xlws);
            Marshal.FinalReleaseComObject(xlwb);
            xl.Quit();
            Marshal.FinalReleaseComObject(xl);
        }

        private int DropDownWidth(ComboBox myCombo)
        {
            int maxWidth = 0;
            int temp = 0;
            System.Windows.Forms.Label label1 = new System.Windows.Forms.Label();

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

        private void AssignParameterValues(int i)
        {
            errorLog = "";
            string parName = parDrops[i - 1].SelectedItem.ToString();
            parName = parName.Substring(0, parName.Length - 7);
            int keyCol = keyColDrop.SelectedIndex + 1;
            int parCol = keyColDrop.Items.IndexOf(colDrops[i - 1].SelectedItem) + 1;
            int startRow = Convert.ToInt32(hdrRowCtrl.Value) + 1;
            int totRows = xlws.UsedRange.Rows.Count;
            string keyCellVal = null;

            if (byType)
            {
                var fslist = new FilteredElementCollector(rvtDoc)
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

                            using (Transaction tx = new Transaction(rvtDoc, "Assign Type Parameter"))
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
            }
            else
            {
                List<Element> filist = new FilteredElementCollector(rvtDoc)
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

                            using (Transaction tx = new Transaction(rvtDoc, "Assign Instance Parameter"))
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
            }

            if(errorLog != "")
            {
                MessageBox.Show("There were errors in the parameter assignment. Please see the error log in the Excel file directory for details.", "Assignment Errors", MessageBoxButtons.OK);

                string logFilePath = xlwb.Path + @"\" + "ExcelToRevitErrorLog.txt";

                using (StreamWriter sw = File.CreateText(logFilePath))
                {
                    sw.Write(errorLog);
                }
            }
            else
            {
                MessageBox.Show("Parameters have been assigned.", "Success!", MessageBoxButtons.OK);
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
            if (fd.FileName != "")
            {
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

                        string newPath = Path.GetExtension(xlPath) == ".xlsx" ? Path.GetDirectoryName(xlPath) + @"\" + Path.GetFileNameWithoutExtension(xlPath) + "-temp.xlsx" : Path.GetDirectoryName(xlPath) + @"\" + Path.GetFileNameWithoutExtension(xlPath) + "-temp.xlsm";
                        File.Copy(xlPath, newPath);
                        xlPath = newPath;
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
            
        }

        private void assnbtn_Click(object sender, EventArgs e)
        {  
            for(int i = 1; i <= parCnt; i++) 
                AssignParameterValues(i);
        }

        private void closebtn_Click(object sender, EventArgs e)
        {
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
            string[] fn = (new FilteredElementCollector(rvtDoc)
                .OfClass(typeof(Family))
                .Where(q => (q as Family).FamilyCategory.Name == catDrop.SelectedItem.ToString())
                .Select(q => q.Name) as IEnumerable<string>).ToArray();

            familyDrop.Items.Clear();
            familyDrop.Items.AddRange(fn);
        }

        private void keycolumndrop_SelectedIndexChanged(object sender, EventArgs e)
        {
            sc1.Items.Clear();
            sc1.Items.AddRange(cols.ToArray());
            sc1.Items.Remove(keyColDrop.SelectedItem);
            if(cols.Count > 1)
                sc1.SelectedIndex = 0;
        }

        private void familydrop_SelectedIndexChanged(object sender, EventArgs e)
        {
            dp1.Items.Clear();

            FamilySymbol fs = new FilteredElementCollector(rvtDoc)
                .OfClass(typeof(FamilySymbol))
                .Where(q => (q as FamilySymbol).Family.Name == familyDrop.SelectedItem.ToString())
                .FirstOrDefault() as FamilySymbol;
            foreach(Autodesk.Revit.DB.Parameter par in fs.Parameters)
            {
                if (!par.IsReadOnly)
                {
                    dp1.Items.Add(par.Definition.Name + " (type)");
                }
            }

            FamilyInstance fi = new FilteredElementCollector(rvtDoc)
                .OfClass(typeof(FamilyInstance))
                .Where(q => (q as FamilyInstance).Symbol.Family.Name == familyDrop.SelectedItem.ToString())
                .FirstOrDefault() as FamilyInstance;
            if (fi != null)
            {
                foreach (Autodesk.Revit.DB.Parameter par in fi.Parameters)
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

            foreach(ComboBox cb in parDrops)
            {
                if(parDrops.IndexOf(cb) != 0)
                {
                    cb.Items.Clear();
                    foreach (string par in dp1.Items)
                    {
                        typeInst = par.Substring(par.Length - 5, 4);
                        if (byType && typeInst == "type")
                            cb.Items.Add(par);
                        else if (!byType && typeInst == "inst")
                            cb.Items.Add(par);

                    }
                    cb.SelectedIndex = 0;
                }
                
            }
        }

        private void ExcelFormClose(object sender, FormClosingEventArgs e)
        {
            if(xl != null)
                CleanupExcel();
        }

        private void addbtn_Click(object sender, EventArgs e)
        {

            this.Height += 35;
            colDrops.Add(new ComboBox());
            colDrops[parCnt].Size = colDrops[parCnt - 1].Size;
            this.Controls.Add(colDrops[parCnt]);
            colDrops[parCnt].Location = colDrops[parCnt - 1].Location;
            colDrops[parCnt].Width = colDrops[parCnt - 1].Width;
            colDrops[parCnt].DropDownStyle = ComboBoxStyle.DropDownList;
            colDrops[parCnt].Top += 35;
            colDrops[parCnt].DropDownWidth = colDrops[parCnt - 1].DropDownWidth;

            parDrops.Add(new ComboBox());
            parDrops[parCnt].Size = parDrops[parCnt - 1].Size;
            this.Controls.Add(parDrops[parCnt]);
            parDrops[parCnt].Location = parDrops[parCnt - 1].Location;
            parDrops[parCnt].Width = parDrops[parCnt - 1].Width;
            parDrops[parCnt].DropDownStyle = ComboBoxStyle.DropDownList;
            parDrops[parCnt].Top += 35;
            parDrops[parCnt].DropDownWidth = parDrops[parCnt-1].DropDownWidth;

            colDrops[parCnt].Items.AddRange(cols.ToArray());
            colDrops[parCnt].Items.Remove(keyColDrop.SelectedItem);

            string typeInst = "";

            foreach(string par in dp1.Items)
            {
                typeInst = par.Substring(par.Length - 5, 4);
                if (byType && typeInst == "type")
                    parDrops[parCnt].Items.Add(par);
                else if (!byType && typeInst == "inst")
                    parDrops[parCnt].Items.Add(par);

            }
            if(parDrops[parCnt].Items.Count > 0)
                parDrops[parCnt].SelectedIndex = 0;

            if (cols.Count >= parCnt + 2)
                colDrops[parCnt].SelectedIndex = parCnt;
            else if (cols.Count > 2)
                colDrops[parCnt].SelectedIndex = cols.Count - 2;
            else if (cols.Count > 1)
                colDrops[parCnt].SelectedIndex = 0;

            parCnt++;
        }

        private void subtractbtn_Click(object sender, EventArgs e)
        {
            if(parCnt > 1)
            {
                this.Height -= 35;
                this.Controls.Remove(colDrops[parCnt - 1]);
                this.Controls.Remove(parDrops[parCnt - 1]);
                colDrops.RemoveAt(colDrops.Count - 1);
                parDrops.RemoveAt(parDrops.Count - 1);
                parCnt--;
            }
        }
    }
}
