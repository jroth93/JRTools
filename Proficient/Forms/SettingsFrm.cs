using System;
using System.Windows.Forms;

namespace Proficient
{
    public partial class SettingsForm : Form
    {
        public SettingsForm()
        {
            InitializeComponent();
            this.StartPosition = FormStartPosition.CenterScreen;
        }

        public bool iscancelled = false;

        private void SettingsForm_Load(object sender, EventArgs e)
        {

        }

        private void okbutton_Click(object sender, EventArgs e)
        {
            iscancelled = false;
            this.Hide();
        }

        private void cancelbutton_Click(object sender, EventArgs e)
        {
            iscancelled = true;
            this.Hide();
        }

    }
}
