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
            this.txtsettingsbox = new System.Windows.Forms.GroupBox();
            this.defaulttext = new System.Windows.Forms.ComboBox();
            this.txtstylelbl = new System.Windows.Forms.Label();
            this.mechbox = new System.Windows.Forms.GroupBox();
            this.pipespaceupdown = new System.Windows.Forms.NumericUpDown();
            this.pipespacelbl = new System.Windows.Forms.Label();
            this.wkstbox.SuspendLayout();
            this.frame.SuspendLayout();
            this.txtsettingsbox.SuspendLayout();
            this.mechbox.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pipespaceupdown)).BeginInit();
            this.SuspendLayout();
            // 
            // defaultworkset
            // 
            this.defaultworkset.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.defaultworkset.FormattingEnabled = true;
            this.defaultworkset.Location = new System.Drawing.Point(128, 28);
            this.defaultworkset.Margin = new System.Windows.Forms.Padding(2);
            this.defaultworkset.Name = "defaultworkset";
            this.defaultworkset.Size = new System.Drawing.Size(259, 24);
            this.defaultworkset.Sorted = true;
            this.defaultworkset.TabIndex = 0;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.Location = new System.Drawing.Point(18, 31);
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
            this.checkBox1.Location = new System.Drawing.Point(21, 59);
            this.checkBox1.Margin = new System.Windows.Forms.Padding(2);
            this.checkBox1.Name = "checkBox1";
            this.checkBox1.Size = new System.Drawing.Size(324, 20);
            this.checkBox1.TabIndex = 2;
            this.checkBox1.Text = "Switch to enlarged and site worksets automatically";
            this.checkBox1.UseVisualStyleBackColor = true;
            // 
            // okbutton
            // 
            this.okbutton.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.okbutton.Location = new System.Drawing.Point(282, 249);
            this.okbutton.Margin = new System.Windows.Forms.Padding(2);
            this.okbutton.Name = "okbutton";
            this.okbutton.Size = new System.Drawing.Size(65, 25);
            this.okbutton.TabIndex = 3;
            this.okbutton.Text = "Save";
            this.okbutton.UseVisualStyleBackColor = true;
            this.okbutton.Click += new System.EventHandler(this.okbutton_Click);
            // 
            // cancelbutton
            // 
            this.cancelbutton.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.cancelbutton.Location = new System.Drawing.Point(354, 249);
            this.cancelbutton.Margin = new System.Windows.Forms.Padding(2);
            this.cancelbutton.Name = "cancelbutton";
            this.cancelbutton.Size = new System.Drawing.Size(65, 25);
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
            this.wkstbox.Location = new System.Drawing.Point(15, 132);
            this.wkstbox.Margin = new System.Windows.Forms.Padding(2);
            this.wkstbox.Name = "wkstbox";
            this.wkstbox.Padding = new System.Windows.Forms.Padding(2);
            this.wkstbox.Size = new System.Drawing.Size(408, 99);
            this.wkstbox.TabIndex = 5;
            this.wkstbox.TabStop = false;
            this.wkstbox.Text = "Worksets";
            // 
            // frame
            // 
            this.frame.BackColor = System.Drawing.SystemColors.ControlLightLight;
            this.frame.Controls.Add(this.wkstbox);
            this.frame.Controls.Add(this.txtsettingsbox);
            this.frame.Controls.Add(this.mechbox);
            this.frame.Location = new System.Drawing.Point(-4, -10);
            this.frame.Margin = new System.Windows.Forms.Padding(2);
            this.frame.Name = "frame";
            this.frame.Padding = new System.Windows.Forms.Padding(2);
            this.frame.Size = new System.Drawing.Size(437, 250);
            this.frame.TabIndex = 6;
            this.frame.TabStop = false;
            // 
            // txtsettingsbox
            // 
            this.txtsettingsbox.Controls.Add(this.defaulttext);
            this.txtsettingsbox.Controls.Add(this.txtstylelbl);
            this.txtsettingsbox.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtsettingsbox.Location = new System.Drawing.Point(15, 21);
            this.txtsettingsbox.Margin = new System.Windows.Forms.Padding(2);
            this.txtsettingsbox.Name = "txtsettingsbox";
            this.txtsettingsbox.Padding = new System.Windows.Forms.Padding(2);
            this.txtsettingsbox.Size = new System.Drawing.Size(408, 52);
            this.txtsettingsbox.TabIndex = 1;
            this.txtsettingsbox.TabStop = false;
            this.txtsettingsbox.Text = "Text";
            // 
            // defaulttext
            // 
            this.defaulttext.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.defaulttext.FormattingEnabled = true;
            this.defaulttext.Location = new System.Drawing.Point(150, 19);
            this.defaulttext.Margin = new System.Windows.Forms.Padding(2);
            this.defaulttext.Name = "defaulttext";
            this.defaulttext.Size = new System.Drawing.Size(237, 24);
            this.defaulttext.TabIndex = 1;
            // 
            // txtstylelbl
            // 
            this.txtstylelbl.AutoSize = true;
            this.txtstylelbl.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F);
            this.txtstylelbl.Location = new System.Drawing.Point(21, 22);
            this.txtstylelbl.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.txtstylelbl.Name = "txtstylelbl";
            this.txtstylelbl.Size = new System.Drawing.Size(125, 16);
            this.txtstylelbl.TabIndex = 0;
            this.txtstylelbl.Text = "Default Text Family:";
            // 
            // mechbox
            // 
            this.mechbox.Controls.Add(this.pipespaceupdown);
            this.mechbox.Controls.Add(this.pipespacelbl);
            this.mechbox.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.mechbox.Location = new System.Drawing.Point(15, 77);
            this.mechbox.Margin = new System.Windows.Forms.Padding(2);
            this.mechbox.Name = "mechbox";
            this.mechbox.Padding = new System.Windows.Forms.Padding(2);
            this.mechbox.Size = new System.Drawing.Size(408, 51);
            this.mechbox.TabIndex = 0;
            this.mechbox.TabStop = false;
            this.mechbox.Text = "Mechanical";
            // 
            // pipespaceupdown
            // 
            this.pipespaceupdown.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.pipespaceupdown.Location = new System.Drawing.Point(236, 22);
            this.pipespaceupdown.Margin = new System.Windows.Forms.Padding(2);
            this.pipespaceupdown.Name = "pipespaceupdown";
            this.pipespaceupdown.Size = new System.Drawing.Size(43, 22);
            this.pipespaceupdown.TabIndex = 1;
            // 
            // pipespacelbl
            // 
            this.pipespacelbl.AutoSize = true;
            this.pipespacelbl.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.pipespacelbl.Location = new System.Drawing.Point(18, 24);
            this.pipespacelbl.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.pipespacelbl.Name = "pipespacelbl";
            this.pipespacelbl.Size = new System.Drawing.Size(216, 16);
            this.pipespacelbl.TabIndex = 0;
            this.pipespacelbl.Text = "Pipe Spacing on 1/8\" Plan (inches):";
            // 
            // SettingsForm
            // 
            this.AcceptButton = this.okbutton;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoSize = true;
            this.BackColor = System.Drawing.Color.Gainsboro;
            this.CancelButton = this.cancelbutton;
            this.ClientSize = new System.Drawing.Size(428, 285);
            this.Controls.Add(this.cancelbutton);
            this.Controls.Add(this.okbutton);
            this.Controls.Add(this.frame);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.Margin = new System.Windows.Forms.Padding(2);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "SettingsForm";
            this.ShowIcon = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Settings";
            this.Load += new System.EventHandler(this.SettingsForm_Load);
            this.wkstbox.ResumeLayout(false);
            this.wkstbox.PerformLayout();
            this.frame.ResumeLayout(false);
            this.txtsettingsbox.ResumeLayout(false);
            this.txtsettingsbox.PerformLayout();
            this.mechbox.ResumeLayout(false);
            this.mechbox.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pipespaceupdown)).EndInit();
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
        private System.Windows.Forms.Label txtstylelbl;
        public System.Windows.Forms.ComboBox defaulttext;
    }
}