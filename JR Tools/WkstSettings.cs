using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace JR_Tools
{
    public partial class WorksetSettingsForm : Form
    {
        public WorksetSettingsForm()
        {
            InitializeComponent();
            this.StartPosition = FormStartPosition.CenterScreen;
        }

        public Boolean iscancelled = false;

        private void WorksetSettingsForm_Load(object sender, EventArgs e)
        {
 
        }

        private void okbutton_Click(object sender, EventArgs e)
        {
            this.Hide();
        }

        private void cancelbutton_Click(object sender, EventArgs e)
        {
            iscancelled = true;
            this.Hide();
        }
    }
}
