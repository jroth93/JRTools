using System;
using System.Windows.Forms;

namespace Proficient
{
    public partial class UserSettings : Form
    {
        public UserSettings()
        {
            InitializeComponent();
            TopMost = Proficient.Settings.appOnTop;
            radiovert.Checked = Proficient.Settings.appVert;
            radiohor.Checked = !Proficient.Settings.appVert;

            numericUpDown1.Value = Proficient.Settings.fricPrec;
            maxdepthtxt.Text = Convert.ToString(Proficient.Settings.defDepthMax);
            mindepthtxt.Text = Convert.ToString(Proficient.Settings.defDepthMin);
            Velocitytxt.Text = Convert.ToString(Proficient.Settings.defVelocity);
            Frictiontxt.Text = Convert.ToString(Proficient.Settings.defFriction);
        }

        private void UserSettings_Load(object sender, EventArgs e)
        {

        }

        private void Savebutton_Click(object sender, EventArgs e)
        {
            Proficient.Settings.defFriction = Convert.ToDouble(Frictiontxt.Text);
            Proficient.Settings.defVelocity = Convert.ToInt32(Velocitytxt.Text);
            Proficient.Settings.defDepthMax = Convert.ToInt32(maxdepthtxt.Text);
            Proficient.Settings.defDepthMin = Convert.ToInt32(mindepthtxt.Text);
            Proficient.Settings.fricPrec = Convert.ToInt32(numericUpDown1.Value);
            Proficient.Settings.appOnTop = checkBox1.Checked;
            Proficient.Settings.appVert = radiovert.Checked;
            MessageBox.Show("Please restart program for new settings to take effect.", "Restart Program");
            this.Close();
        }

    }
}
