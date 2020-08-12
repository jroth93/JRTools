namespace JR_Tools
{
    partial class ExcelAssignFrm
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.xllbl = new System.Windows.Forms.Label();
            this.textBox1 = new System.Windows.Forms.TextBox();
            this.xlfilebtn = new System.Windows.Forms.Button();
            this.xlgroup = new System.Windows.Forms.GroupBox();
            this.keycolumndrop = new System.Windows.Forms.ComboBox();
            this.keycolumnlbl = new System.Windows.Forms.Label();
            this.Wkshtdropdown = new System.Windows.Forms.ComboBox();
            this.Wkshtlbl = new System.Windows.Forms.Label();
            this.rvtgroup = new System.Windows.Forms.GroupBox();
            this.catdrop = new System.Windows.Forms.ComboBox();
            this.catlbl = new System.Windows.Forms.Label();
            this.familydrop = new System.Windows.Forms.ComboBox();
            this.familylbl = new System.Windows.Forms.Label();
            this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            this.comboBox1 = new System.Windows.Forms.ComboBox();
            this.comboBox2 = new System.Windows.Forms.ComboBox();
            this.srclbl = new System.Windows.Forms.Label();
            this.destlbl = new System.Windows.Forms.Label();
            this.addbtn = new System.Windows.Forms.Button();
            this.subtractbtn = new System.Windows.Forms.Button();
            this.okbtn = new System.Windows.Forms.Button();
            this.cancelbtn = new System.Windows.Forms.Button();
            this.getcolsbtn = new System.Windows.Forms.Button();
            this.typeinstlbl = new System.Windows.Forms.Label();
            this.xlgroup.SuspendLayout();
            this.rvtgroup.SuspendLayout();
            this.tableLayoutPanel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // xllbl
            // 
            this.xllbl.AutoSize = true;
            this.xllbl.Location = new System.Drawing.Point(15, 40);
            this.xllbl.Name = "xllbl";
            this.xllbl.Size = new System.Drawing.Size(92, 17);
            this.xllbl.TabIndex = 0;
            this.xllbl.Text = "File Location:";
            // 
            // textBox1
            // 
            this.textBox1.Location = new System.Drawing.Point(112, 37);
            this.textBox1.Name = "textBox1";
            this.textBox1.Size = new System.Drawing.Size(259, 22);
            this.textBox1.TabIndex = 1;
            // 
            // xlfilebtn
            // 
            this.xlfilebtn.Location = new System.Drawing.Point(378, 36);
            this.xlfilebtn.Name = "xlfilebtn";
            this.xlfilebtn.Size = new System.Drawing.Size(31, 23);
            this.xlfilebtn.TabIndex = 2;
            this.xlfilebtn.Text = "...";
            this.xlfilebtn.UseVisualStyleBackColor = true;
            // 
            // xlgroup
            // 
            this.xlgroup.Controls.Add(this.keycolumndrop);
            this.xlgroup.Controls.Add(this.keycolumnlbl);
            this.xlgroup.Controls.Add(this.Wkshtdropdown);
            this.xlgroup.Controls.Add(this.Wkshtlbl);
            this.xlgroup.Controls.Add(this.xllbl);
            this.xlgroup.Controls.Add(this.xlfilebtn);
            this.xlgroup.Controls.Add(this.textBox1);
            this.xlgroup.Location = new System.Drawing.Point(12, 12);
            this.xlgroup.Name = "xlgroup";
            this.xlgroup.Size = new System.Drawing.Size(427, 170);
            this.xlgroup.TabIndex = 3;
            this.xlgroup.TabStop = false;
            this.xlgroup.Text = "Excel";
            // 
            // keycolumndrop
            // 
            this.keycolumndrop.FormattingEnabled = true;
            this.keycolumndrop.Location = new System.Drawing.Point(112, 114);
            this.keycolumndrop.Name = "keycolumndrop";
            this.keycolumndrop.Size = new System.Drawing.Size(259, 24);
            this.keycolumndrop.TabIndex = 5;
            // 
            // keycolumnlbl
            // 
            this.keycolumnlbl.AutoSize = true;
            this.keycolumnlbl.Location = new System.Drawing.Point(20, 117);
            this.keycolumnlbl.Name = "keycolumnlbl";
            this.keycolumnlbl.Size = new System.Drawing.Size(87, 17);
            this.keycolumnlbl.TabIndex = 2;
            this.keycolumnlbl.Text = "Key Column:";
            // 
            // Wkshtdropdown
            // 
            this.Wkshtdropdown.FormattingEnabled = true;
            this.Wkshtdropdown.Location = new System.Drawing.Point(112, 74);
            this.Wkshtdropdown.Name = "Wkshtdropdown";
            this.Wkshtdropdown.Size = new System.Drawing.Size(259, 24);
            this.Wkshtdropdown.TabIndex = 4;
            // 
            // Wkshtlbl
            // 
            this.Wkshtlbl.AutoSize = true;
            this.Wkshtlbl.Location = new System.Drawing.Point(27, 77);
            this.Wkshtlbl.Name = "Wkshtlbl";
            this.Wkshtlbl.Size = new System.Drawing.Size(80, 17);
            this.Wkshtlbl.TabIndex = 3;
            this.Wkshtlbl.Text = "Worksheet:";
            // 
            // rvtgroup
            // 
            this.rvtgroup.Controls.Add(this.catdrop);
            this.rvtgroup.Controls.Add(this.catlbl);
            this.rvtgroup.Controls.Add(this.familydrop);
            this.rvtgroup.Controls.Add(this.familylbl);
            this.rvtgroup.Location = new System.Drawing.Point(12, 188);
            this.rvtgroup.Name = "rvtgroup";
            this.rvtgroup.Size = new System.Drawing.Size(427, 134);
            this.rvtgroup.TabIndex = 4;
            this.rvtgroup.TabStop = false;
            this.rvtgroup.Text = "Revit";
            // 
            // catdrop
            // 
            this.catdrop.FormattingEnabled = true;
            this.catdrop.Location = new System.Drawing.Point(112, 37);
            this.catdrop.Name = "catdrop";
            this.catdrop.Size = new System.Drawing.Size(259, 24);
            this.catdrop.TabIndex = 5;
            // 
            // catlbl
            // 
            this.catlbl.AutoSize = true;
            this.catlbl.Location = new System.Drawing.Point(37, 37);
            this.catlbl.Name = "catlbl";
            this.catlbl.Size = new System.Drawing.Size(69, 17);
            this.catlbl.TabIndex = 4;
            this.catlbl.Text = "Category:";
            // 
            // familydrop
            // 
            this.familydrop.FormattingEnabled = true;
            this.familydrop.Location = new System.Drawing.Point(112, 81);
            this.familydrop.Name = "familydrop";
            this.familydrop.Size = new System.Drawing.Size(259, 24);
            this.familydrop.TabIndex = 3;
            // 
            // familylbl
            // 
            this.familylbl.AutoSize = true;
            this.familylbl.Location = new System.Drawing.Point(54, 84);
            this.familylbl.Name = "familylbl";
            this.familylbl.Size = new System.Drawing.Size(52, 17);
            this.familylbl.TabIndex = 2;
            this.familylbl.Text = "Family:";
            // 
            // tableLayoutPanel1
            // 
            this.tableLayoutPanel1.ColumnCount = 2;
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 49.64871F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50.35129F));
            this.tableLayoutPanel1.Controls.Add(this.comboBox1, 1, 0);
            this.tableLayoutPanel1.Controls.Add(this.comboBox2, 0, 0);
            this.tableLayoutPanel1.Location = new System.Drawing.Point(12, 361);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.RowCount = 2;
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.tableLayoutPanel1.Size = new System.Drawing.Size(427, 30);
            this.tableLayoutPanel1.TabIndex = 5;
            // 
            // comboBox1
            // 
            this.comboBox1.FormattingEnabled = true;
            this.comboBox1.Location = new System.Drawing.Point(214, 3);
            this.comboBox1.Name = "comboBox1";
            this.comboBox1.Size = new System.Drawing.Size(209, 24);
            this.comboBox1.TabIndex = 0;
            // 
            // comboBox2
            // 
            this.comboBox2.FormattingEnabled = true;
            this.comboBox2.Location = new System.Drawing.Point(3, 3);
            this.comboBox2.Name = "comboBox2";
            this.comboBox2.Size = new System.Drawing.Size(205, 24);
            this.comboBox2.TabIndex = 1;
            // 
            // srclbl
            // 
            this.srclbl.AutoSize = true;
            this.srclbl.Location = new System.Drawing.Point(15, 338);
            this.srclbl.Name = "srclbl";
            this.srclbl.Size = new System.Drawing.Size(108, 17);
            this.srclbl.TabIndex = 6;
            this.srclbl.Text = "Source Column:";
            // 
            // destlbl
            // 
            this.destlbl.AutoSize = true;
            this.destlbl.Location = new System.Drawing.Point(227, 341);
            this.destlbl.Name = "destlbl";
            this.destlbl.Size = new System.Drawing.Size(153, 17);
            this.destlbl.TabIndex = 7;
            this.destlbl.Text = "Destination Parameter:";
            // 
            // addbtn
            // 
            this.addbtn.Location = new System.Drawing.Point(384, 397);
            this.addbtn.Name = "addbtn";
            this.addbtn.Size = new System.Drawing.Size(25, 23);
            this.addbtn.TabIndex = 8;
            this.addbtn.Text = "+";
            this.addbtn.UseVisualStyleBackColor = true;
            // 
            // subtractbtn
            // 
            this.subtractbtn.Location = new System.Drawing.Point(415, 397);
            this.subtractbtn.Name = "subtractbtn";
            this.subtractbtn.Size = new System.Drawing.Size(25, 23);
            this.subtractbtn.TabIndex = 9;
            this.subtractbtn.Text = "-";
            this.subtractbtn.UseVisualStyleBackColor = true;
            // 
            // okbtn
            // 
            this.okbtn.Location = new System.Drawing.Point(262, 461);
            this.okbtn.Name = "okbtn";
            this.okbtn.Size = new System.Drawing.Size(86, 28);
            this.okbtn.TabIndex = 10;
            this.okbtn.Text = "OK";
            this.okbtn.UseVisualStyleBackColor = true;
            // 
            // cancelbtn
            // 
            this.cancelbtn.Location = new System.Drawing.Point(354, 461);
            this.cancelbtn.Name = "cancelbtn";
            this.cancelbtn.Size = new System.Drawing.Size(86, 28);
            this.cancelbtn.TabIndex = 11;
            this.cancelbtn.Text = "Cancel";
            this.cancelbtn.UseVisualStyleBackColor = true;
            // 
            // getcolsbtn
            // 
            this.getcolsbtn.Location = new System.Drawing.Point(12, 461);
            this.getcolsbtn.Name = "getcolsbtn";
            this.getcolsbtn.Size = new System.Drawing.Size(124, 28);
            this.getcolsbtn.TabIndex = 12;
            this.getcolsbtn.Text = "Get All Columns";
            this.getcolsbtn.UseVisualStyleBackColor = true;
            // 
            // typeinstlbl
            // 
            this.typeinstlbl.AutoSize = true;
            this.typeinstlbl.Location = new System.Drawing.Point(15, 402);
            this.typeinstlbl.Name = "typeinstlbl";
            this.typeinstlbl.Size = new System.Drawing.Size(0, 17);
            this.typeinstlbl.TabIndex = 13;
            // 
            // ExcelAssignFrm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(452, 501);
            this.Controls.Add(this.typeinstlbl);
            this.Controls.Add(this.getcolsbtn);
            this.Controls.Add(this.cancelbtn);
            this.Controls.Add(this.okbtn);
            this.Controls.Add(this.subtractbtn);
            this.Controls.Add(this.addbtn);
            this.Controls.Add(this.destlbl);
            this.Controls.Add(this.srclbl);
            this.Controls.Add(this.tableLayoutPanel1);
            this.Controls.Add(this.rvtgroup);
            this.Controls.Add(this.xlgroup);
            this.Name = "ExcelAssignFrm";
            this.Text = "Excel Assign";
            this.xlgroup.ResumeLayout(false);
            this.xlgroup.PerformLayout();
            this.rvtgroup.ResumeLayout(false);
            this.rvtgroup.PerformLayout();
            this.tableLayoutPanel1.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label xllbl;
        private System.Windows.Forms.TextBox textBox1;
        private System.Windows.Forms.Button xlfilebtn;
        private System.Windows.Forms.GroupBox xlgroup;
        private System.Windows.Forms.ComboBox keycolumndrop;
        private System.Windows.Forms.Label keycolumnlbl;
        private System.Windows.Forms.ComboBox Wkshtdropdown;
        private System.Windows.Forms.Label Wkshtlbl;
        private System.Windows.Forms.GroupBox rvtgroup;
        private System.Windows.Forms.ComboBox familydrop;
        private System.Windows.Forms.Label familylbl;
        private System.Windows.Forms.ComboBox catdrop;
        private System.Windows.Forms.Label catlbl;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
        private System.Windows.Forms.ComboBox comboBox1;
        private System.Windows.Forms.ComboBox comboBox2;
        private System.Windows.Forms.Label srclbl;
        private System.Windows.Forms.Label destlbl;
        private System.Windows.Forms.Button addbtn;
        private System.Windows.Forms.Button subtractbtn;
        private System.Windows.Forms.Button okbtn;
        private System.Windows.Forms.Button cancelbtn;
        private System.Windows.Forms.Button getcolsbtn;
        private System.Windows.Forms.Label typeinstlbl;
    }
}