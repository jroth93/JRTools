﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Text.RegularExpressions;
using org.mariuszgromada.math.mxparser;

namespace Proficient
{
    public partial class DuctMain : Form
    {
        public DuctMain()
        {
            InitializeComponent();
            this.TopMost = Properties.Settings.Default.appontop;
            this.ActiveControl = Airflowtxt1;
            if(Properties.Settings.Default.appvertical) {VerticalApplication();}
        }

        private void Tab1Control(object sender, EventArgs e)
        {
            TotalCFM.Text = "";
            Output1.Text = "";
            Output2.Text = "";
            Output10.Text = "";           

            List<string> inputs = new List<string>() { Airflowtxt1.Text, Frictiontxt1.Text, Depthmintxt1.Text, Depthmaxtxt1.Text };

            if (Parser(inputs) == false)
            {
                TotalCFM.Text = "Not Valid Input";
                return;
            }
            double airflow = new Expression(inputs[0]).calculate();
            double friction = new Expression(inputs[1]).calculate();
            int mindepth = Convert.ToInt32(new Expression(inputs[2]).calculate());
            int maxdepth = Convert.ToInt32(new Expression(inputs[3]).calculate());

            Match result = Regex.Match(inputs[0], Constants.numpattern);
            TotalCFM.Text = result.Success ? "" : $"{Convert.ToInt32(airflow)} CFM Total";           
           
            List<string> outputs = Backend.AirflowFriction(airflow, friction, mindepth, maxdepth);

            Output1.Text = outputs[0];
            Output2.Text = outputs[1];
            Output10.Text = outputs[2];
            WindowSize(Output1.Height);
        }

        private void Tab2Control(object sender, EventArgs e)
        {
            TotalCFM2.Text = "";
            Output3.Text = "";
            Output4.Text = "";
            Output11.Text = "";

            List<string> inputs = new List<string>() {Airflowtxt2.Text, Veltxt1.Text, Depthmintxt2.Text, Depthmaxtxt2.Text};

            if (Parser(inputs) == false)
            {
                TotalCFM2.Text = "Not Valid Input";
                return;
            }

            double airflow = new Expression(inputs[0]).calculate();
            int velocity = Convert.ToInt32(new Expression(inputs[1]).calculate());
            int mindepth = Convert.ToInt32(new Expression(inputs[2]).calculate());
            int maxdepth = Convert.ToInt32(new Expression(inputs[3]).calculate());

            Match result = Regex.Match(inputs[0], Constants.numpattern);
            TotalCFM2.Text = result.Success ? "" : $"{Convert.ToInt32(airflow)} CFM Total";

            List<string> outputs = Backend.AirflowVelocity(airflow, velocity, mindepth, maxdepth);

            Output3.Text = outputs[0];
            Output4.Text = outputs[1];
            Output11.Text = outputs[2];

            WindowSize(Output3.Height);
        }

        private void Tab3Control(object sender, EventArgs e)
        {
            
            Boolean boolrnd = radiornd1.Checked;
            Depthlbl1.Visible = !boolrnd;
            Depthtxt1.Visible = !boolrnd;
            Widthlbl1.Visible = !boolrnd;
            Widthtxt1.Visible = !boolrnd;
            label3.Visible = !boolrnd;
            Depthlbl3.Visible = !boolrnd;
            label5.Visible = boolrnd;
            Diatxt1.Visible = boolrnd;

            if (boolrnd & Properties.Settings.Default.appvertical)
            {
                Output5.Location = new Point(40, 130);
            }
            else if (!boolrnd & Properties.Settings.Default.appvertical)
            {
                Output5.Location = new Point(40, 170);
            }

            TotalCFM3.Text = "";
            Output5.Text = "";

            List<string> inputs = boolrnd ? new List<string>() { Airflowtxt3.Text, Diatxt1.Text} : new List<string>() { Airflowtxt3.Text, Widthtxt1.Text, Depthtxt1.Text };

            if (Parser(inputs) == false)
            {
                Output5.Text = "Not Valid Input";
                return;
            }

            double airflow = new Expression(inputs[0]).calculate();
            int dia = boolrnd ? Convert.ToInt32(new Expression(inputs[1]).calculate()) : 0;
            int width = boolrnd ? 0 : Convert.ToInt32(new Expression(inputs[1]).calculate());
            int depth = boolrnd ? 0 : Convert.ToInt32(new Expression(inputs[2]).calculate());

            Match result = Regex.Match(inputs[0], Constants.numpattern);
            TotalCFM3.Text = result.Success ? "" : $"{Convert.ToInt32(airflow)} CFM Total";

            int velocity = Convert.ToInt32(Functions.Velocitysolver(airflow, dia, width, depth, boolrnd));
            double friction = Math.Ceiling(Functions.Frictionsolver(airflow, dia, width, depth, boolrnd) * Constants.fprecision) / Constants.fprecision;

            Output5.Text = $"{friction} In./100 ft.\n\n{velocity} FPM";            
        }

