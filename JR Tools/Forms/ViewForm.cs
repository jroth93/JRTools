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
    public partial class ViewForm : Form
    {
        public ViewForm()
        {
            InitializeComponent();
            this.StartPosition = FormStartPosition.CenterScreen;
            
        }

        public Boolean iscancelled = false;

        private void Okbutton_Click(object sender, EventArgs e)
        {
            this.Hide();
        }
        private void Cancelbutton_Click(object sender, EventArgs e)
        {
            iscancelled = true;
            this.Hide();
        }
    }
}
