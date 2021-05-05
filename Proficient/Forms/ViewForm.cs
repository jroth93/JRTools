using System;
using System.Windows.Forms;

namespace Proficient
{
    public partial class ViewForm : Form
    {
        public int selectedViewIndex { get; private set; }
        public ViewForm(string[] calloutViews)
        {
            InitializeComponent();
            this.viewdropdown.Items.AddRange(calloutViews);
            this.StartPosition = FormStartPosition.CenterScreen;
            selectedViewIndex = 0;
        }

        private void Okbutton_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.OK;
            this.Close();
        }
        private void Cancelbutton_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
            this.Close();
        }

        private void viewdropdown_SelectedIndexChanged(object sender, EventArgs e)
        {
            selectedViewIndex = viewdropdown.SelectedIndex;
        }
    }
}