        private void Tab4Control(object sender, EventArgs e)
        {
            Boolean boolrnd = radiornd2.Checked;
            depthlbl4.Visible = !boolrnd;
            widthlbl2.Visible = !boolrnd;
            depthtxt2.Visible = !boolrnd;
            widthtxt2.Visible = !boolrnd;
            dialbl2.Visible = boolrnd;
            diatxt2.Visible = boolrnd;
            label6.Visible = !boolrnd;
            label7.Visible = !boolrnd;

            if(boolrnd & Properties.Settings.Default.appvertical)
            {
                Output6.Location = new Point(40, 130);
            }
            else if (!boolrnd & Properties.Settings.Default.appvertical)
            {
                Output6.Location = new Point(40, 170);
            }

            Output6.Text = "";

            List<string> inputs = boolrnd ? new List<string>() { frictiontxt2.Text, diatxt2.Text } : new List<string>() { frictiontxt2.Text, widthtxt2.Text, depthtxt2.Text };

            if (Parser(inputs) == false)
            {
                Output6.Text = "Not Valid Input";
                return;
            }

            double friction = new Expression(inputs[0]).calculate();
            int dia = boolrnd ? Convert.ToInt32(new Expression(inputs[1]).calculate()) : 0;
            int width = boolrnd ? 0 : Convert.ToInt32(new Expression(inputs[1]).calculate());
            int depth = boolrnd ? 0 : Convert.ToInt32(new Expression(inputs[2]).calculate());

            int airflow = Convert.ToInt32(Functions.Airflowsolver(friction, dia, width, depth, boolrnd));
            int velocity = Convert.ToInt32(Functions.Velocitysolver(airflow, dia, width, depth, boolrnd));

            Output6.Text = $"{airflow} CFM \n\n{velocity} FPM";
        }

        private void Tab5Control(object sender, EventArgs e)
        {
            Boolean boolrnd = Radiornd3.Checked;
            widthtxt3.Visible = !boolrnd;
            widthlbl3.Visible = !boolrnd;
            depthlbl5.Visible = !boolrnd;
            depthtxt3.Visible = !boolrnd;
            label11.Visible = !boolrnd;
            dialbl3.Visible = boolrnd;
            diatxt3.Visible = boolrnd;

            if (boolrnd & Properties.Settings.Default.appvertical)
            {
                Output8.Location = new Point(40, 130);
            }
            else if (!boolrnd & Properties.Settings.Default.appvertical)
            {
                Output8.Location = new Point(40, 170);
            }

            Output8.Text = "";

            List<string> inputs = boolrnd ? new List<string>() { velocitytxt2.Text, diatxt3.Text } : new List<string>() { velocitytxt2.Text, widthtxt3.Text, depthtxt3.Text };

            if (Parser(inputs) == false)
            {
                Output8.Text = "Not Valid Input";
                return;
            }

            double velocity = new Expression(inputs[0]).calculate();
            int dia = boolrnd ? Convert.ToInt32(new Expression(inputs[1]).calculate()) : 0;
            int width = boolrnd ? 0 : Convert.ToInt32(new Expression(inputs[1]).calculate());
            int depth = boolrnd ? 0 : Convert.ToInt32(new Expression(inputs[2]).calculate());

            int airflow = Convert.ToInt32(boolrnd ? velocity * Math.PI * Math.Pow(dia, 2) / 576 : velocity * width * depth/144.0);
            double friction = Convert.ToInt32(Functions.Frictionsolver(airflow, dia, width, depth, boolrnd)* Constants.fprecision) / Constants.fprecision;

            Output8.Text = $"{airflow} CFM\n\n{friction} In./100 ft.";
        }

