using System;
using System.Collections.Generic;
using System.Windows.Forms;

namespace Proficient
{
    [Autodesk.Revit.Attributes.Transaction(Autodesk.Revit.Attributes.TransactionMode.Manual)]
    public partial class ExcelAssignFrm : System.Windows.Forms.Form
    {
        private int parCnt = 1;
        private String[] cols;
        private bool byType;
        private List<ComboBox> colDrops = new List<ComboBox>();
        private List<ComboBox> parDrops = new List<ComboBox>();

        public ExcelAssignFrm()
        {
            InitializeComponent();
            getColsBtn.Visible = false;            
            catDrop.Items.AddRange(ExcelAssign.GetCategories());
            catDrop.SelectedIndex = 0;
            colDrops.Add(sc1);
            parDrops.Add(dp1);
        }

        private int DropDownWidth(ComboBox myCombo)
        {
            int maxWidth = 0;
            int temp = 0;
            Label label1 = new Label();

            foreach (var obj in myCombo.Items)
            {
                label1.Text = Convert.ToString(obj);
                temp = label1.PreferredWidth;
                if (temp > maxWidth)
                {
                    maxWidth = temp;
                }
            }
            label1.Dispose();
            return maxWidth + SystemInformation.VerticalScrollBarWidth;
        }        

        private void xlfilebtn_Click(object sender, EventArgs e)
        {
            OpenFileDialog fd = new OpenFileDialog();
            fd.Title = "Browse to Excel file.";
            fd.ValidateNames = false;
            fd.ShowDialog();
            if (fd.FileName != "")
            {
                filelocationtxt.Text = fd.FileName;
                String[] xlSheets = ExcelAssign.OpenExcel(fd.FileName);
                wkshtDrop.Items.AddRange(xlSheets);
                wkshtDrop.SelectedIndex = 0;
            }
        }

        private void assnbtn_Click(object sender, EventArgs e)
        {
            string errorLog = String.Empty;
            for (int i = 1; i <= parCnt; i++)
            {
                string parName = Convert.ToString(parDrops[i - 1].SelectedItem);
                parName = parName.Substring(0, parName.Length - 7);
                string familyName = Convert.ToString(familyDrop.SelectedItem);
                int keyCol = keyColDrop.SelectedIndex + 1;
                int parCol = keyColDrop.Items.IndexOf(colDrops[i - 1].SelectedItem) + 1;
                int startRow = Convert.ToInt32(hdrRowCtrl.Value) + 1;

                if(byType)
                {
                    errorLog += ExcelAssign.AssignParameterValuesType(familyName, parName, keyCol, startRow, parCol);
                }
                else
                {
                    errorLog += ExcelAssign.AssignParameterValuesInst(familyName, parName, keyCol, startRow, parCol);
                }
                
            }

            if (errorLog != String.Empty)
            {
                MessageBox.Show("There were errors in the parameter assignment. Please see the error log in the Excel file directory for details.", "Assignment Errors", MessageBoxButtons.OK);
                ExcelAssign.WriteErrorFile(errorLog);
            }
            else
            {
                MessageBox.Show("Parameters have been assigned.", "Success!", MessageBoxButtons.OK);
            }

        }

        private void closebtn_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void wkshtDrop_SelectedIndexChanged(object sender, EventArgs e)
        {
            int wsIndex = wkshtDrop.SelectedIndex + 1;
            int hdrRow = Convert.ToInt32(hdrRowCtrl.Value);
            cols = ExcelAssign.GetExcelColumns(wsIndex, hdrRow);

            keyColDrop.Items.Clear();
            keyColDrop.Items.AddRange(cols);
            keyColDrop.SelectedIndex = 0;

        }

        private void catDrop_SelectedIndexChanged(object sender, EventArgs e)
        {
            string catName = Convert.ToString(catDrop.SelectedItem);
            familyDrop.Items.Clear();
            familyDrop.Items.AddRange(ExcelAssign.GetFamiliesOfCategory(catName));
        }

        private void keycolumndrop_SelectedIndexChanged(object sender, EventArgs e)
        {
            sc1.Items.Clear();
            sc1.Items.AddRange(cols);
            sc1.Items.Remove(keyColDrop.SelectedItem);
            if(cols.Length > 1)
                sc1.SelectedIndex = 0;
        }

        private void familydrop_SelectedIndexChanged(object sender, EventArgs e)
        {
            dp1.Items.Clear();

            string familyName = Convert.ToString(familyDrop.SelectedItem);

            dp1.Items.AddRange(ExcelAssign.GetFamilyParameters(familyName));
            dp1.SelectedIndex = 0;
            dp1.DropDownWidth = DropDownWidth(dp1);
        }

        private void dp1_SelectedIndexChanged(object sender, EventArgs e)
        {
            string curItem = Convert.ToString(dp1.SelectedItem);
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
                        else if (byType && typeInst == "inst")
                            cb.Items.Add(par);

                    }
                    cb.SelectedIndex = 0;
                }
                
            }
        }

        private void ExcelFormClose(object sender, FormClosingEventArgs e)
        {
            ExcelAssign.CleanupExcel();
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

            colDrops[parCnt].Items.AddRange(cols);
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

            if (cols.Length >= parCnt + 2)
                colDrops[parCnt].SelectedIndex = parCnt;
            else if (cols.Length > 2)
                colDrops[parCnt].SelectedIndex = cols.Length - 2;
            else if (cols.Length > 1)
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
