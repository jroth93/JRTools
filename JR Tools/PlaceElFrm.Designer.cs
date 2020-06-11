namespace JR_Tools
{
    partial class PlaceElFrm
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
            this.radionumber = new System.Windows.Forms.RadioButton();
            this.radiooffset = new System.Windows.Forms.RadioButton();
            this.textBox1 = new System.Windows.Forms.TextBox();
            this.txtlabel = new System.Windows.Forms.Label();
            this.startoffsetlbl = new System.Windows.Forms.Label();
            this.startoffset = new System.Windows.Forms.TextBox();
            this.okbutton = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // radionumber
            // 
            this.radionumber.AutoSize = true;
            this.radionumber.Checked = true;
            this.radionumber.Location = new System.Drawing.Point(78, 22);
            this.radionumber.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.radionumber.Name = "radionumber";
            this.radionumber.Size = new System.Drawing.Size(106, 17);
            this.radionumber.TabIndex = 0;
            this.radionumber.TabStop = true;
            this.radionumber.Text = "Place by Number";
            this.radionumber.UseVisualStyleBackColor = true;
            this.radionumber.CheckedChanged += new System.EventHandler(this.radionumber_CheckedChanged);
            // 
            // radiooffset
            // 
            this.radiooffset.AutoSize = true;
            this.radiooffset.Location = new System.Drawing.Point(80, 51);
            this.radiooffset.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.radiooffset.Name = "radiooffset";
            this.radiooffset.Size = new System.Drawing.Size(97, 17);
            this.radiooffset.TabIndex = 1;
            this.radiooffset.Text = "Place by Offset";
            this.radiooffset.UseVisualStyleBackColor = true;
            this.radiooffset.CheckedChanged += new System.EventHandler(this.radiooffset_CheckedChanged);
            // 
            // textBox1
            // 
            this.textBox1.Location = new System.Drawing.Point(196, 86);
            this.textBox1.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.textBox1.Name = "textBox1";
            this.textBox1.Size = new System.Drawing.Size(47, 20);
            this.textBox1.TabIndex = 2;
            // 
            // txtlabel
            // 
            this.txtlabel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.txtlabel.ImageAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.txtlabel.Location = new System.Drawing.Point(21, 88);
            this.txtlabel.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.txtlabel.Name = "txtlabel";
            this.txtlabel.Size = new System.Drawing.Size(161, 15);
            this.txtlabel.TabIndex = 3;
            this.txtlabel.Text = "label1";
            this.txtlabel.TextAlign = System.Drawing.ContentAlignment.TopRight;
            // 
            // startoffsetlbl
            // 
            this.startoffsetlbl.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.startoffsetlbl.AutoSize = true;
            this.startoffsetlbl.Location = new System.Drawing.Point(82, 121);
            this.startoffsetlbl.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.startoffsetlbl.Name = "startoffsetlbl";
            this.startoffsetlbl.Size = new System.Drawing.Size(101, 13);
            this.startoffsetlbl.TabIndex = 5;
            this.startoffsetlbl.Text = "Offset from Start (ft):";
            this.startoffsetlbl.TextAlign = System.Drawing.ContentAlignment.TopRight;
            // 
            // startoffset
            // 
            this.startoffset.Location = new System.Drawing.Point(196, 119);
            this.startoffset.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.startoffset.Name = "startoffset";
            this.startoffset.Size = new System.Drawing.Size(47, 20);
            this.startoffset.TabIndex = 4;
            this.startoffset.Text = "0";
            // 
            // okbutton
            // 
            this.okbutton.Location = new System.Drawing.Point(85, 155);
            this.okbutton.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.okbutton.Name = "okbutton";
            this.okbutton.Size = new System.Drawing.Size(94, 30);
            this.okbutton.TabIndex = 6;
            this.okbutton.Text = "OK";
            this.okbutton.UseVisualStyleBackColor = true;
            this.okbutton.Click += new System.EventHandler(this.okbutton_Click);
            // 
            // PlaceElFrm
            // 
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
            this.ClientSize = new System.Drawing.Size(261, 199);
            this.Controls.Add(this.okbutton);
            this.Controls.Add(this.startoffsetlbl);
            this.Controls.Add(this.startoffset);
            this.Controls.Add(this.txtlabel);
            this.Controls.Add(this.textBox1);
            this.Controls.Add(this.radiooffset);
            this.Controls.Add(this.radionumber);
            this.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.Name = "PlaceElFrm";
            this.Text = "Place Elements";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        public System.Windows.Forms.RadioButton radionumber;
        public System.Windows.Forms.RadioButton radiooffset;
        public System.Windows.Forms.TextBox textBox1;
        private System.Windows.Forms.Label txtlabel;
        private System.Windows.Forms.Label startoffsetlbl;
        public System.Windows.Forms.TextBox startoffset;
        private System.Windows.Forms.Button okbutton;
    }
}