        private void Tab6Control(object sender, EventArgs e)
        {
            Boolean boolrnd = radiornd4.Checked;
            dialbl4.Visible = boolrnd;
            diatxt4.Visible = boolrnd;
            widthlbl4.Visible = !boolrnd;
            widthtxt4.Visible = !boolrnd;
            depthtxt4.Visible = !boolrnd;
            depthlbl6.Visible = !boolrnd;
            label18.Top = boolrnd? 89 : 127;
            depthmintxt3.Top = boolrnd ? 92 : 130;
            depthmaxtxt3.Top = boolrnd ? 92 : 130;
            label15.Top = boolrnd ? 95 : 133;
            label13.Top = boolrnd ? 95 : 133;

            if (boolrnd & Properties.Settings.Default.appvertical)
            {
                Output9.Location = new Point(40, 130);
            }
            else if (!boolrnd & Properties.Settings.Default.appvertical)
            {
                Output9.Location = new Point(40, 170);
            }

            Output9.Text = "";

            List<string> inputs = boolrnd ? new List<string>() {diatxt4.Text, "1", depthmintxt3.Text, depthmaxtxt3.Text } : new List<string>() { widthtxt4.Text, depthtxt4.Text, depthmintxt3.Text, depthmaxtxt3.Text };

            if (Parser(inputs) == false)
            {
                Output9.Text = "Not Valid Input";
                return;
            }

            int dia = boolrnd ? Convert.ToInt32(new Expression(inputs[0]).calculate()) : 0;
            int width = boolrnd ? 0 : Convert.ToInt32(new Expression(inputs[0]).calculate());
            int depth = boolrnd ? 0 : Convert.ToInt32(new Expression(inputs[1]).calculate());
            int mindepth = Convert.ToInt32(new Expression(inputs[2]).calculate());
            int maxdepth = Convert.ToInt32(new Expression(inputs[3]).calculate());

            string output = Backend.EquivalentDuct(dia, width, depth, mindepth, maxdepth, boolrnd);

            Output9.Text = output;

            WindowSize(Output9.Height);
        }

        public bool Parser(List<string> inputs)
        {
            
            foreach (string input in inputs)
            {
                Match result = Regex.Match(input, Constants.pattern);
                if (result.Success == false)
                {
                    return false;
                }
                if (new Expression(input).calculate() <= 0 || new Expression(input).calculate() > 2147483647)
                {
                    return false;
                }
            }

            return inputs.Count != 0;
        }

        private void WindowSize(int size)
        {
            if(Properties.Settings.Default.appvertical)
            {
                this.Height = size > 170 ? size + 300 : 450;
            }
            else
            {
                this.Height = size > 150 ? size + 130 : 275;
            }
                
        }

        private void SettingsButtonClick(object sender, EventArgs e)
        {
            UserSettings usr = new UserSettings();
            usr.Show();
        }

        public void VerticalApplication()
        {
            //form size
            this.Width = 330;
            this.Height = 450;
            //tab1
            pictureBox1.Location = new Point(255,5);
            Output1.Location = new Point(40, 165);
            Output2.Location = new Point(110, 165);
            Output10.Location = new Point(170, 165);
            //tab2
            Output3.Location = new Point(40, 165);
            Output4.Location = new Point(105, 165);
            Output11.Location = new Point(175, 165);
            //tab3
            Output5.Location = new Point(40, 170);
            //tab4
            Output6.Location = new Point(40, 170);
            //tab5
            Output8.Location = new Point(40, 170);
            //tab6
            Output9.Location = new Point(40, 170);
;        }

        // Drag from any point on form
        public const int WM_NCLBUTTONDOWN = 0xA1;
        public const int HT_CAPTION = 0x2;
        [System.Runtime.InteropServices.DllImport("user32.dll")]
        public static extern int SendMessage(IntPtr hWnd, int Msg, int wParam, int lParam);
        [System.Runtime.InteropServices.DllImport("user32.dll")]
        public static extern bool ReleaseCapture();
        private void Form1_MouseDown(object sender, System.Windows.Forms.MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                ReleaseCapture();
                SendMessage(Handle, WM_NCLBUTTONDOWN, HT_CAPTION, 0);
            }
        }
    }
}