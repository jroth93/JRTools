using System;
using System.Collections.Generic;

namespace Proficient
{
    public static class Backend
    {
        public static List<string> AirflowFriction(double airflow, double friction, int mindepth, int maxdepth)
        {
            string[] outputinitialize = { "Duct Size\n[in]\n\n", "Velocity\n[FPM]\n\n", "Actual Friction\n[In./100 ft.]\n\n" };
            List<string> output = new List<string>(outputinitialize);
            double width, vel, frict;
            // determine rectangular sizes
            for (int depthcounter = mindepth; depthcounter < maxdepth + 2; depthcounter += 2)
            {
                depthcounter += depthcounter == 0 ? 2 : 0;
                width = Functions.Widthsolver(friction, airflow, depthcounter);
                // print equivalent diameter on first iteration
                if (depthcounter == mindepth)
                {
                    double dia = Convert.ToInt32(Math.Ceiling(1.3 * Math.Pow(width * depthcounter, 0.625) / Math.Pow(width + depthcounter, 0.25)));
                    dia = dia % 2 == 1 ? dia += 1 : dia;
                    dia = Functions.Frictionsolver(airflow, dia - 2, 0, 0, true) < friction ? dia - 2 : dia;
                    vel = Math.Ceiling(Functions.Velocitysolver(airflow, dia, 0, 0, true));
                    frict = Math.Ceiling(Functions.Frictionsolver(airflow, dia, 0, 0, true) * Constants.fprecision) / Constants.fprecision;
                    output[0] += $"{dia} Ø\n";
                    output[1] += $"{vel}\n";
                    output[2] += $"{frict}\n";
                }
                //output duct dimensions
                vel = Convert.ToInt32(Functions.Velocitysolver(airflow, 0, width, depthcounter, false));
                frict = Math.Ceiling(Functions.Frictionsolver(airflow, 0, width, depthcounter, false) * Constants.fprecision) / Constants.fprecision;
                output[0] += $"\n{width} / {depthcounter}";
                output[1] += $"\n{vel}";
                output[2] += $"\n{frict}";
            }

            return output;
        }

        public static List<string> AirflowVelocity(double airflow, int velocity, int mindepth, int maxdepth)
        {
            string[] outputinitialize = { "Duct Size\n[in]\n\n", "Friction\n[In./100 ft.]\n\n", "Actual Velocity\n[FPM]\n\n" };
            List<string> output = new List<string>(outputinitialize);

            for (int depthcounter = mindepth; depthcounter < maxdepth + 2; depthcounter += 2)
            {
                int actualvel;
                int width = Convert.ToInt32(Math.Ceiling(144 * airflow / (velocity * depthcounter)));
                width = width % 2 == 1 ? width + 1 : width;
                double frictionrect = Math.Ceiling(Functions.Frictionsolver(airflow, 0, width, depthcounter, false) * Constants.fprecision) / Constants.fprecision;
                if (depthcounter == mindepth)
                {
                    double dia = Math.Ceiling(Math.Pow(576.0 * airflow / (Math.PI * velocity), 0.5));
                    dia = dia % 2 == 1 ? dia += 1 : dia;
                    double frictionrnd = Math.Ceiling(Functions.Frictionsolver(airflow, dia, 0, 0, true) * Constants.fprecision) / Constants.fprecision;
                    actualvel = Convert.ToInt32(Functions.Velocitysolver(airflow, dia, 0, 0, true));
                    output[0] += $"{dia} Ø\n";
                    output[1] += $"{frictionrnd}\n";
                    output[2] += $"{actualvel}\n";
                }
                actualvel = Convert.ToInt32(Functions.Velocitysolver(airflow, 0, width, depthcounter, false));
                output[0] += $"\n{width} / {depthcounter}";
                output[1] += $"\n{frictionrect}";
                output[2] += $"\n{actualvel}";
            }

            return output;
        }

        public static string EquivalentDuct(int dia, int width, int depth, int mindepth, int maxdepth, bool boolrnd)
        {

            string output = "Duct Size\n[in]\n\n";
            int airflow = 1000;
            double friction = Functions.Frictionsolver(airflow, dia, width, depth, boolrnd);

            if (!boolrnd)
            {
                double diaout = Math.Ceiling(1.3 * Math.Pow(width * depth, 0.625) / Math.Pow(width + depth, 0.25));
                diaout = diaout % 2 == 1 ? diaout + 1 : diaout;
                output += $"{diaout} Ø\n\n";
            }

            for (int depthcounter = mindepth; depthcounter < maxdepth + 2; depthcounter += 2)
            {
                int widthout = Convert.ToInt32(Functions.Widthsolver(friction, airflow, depthcounter));
                output += depthcounter == depth && boolrnd ? "" : $"{widthout}/{depthcounter}\n";
            }

            return output;
        }
    }
}
