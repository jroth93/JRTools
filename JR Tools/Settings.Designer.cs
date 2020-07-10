namespace JR_Tools
{
    partial class SettingsForm
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
            this.defaultworkset = new System.Windows.Forms.ComboBox();
            this.label1 = new System.Windows.Forms.Label();
            this.checkBox1 = new System.Windows.Forms.CheckBox();
            this.okbutton = new System.Windows.Forms.Button();
            this.cancelbutton = new System.Windows.Forms.Button();
            this.wkstbox = new System.Windows.Forms.GroupBox();
            this.frame = new System.Windows.Forms.GroupBox();
            this.mechbox = new System.Windows.Forms.GroupBox();
            this.pipespacelbl = new System.Windows.Forms.Label();
            this.pipespaceupdown = new System.Windows.Forms.NumericUpDown();
            this.txtsettingsbox = new System.Windows.Forms.GroupBox();
            this.txtstylelbl = new System.Windows.Forms.Label();
            this.comboBox1 = new System.Windows.Forms.ComboBox();
            this.wkstbox.SuspendLayout();
            this.frame.SuspendLayout();
            this.mechbox.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pipespaceupdown)).BeginInit();
            this.txtsettingsbox.SuspendLayout();
            this.SuspendLayout();
            // 
            // defaultworkset
            // 
            this.defaultworkset.FormattingEnabled = true;
            this.defaultworkset.Location = new System.Drawing.Point(133, 36);
            this.defaultworkset.Margin = new System.Windows.Forms.Padding(2);
            this.defaultworkset.Name = "defaultworkset";
            this.defaultworkset.Size = new System.Drawing.Size(260, 24);
            this.defaultworkset.Sorted = true;
            this.defaultworkset.TabIndex = 0;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.Location = new System.Drawing.Point(23, 39);
            this.label1.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(106, 16);
            this.label1.TabIndex = 1;
            this.label1.Text = "Default Workset:";
            // 
            // checkBox1
            // 
            this.checkBox1.AutoSize = true;
            this.checkBox1.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.checkBox1.Location = new System.Drawing.Point(26, 74);
            this.checkBox1.Margin = new System.Windows.Forms.Padding(2);
            this.checkBox1.Name = "checkBox1";
            this.checkBox1.Size = new System.Drawing.Size(367, 20);
            this.checkBox1.TabIndex = 2;
            this.checkBox1.Text = "Switch to enlarged workset upon change to enlarged view";
            this.checkBox1.UseVisualStyleBackColor = true;
            // 
            // okbutton
            // 
            this.okbutton.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.okbutton.Location = new System.Drawing.Point(266, 430);
            this.okbutton.Margin = new System.Windows.Forms.Padding(2);
            this.okbutton.Name = "okbutton";
            this.okbutton.Size = new System.Drawing.Size(76, 23);
            this.okbutton.TabIndex = 3;
            this.okbutton.Text = "Save";
            this.okbutton.UseVisualStyleBackColor = true;
            this.okbutton.Click += new System.EventHandler(this.okbutton_Click);
            // 
            // cancelbutton
            // 
            this.cancelbutton.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.cancelbutton.Location = new System.Drawing.Point(358, 430);
            this.cancelbutton.Margin = new System.Windows.Forms.Padding(2);
            this.cancelbutton.Name = "cancelbutton";
            this.cancelbutton.Size = new System.Drawing.Size(76, 23);
            this.cancelbutton.TabIndex = 4;
            this.cancelbutton.Text = "Cancel";
            this.cancelbutton.UseVisualStyleBackColor = true;
            this.cancelbutton.Click += new System.EventHandler(this.cancelbutton_Click);
            // 
            // wkstbox
            // 
            this.wkstbox.BackColor = System.Drawing.SystemColors.ControlLightLight;
            this.wkstbox.Controls.Add(this.label1);
            this.wkstbox.Controls.Add(this.defaultworkset);
            this.wkstbox.Controls.Add(this.checkBox1);
            this.wkstbox.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.wkstbox.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.wkstbox.ForeColor = System.Drawing.SystemColors.ControlText;
            this.wkstbox.Location = new System.Drawing.Point(12, 283);
            this.wkstbox.Name = "wkstbox";
            this.wkstbox.Size = new System.Drawing.Size(427, 124);
            this.wkstbox.TabIndex = 5;
            this.wkstbox.TabStop = false;
            this.wkstbox.Text = "Worksets";
            // 
            // frame
            // 
            this.frame.BackColor = System.Drawing.SystemColors.ControlLightLight;
            this.frame.Controls.Add(this.txtsettingsbox);
            this.frame.Controls.Add(this.mechbox);
            this.frame.Location = new System.Drawing.Point(-5, -12);
            this.frame.Name = "frame";
            this.frame.Size = new System.Drawing.Size(462, 434);
            this.frame.TabIndex = 6;
            this.frame.TabStop = false;
            // 
            // mechbox
            // 
            this.mechbox.Controls.Add(this.pipespaceupdown);
            this.mechbox.Controls.Add(this.pipespacelbl);
            this.mechbox.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.mechbox.Location = new System.Drawing.Point(17, 225);
            this.mechbox.Name = "mechbox";
            this.mechbox.Size = new System.Drawing.Size(427, 64);
            this.mechbox.TabIndex = 0;
            this.mechbox.TabStop = false;
            this.mechbox.Text = "Mechanical";
            // 
            // pipespacelbl
            // 
            this.pipespacelbl.AutoSize = true;
            this.pipespacelbl.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.pipespacelbl.Location = new System.Drawing.Point(23, 30);
            this.pipespacelbl.Name = "pipespacelbl";
            this.pipespacelbl.Size = new System.Drawing.Size(259, 16);
            this.pipespacelbl.TabIndex = 0;
            this.pipespacelbl.Text = "Pipe Spacing on 1/8\" Plan:                  inches";
            // 
            // pipespaceupdown
            // 
            this.pipespaceupdown.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.pipespaceupdown.Location = new System.Drawing.Point(191, 28);
            this.pipespaceupdown.Name = "pipespaceupdown";
            this.pipespaceupdown.Size = new System.Drawing.Size(41, 22);
            this.pipespaceupdown.TabIndex = 1;
            // 
            // txtsettingsbox
            // 
            this.txtsettingsbox.Controls.Add(this.comboBox1);
            this.txtsettingsbox.Controls.Add(this.txtstylelbl);
            this.txtsettingsbox.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtsettingsbox.Location = new System.Drawing.Point(17, 154);
            this.txtsettingsbox.Name = "txtsettingsbox";
            this.txtsettingsbox.Size = new System.Drawing.Size(427, 65);
            this.txtsettingsbox.TabIndex = 1;
            this.txtsettingsbox.TabStop = false;
            this.txtsettingsbox.Text = "Text";
            // 
            // txtstylelbl
            // 
            this.txtstylelbl.AutoSize = true;
            this.txtstylelbl.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F);
            this.txtstylelbl.Location = new System.Drawing.Point(26, 27);
            this.txtstylelbl.Name = "txtstylelbl";
            this.txtstylelbl.Size = new System.Drawing.Size(125, 16);
            this.txtstylelbl.TabIndex = 0;
            this.txtstylelbl.Text = "Default Text Family:";
            // 
            // comboBox1
            // 
            this.comboBox1.FormattingEnabled = true;
            this.comboBox1.Location = new System.Drawing.Point(157, 24);
            this.comboBox1.Name = "comboBox1";
            this.comboBox1.Size = new System.Drawing.Size(236, 24);
            this.comboBox1.TabIndex = 1;
            // 
            // SettingsForm
            // 
            this.AcceptButton = this.okbutton;
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
            this.BackColor = System.Drawing.Color.Gainsboro;
            this.CancelButton = this.cancelbutton;
            this.ClientSize = new System.Drawing.Size(451, 461);
            this.Controls.Add(this.wkstbox);
            this.Controls.Add(this.cancelbutton);
            this.Controls.Add(this.okbutton);
            this.Controls.Add(this.frame);
            this.Margin = new System.Windows.Forms.Padding(2);
            this.Name = "SettingsForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Workset Settings";
            this.Load += new System.EventHandler(this.WorksetSettingsForm_Load);
            this.wkstbox.ResumeLayout(false);
            this.wkstbox.PerformLayout();
            this.frame.ResumeLayout(false);
            this.mechbox.ResumeLayout(false);
            this.mechbox.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pipespaceupdown)).EndInit();
            this.txtsettingsbox.ResumeLayout(false);
            this.txtsettingsbox.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        public System.Windows.Forms.ComboBox defaultworkset;
        private System.Windows.Forms.Label label1;
        public System.Windows.Forms.CheckBox checkBox1;
        private System.Windows.Forms.Button okbutton;
        private System.Windows.Forms.Button cancelbutton;
        private System.Windows.Forms.GroupBox wkstbox;
        private System.Windows.Forms.GroupBox frame;
        private System.Windows.Forms.GroupBox mechbox;
        public System.Windows.Forms.NumericUpDown pipespaceupdown;
        private System.Windows.Forms.Label pipespacelbl;
        private System.Windows.Forms.GroupBox txtsettingsbox;
        private System.Windows.Forms.ComboBox comboBox1;
        private System.Windows.Forms.Label txtstylelbl;
    }
}