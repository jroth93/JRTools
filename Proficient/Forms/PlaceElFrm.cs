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
    public partial class PlaceElFrm : Form
    {
        public PlaceElFrm()
        {
            InitializeComponent();
            startoffsetlbl.Visible = false;
            startoffset.Visible = false;
            txtlabel.Text = "Number of Elements:";
        }

        private void radionumber_CheckedChanged(object sender, EventArgs e)
        {
            if(radionumber.Checked)
            {
                txtlabel.Text = "Number of Elements:";
                startoffsetlbl.Visible = false;
                startoffset.Visible = false;
            }
        }

        private void radiooffset_CheckedChanged(object sender, EventArgs e)
        {
            if(radiooffset.Checked)
            {
                txtlabel.Text = "Distance between Elements (ft):";
                startoffset.Visible = true;
                startoffsetlbl.Visible = true;
            }
        }

        private void okbutton_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.OK;
            this.Hide();
        }

        private void PlaceElFrm_Load(object sender, EventArgs e)
        {

        }
    }
}
