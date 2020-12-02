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
            this.filelocationtxt = new System.Windows.Forms.TextBox();
            this.xlfilebtn = new System.Windows.Forms.Button();
            this.xlgroup = new System.Windows.Forms.GroupBox();
            this.hdrLbl = new System.Windows.Forms.Label();
            this.hdrRowCtrl = new System.Windows.Forms.NumericUpDown();
            this.keyColDrop = new System.Windows.Forms.ComboBox();
            this.keycolumnlbl = new System.Windows.Forms.Label();
            this.wkshtDrop = new System.Windows.Forms.ComboBox();
            this.Wkshtlbl = new System.Windows.Forms.Label();
            this.rvtgroup = new System.Windows.Forms.GroupBox();
            this.catDrop = new System.Windows.Forms.ComboBox();
            this.catlbl = new System.Windows.Forms.Label();
            this.familyDrop = new System.Windows.Forms.ComboBox();
            this.familylbl = new System.Windows.Forms.Label();
            this.srclbl = new System.Windows.Forms.Label();
            this.destlbl = new System.Windows.Forms.Label();
            this.addbtn = new System.Windows.Forms.Button();
            this.subtractbtn = new System.Windows.Forms.Button();
            this.assnbtn = new System.Windows.Forms.Button();
            this.closebtn = new System.Windows.Forms.Button();
            this.getColsBtn = new System.Windows.Forms.Button();
            this.typeInstLbl = new System.Windows.Forms.Label();
            this.assignbylbl = new System.Windows.Forms.Label();
            this.sc1 = new System.Windows.Forms.ComboBox();
            this.dp1 = new System.Windows.Forms.ComboBox();
            this.xlgroup.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.hdrRowCtrl)).BeginInit();
            this.rvtgroup.SuspendLayout();
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
            // filelocationtxt
            // 
            this.filelocationtxt.Location = new System.Drawing.Point(112, 37);
            this.filelocationtxt.Name = "filelocationtxt";
            this.filelocationtxt.ReadOnly = true;
            this.filelocationtxt.Size = new System.Drawing.Size(259, 22);
            this.filelocationtxt.TabIndex = 1;
            // 
            // xlfilebtn
            // 
            this.xlfilebtn.Location = new System.Drawing.Point(378, 36);
            this.xlfilebtn.Name = "xlfilebtn";
            this.xlfilebtn.Size = new System.Drawing.Size(31, 23);
            this.xlfilebtn.TabIndex = 2;
            this.xlfilebtn.Text = "...";
            this.xlfilebtn.UseVisualStyleBackColor = true;
            this.xlfilebtn.Click += new System.EventHandler(this.xlfilebtn_Click);
            // 
            // xlgroup
            // 
            this.xlgroup.Controls.Add(this.hdrLbl);
            this.xlgroup.Controls.Add(this.hdrRowCtrl);
            this.xlgroup.Controls.Add(this.keyColDrop);
            this.xlgroup.Controls.Add(this.keycolumnlbl);
            this.xlgroup.Controls.Add(this.wkshtDrop);
            this.xlgroup.Controls.Add(this.Wkshtlbl);
            this.xlgroup.Controls.Add(this.xllbl);
            this.xlgroup.Controls.Add(this.xlfilebtn);
            this.xlgroup.Controls.Add(this.filelocationtxt);
            this.xlgroup.Location = new System.Drawing.Point(12, 12);
            this.xlgroup.Name = "xlgroup";
            this.xlgroup.Size = new System.Drawing.Size(427, 184);
            this.xlgroup.TabIndex = 3;
            this.xlgroup.TabStop = false;
            this.xlgroup.Text = "Excel";
            // 
            // hdrLbl
            // 
            this.hdrLbl.AutoSize = true;
            this.hdrLbl.Location = new System.Drawing.Point(19, 116);
            this.hdrLbl.Name = "hdrLbl";
            this.hdrLbl.Size = new System.Drawing.Size(90, 17);
            this.hdrLbl.TabIndex = 9;
            this.hdrLbl.Text = "Header Row:";
            // 
            // hdrRowCtrl
            // 
            this.hdrRowCtrl.Location = new System.Drawing.Point(112, 114);
            this.hdrRowCtrl.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.hdrRowCtrl.Name = "hdrRowCtrl";
            this.hdrRowCtrl.Size = new System.Drawing.Size(75, 22);
            this.hdrRowCtrl.TabIndex = 4;
            this.hdrRowCtrl.ThousandsSeparator = true;
            this.hdrRowCtrl.Value = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.hdrRowCtrl.ValueChanged += new System.EventHandler(this.wkshtDrop_SelectedIndexChanged);
            // 
            // keyColDrop
            // 
            this.keyColDrop.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.Suggest;
            this.keyColDrop.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.ListItems;
            this.keyColDrop.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.keyColDrop.FormattingEnabled = true;
            this.keyColDrop.Location = new System.Drawing.Point(112, 150);
            this.keyColDrop.Name = "keyColDrop";
            this.keyColDrop.Size = new System.Drawing.Size(259, 24);
            this.keyColDrop.TabIndex = 5;
            this.keyColDrop.SelectedIndexChanged += new System.EventHandler(this.keycolumndrop_SelectedIndexChanged);
            // 
            // keycolumnlbl
            // 
            this.keycolumnlbl.AutoSize = true;
            this.keycolumnlbl.Location = new System.Drawing.Point(19, 153);
            this.keycolumnlbl.Name = "keycolumnlbl";
            this.keycolumnlbl.Size = new System.Drawing.Size(87, 17);
            this.keycolumnlbl.TabIndex = 2;
            this.keycolumnlbl.Text = "Key Column:";
            // 
            // wkshtDrop
            // 
            this.wkshtDrop.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.Suggest;
            this.wkshtDrop.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.ListItems;
            this.wkshtDrop.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.wkshtDrop.FormattingEnabled = true;
            this.wkshtDrop.Location = new System.Drawing.Point(112, 74);
            this.wkshtDrop.Name = "wkshtDrop";
            this.wkshtDrop.Size = new System.Drawing.Size(259, 24);
            this.wkshtDrop.TabIndex = 3;
            this.wkshtDrop.SelectedIndexChanged += new System.EventHandler(this.wkshtDrop_SelectedIndexChanged);
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
            this.rvtgroup.Controls.Add(this.catDrop);
            this.rvtgroup.Controls.Add(this.catlbl);
            this.rvtgroup.Controls.Add(this.familyDrop);
            this.rvtgroup.Controls.Add(this.familylbl);
            this.rvtgroup.Location = new System.Drawing.Point(12, 202);
            this.rvtgroup.Name = "rvtgroup";
            this.rvtgroup.Size = new System.Drawing.Size(427, 134);
            this.rvtgroup.TabIndex = 4;
            this.rvtgroup.TabStop = false;
            this.rvtgroup.Text = "Revit";
            // 
            // catDrop
            // 
            this.catDrop.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.Suggest;
            this.catDrop.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.ListItems;
            this.catDrop.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.catDrop.FormattingEnabled = true;
            this.catDrop.Location = new System.Drawing.Point(112, 37);
            this.catDrop.Name = "catDrop";
            this.catDrop.Size = new System.Drawing.Size(259, 24);
            this.catDrop.TabIndex = 6;
            this.catDrop.SelectedIndexChanged += new System.EventHandler(this.catDrop_SelectedIndexChanged);
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
            // familyDrop
            // 
            this.familyDrop.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.Suggest;
            this.familyDrop.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.ListItems;
            this.familyDrop.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.familyDrop.FormattingEnabled = true;
            this.familyDrop.Location = new System.Drawing.Point(112, 81);
            this.familyDrop.Name = "familyDrop";
            this.familyDrop.Size = new System.Drawing.Size(259, 24);
            this.familyDrop.Sorted = true;
            this.familyDrop.TabIndex = 7;
            this.familyDrop.SelectedIndexChanged += new System.EventHandler(this.familydrop_SelectedIndexChanged);
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
            // srclbl
            // 
            this.srclbl.AutoSize = true;
            this.srclbl.Location = new System.Drawing.Point(15, 352);
            this.srclbl.Name = "srclbl";
            this.srclbl.Size = new System.Drawing.Size(108, 17);
            this.srclbl.TabIndex = 6;
            this.srclbl.Text = "Source Column:";
            // 
            // destlbl
            // 
            this.destlbl.AutoSize = true;
            this.destlbl.Location = new System.Drawing.Point(227, 355);
            this.destlbl.Name = "destlbl";
            this.destlbl.Size = new System.Drawing.Size(153, 17);
            this.destlbl.TabIndex = 7;
            this.destlbl.Text = "Destination Parameter:";
            // 
            // addbtn
            // 
            this.addbtn.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.addbtn.Location = new System.Drawing.Point(384, 411);
            this.addbtn.Name = "addbtn";
            this.addbtn.Size = new System.Drawing.Size(25, 23);
            this.addbtn.TabIndex = 10;
            this.addbtn.Text = "+";
            this.addbtn.UseVisualStyleBackColor = true;
            this.addbtn.Click += new System.EventHandler(this.addbtn_Click);
            // 
            // subtractbtn
            // 
            this.subtractbtn.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.subtractbtn.Location = new System.Drawing.Point(415, 411);
            this.subtractbtn.Name = "subtractbtn";
            this.subtractbtn.Size = new System.Drawing.Size(25, 23);
            this.subtractbtn.TabIndex = 11;
            this.subtractbtn.Text = "-";
            this.subtractbtn.UseVisualStyleBackColor = true;
            this.subtractbtn.Click += new System.EventHandler(this.subtractbtn_Click);
            // 
            // assnbtn
            // 
            this.assnbtn.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.assnbtn.Location = new System.Drawing.Point(262, 461);
            this.assnbtn.Name = "assnbtn";
            this.assnbtn.Size = new System.Drawing.Size(86, 28);
            this.assnbtn.TabIndex = 13;
            this.assnbtn.Text = "Assign";
            this.assnbtn.UseVisualStyleBackColor = true;
            this.assnbtn.Click += new System.EventHandler(this.assnbtn_Click);
            // 
            // closebtn
            // 
            this.closebtn.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.closebtn.Location = new System.Drawing.Point(354, 461);
            this.closebtn.Name = "closebtn";
            this.closebtn.Size = new System.Drawing.Size(86, 28);
            this.closebtn.TabIndex = 14;
            this.closebtn.Text = "Close";
            this.closebtn.UseVisualStyleBackColor = true;
            this.closebtn.Click += new System.EventHandler(this.closebtn_Click);
            // 
            // getcolsbtn
            // 
            this.getColsBtn.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.getColsBtn.Location = new System.Drawing.Point(12, 461);
            this.getColsBtn.Name = "getcolsbtn";
            this.getColsBtn.Size = new System.Drawing.Size(124, 28);
            this.getColsBtn.TabIndex = 12;
            this.getColsBtn.Text = "Get All Columns";
            this.getColsBtn.UseVisualStyleBackColor = true;
            // 
            // typeInstLbl
            // 
            this.typeInstLbl.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.typeInstLbl.AutoSize = true;
            this.typeInstLbl.Font = new System.Drawing.Font("Microsoft Sans Serif", 7.8F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.typeInstLbl.Location = new System.Drawing.Point(15, 422);
            this.typeInstLbl.Name = "typeInstLbl";
            this.typeInstLbl.Size = new System.Drawing.Size(0, 17);
            this.typeInstLbl.TabIndex = 13;
            // 
            // assignbylbl
            // 
            this.assignbylbl.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.assignbylbl.AutoSize = true;
            this.assignbylbl.Location = new System.Drawing.Point(15, 420);
            this.assignbylbl.Name = "assignbylbl";
            this.assignbylbl.Size = new System.Drawing.Size(0, 17);
            this.assignbylbl.TabIndex = 6;
            // 
            // sc1
            // 
            this.sc1.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.Suggest;
            this.sc1.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.ListItems;
            this.sc1.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.sc1.FormattingEnabled = true;
            this.sc1.Location = new System.Drawing.Point(18, 378);
            this.sc1.Name = "sc1";
            this.sc1.Size = new System.Drawing.Size(205, 24);
            this.sc1.TabIndex = 8;
            // 
            // dp1
            // 
            this.dp1.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.Suggest;
            this.dp1.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.ListItems;
            this.dp1.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.dp1.DropDownWidth = 209;
            this.dp1.FormattingEnabled = true;
            this.dp1.Location = new System.Drawing.Point(229, 378);
            this.dp1.Name = "dp1";
            this.dp1.Size = new System.Drawing.Size(209, 24);
            this.dp1.Sorted = true;
            this.dp1.TabIndex = 9;
            this.dp1.SelectedIndexChanged += new System.EventHandler(this.dp1_SelectedIndexChanged);
            // 
            // ExcelAssignFrm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(452, 501);
            this.Controls.Add(this.sc1);
            this.Controls.Add(this.dp1);
            this.Controls.Add(this.typeInstLbl);
            this.Controls.Add(this.getColsBtn);
            this.Controls.Add(this.closebtn);
            this.Controls.Add(this.assnbtn);
            this.Controls.Add(this.subtractbtn);
            this.Controls.Add(this.addbtn);
            this.Controls.Add(this.destlbl);
            this.Controls.Add(this.assignbylbl);
            this.Controls.Add(this.srclbl);
            this.Controls.Add(this.rvtgroup);
            this.Controls.Add(this.xlgroup);
            this.Name = "ExcelAssignFrm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Excel Assign";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.ExcelFormClose);
            this.xlgroup.ResumeLayout(false);
            this.xlgroup.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.hdrRowCtrl)).EndInit();
            this.rvtgroup.ResumeLayout(false);
            this.rvtgroup.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label xllbl;
        private System.Windows.Forms.TextBox filelocationtxt;
        private System.Windows.Forms.Button xlfilebtn;
        private System.Windows.Forms.GroupBox xlgroup;
        private System.Windows.Forms.ComboBox keyColDrop;
        private System.Windows.Forms.Label keycolumnlbl;
        private System.Windows.Forms.ComboBox wkshtDrop;
        private System.Windows.Forms.Label Wkshtlbl;
        private System.Windows.Forms.GroupBox rvtgroup;
        private System.Windows.Forms.ComboBox familyDrop;
        private System.Windows.Forms.Label familylbl;
        private System.Windows.Forms.ComboBox catDrop;
        private System.Windows.Forms.Label catlbl;
        private System.Windows.Forms.Label srclbl;
        private System.Windows.Forms.Label destlbl;
        private System.Windows.Forms.Button addbtn;
        private System.Windows.Forms.Button subtractbtn;
        private System.Windows.Forms.Button assnbtn;
        private System.Windows.Forms.Button closebtn;
        private System.Windows.Forms.Button getColsBtn;
        private System.Windows.Forms.Label typeInstLbl;
        private System.Windows.Forms.Label assignbylbl;
        private System.Windows.Forms.Label hdrLbl;
        private System.Windows.Forms.NumericUpDown hdrRowCtrl;
        private System.Windows.Forms.ComboBox sc1;
        private System.Windows.Forms.ComboBox dp1;
    }
}