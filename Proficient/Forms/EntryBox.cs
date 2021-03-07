using System;
using System.Windows.Forms;

namespace Proficient
{
    public partial class EntryBox: Form
    {
        public string Entry { get; private set; }
        private Type expectedType;
        private string errorMessage;
        private string cap;
        public EntryBox(string title, string caption, Type expType, string errorMsg)
        {
            InitializeComponent();
            expectedType = expType;
            errorMessage = errorMsg;
            cap = caption;
            Text = title;
            label1.Text = cap;
        }

        public EntryBox(string title, string caption, Type expType)
        {
            InitializeComponent();
            expectedType = expType;
            errorMessage = "Invalid Format. Please Try Again.";
            cap = caption;
            Text = title;
            label1.Text = cap;
        }

        public EntryBox(string title, string caption)
        {
            InitializeComponent();
            expectedType = typeof(string);
            errorMessage = "Invalid Format. Please Try Again.";
            cap = caption;
            Text = title;
            label1.Text = cap;
        }


        private void okbutton_Click(object sender, EventArgs e)
        {
            
            Entry = textBox1.Text;

            if(ValidateEntry())
            {
                DialogResult = DialogResult.OK;
                Close();
            }
            else
            {
                DialogResult = DialogResult.None;
                label1.Text = errorMessage + "\n\n" + cap;
            }
            
        }

        private void cancelbutton_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.Cancel;
            Close();
        }

        private bool ValidateEntry()
        {
            if(expectedType == typeof(double))
            {
                return Double.TryParse(this.Entry, out double x);
            }
            else if (expectedType == typeof(int))
            {
                return Int32.TryParse(this.Entry, out int y);
            }
            else
            {
                return true;
            }            
        }
    }
}
