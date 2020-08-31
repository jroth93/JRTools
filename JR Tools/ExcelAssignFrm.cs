using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
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
    public partial class ExcelAssignFrm : System.Windows.Forms.Form
    {
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
            catdrop.Items.AddRange(clist.ToArray());
            catdrop.SelectedIndex = 0;

        }

        private Document rvtdoc;
        private Excel.Application xl;
        private Excel.Workbook xlwb;

        private void cleanupExcel()
        {
            GC.Collect();
            GC.WaitForPendingFinalizers();
            xlwb.Close(false);
            Marshal.FinalReleaseComObject(xlwb);
            xl.Quit();
            Marshal.FinalReleaseComObject(xl);
        }

        private void xlfilebtn_Click(object sender, EventArgs e)
        {
            OpenFileDialog fd = new OpenFileDialog();
            fd.Title = "Browse to Excel file.";
            fd.ShowDialog();
            filelocationtxt.Text = fd.FileName;

            xl = new Excel.Application();
            xl.Workbooks.Open(filelocationtxt.Text, 0, true);
            xlwb = xl.ActiveWorkbook;

            String[] xlsheets = new String[xlwb.Worksheets.Count];
            int i = 0;
            foreach (Excel.Worksheet wSheet in xlwb.Worksheets)
            {
                xlsheets[i] = wSheet.Name;
                i++;
            }
            Wkshtdropdown.Items.AddRange(xlsheets);
            Wkshtdropdown.SelectedIndex = 0;
            
        }

        private void okbtn_Click(object sender, EventArgs e)
        {  


            cleanupExcel();
            this.Close();
        }


        private void cancelbtn_Click(object sender, EventArgs e)
        {
            cleanupExcel();
        }

        private void Wkshtdropdown_SelectedIndexChanged(object sender, EventArgs e)
        {
            List<string> cols = new List<string>();

            Excel.Worksheet xlws = xlwb.Worksheets.Item[Wkshtdropdown.SelectedIndex + 1];
            string cellval = xlws.Cells[1, 1].Value == null ? "" : xlws.Cells[1, 1].Value;
            int i = 1;

            while (cellval != null)
            {
                cols.Add(cellval);
                i++;
                cellval = xlws.Cells[1, i].Value;
            }

            keycolumndrop.Items.Clear();
            keycolumndrop.Items.AddRange(cols.ToArray());
            keycolumndrop.SelectedIndex = 0;
        }

        private void catdrop_SelectedIndexChanged(object sender, EventArgs e)
        {
            string[] fn = (new FilteredElementCollector(rvtdoc)
                .OfClass(typeof(Family))
                .Where(q => (q as Family).FamilyCategory.Name == catdrop.SelectedItem.ToString())
                .Select(q => q.Name) as IEnumerable<string>).ToArray();

            familydrop.Items.Clear();
            familydrop.Items.AddRange(fn);
        }

        private void keycolumndrop_SelectedIndexChanged(object sender, EventArgs e)
        {
            string[] kcditems = new string[keycolumndrop.Items.Count];
            keycolumndrop.Items.CopyTo(kcditems, 0);
            sc1.Items.Clear();
            sc1.Items.AddRange(kcditems);
            sc1.Items.Remove(keycolumndrop.SelectedItem);
        }

        private void familydrop_SelectedIndexChanged(object sender, EventArgs e)
        {
            FamilySymbol fs = new FilteredElementCollector(rvtdoc)
                .OfClass(typeof(FamilySymbol))
                .Where(q => (q as FamilySymbol).Family.Name == familydrop.SelectedItem.ToString())
                .FirstOrDefault() as FamilySymbol;
            dp1.Items.Clear();
            foreach(Parameter par in fs.Parameters)
            {
                dp1.Items.Add(par.Definition.Name);
            }

            FamilyInstance fi = new FilteredElementCollector(rvtdoc)
                .OfClass(typeof(FamilyInstance))
                .Where(q => (q as FamilyInstance).Symbol.Family.Name == familydrop.SelectedItem.ToString())
                .FirstOrDefault() as FamilyInstance;
            foreach (Parameter par in fi.Parameters)
            {
                dp1.Items.Add(par.Definition.Name);
            }

        }
    }
}
