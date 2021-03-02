using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Proficient
{
    public partial class UserSettings : Form
    {
        public UserSettings()
        {
            InitializeComponent();
            this.TopMost = Properties.Settings.Default.appontop;
            radiovert.Checked = Properties.Settings.Default.appvertical;
            radiohor.Checked = !Properties.Settings.Default.appvertical;
        }

        private void UserSettings_Load(object sender, EventArgs e)
        {
            
        }

        private void Savebutton_Click(object sender, EventArgs e)
        {
            Properties.Settings.Default.defaultfriction = Frictiontxt.Text;
            Properties.Settings.Default.defaultvelocity = Velocitytxt.Text;
            Properties.Settings.Default.defaultdepthmax = maxdepthtxt.Text;
            Properties.Settings.Default.defaultdepthmin = mindepthtxt.Text;
            Properties.Settings.Default.frictionprecision = Convert.ToInt32(numericUpDown1.Value);
            Properties.Settings.Default.appontop = checkBox1.Checked;
            Properties.Settings.Default.appvertical = radiovert.Checked;
            Properties.Settings.Default.Save();
            MessageBox.Show("Please restart program for new settings to take effect.", "Restart Program");
            this.Close();
        }

    }
}
