using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Proficient
{
    public class Settings
    {
        public Settings() { }
        public bool switchEnlarged { get; set; }
        public string defWorkset { get; set; }
        public int pipeDist { get; set; }
        public string defFont { get; set; }

        public double defFriction { get; set; }
        public int defVelocity { get; set; }
        public int defDepthMin { get; set; }
        public int defDepthMax { get; set; }
        public int fricPrec { get; set; }
        public bool appOnTop { get; set; }
        public bool appVert { get; set; }

        public void InitializeDefaults()
        {
            switchEnlarged = true;
            defWorkset = "M-Mechanical";
            pipeDist = 9;
            defFont = "3/32\" Arial";

            defFriction = 0.08;
            defVelocity = 500;
            defDepthMin = 6;
            defDepthMax = 20;
            fricPrec = 3;
            appOnTop = false;
            appVert = false;
        }


    }
}
