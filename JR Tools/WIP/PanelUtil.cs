using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Autodesk.Revit.UI;
using Autodesk.Revit.DB;
using Autodesk.Revit.UI.Selection;
using Autodesk.Revit.DB.Electrical;
using System.Windows.Forms;
using Microsoft.Office.Interop.Excel;

namespace JR_Tools
{
    [Autodesk.Revit.Attributes.Transaction(Autodesk.Revit.Attributes.TransactionMode.Manual)]
    class PanelUtil : IExternalCommand
    {
        public Result Execute(ExternalCommandData revit, ref string message, ElementSet elements)
        {
            UIApplication app = revit.Application; 
            UIDocument uidoc = revit.Application.ActiveUIDocument;
            Document doc = uidoc.Document;
            Autodesk.Revit.DB.View view = doc.GetElement(uidoc.ActiveView.Id) as Autodesk.Revit.DB.View;
            FilteredElementCollector picol = new FilteredElementCollector(doc);
            var picolsort = picol.OfClass(typeof(PanelScheduleSheetInstance)).OrderBy(x => x.Name);
            FilteredElementCollector pscol = new FilteredElementCollector(doc);
            var pscolsort = pscol.OfClass(typeof(PanelScheduleView)).OrderBy(x => x.Name);
            bool[] isplaced = new bool[pscol.Count()];
            string[] placedsheet = new string[pscol.Count()];
            int cntr = 0;
            string unplres = "";

            foreach(PanelScheduleView ps in pscolsort)
            {
                isplaced[cntr] = false;
                foreach(PanelScheduleSheetInstance pi in picolsort)
                {
                    if (pi.ScheduleId == ps.Id)
                    {
                        isplaced[cntr] = true;
                        placedsheet[cntr] = placedsheet[cntr] == null ? ps.Name + "\t" + doc.GetElement(pi.OwnerViewId).get_Parameter(BuiltInParameter.SHEET_NUMBER).AsString() : placedsheet[cntr] + ", " + doc.GetElement(pi.OwnerViewId).get_Parameter(BuiltInParameter.SHEET_NUMBER).AsString();
                    }
                }

                if(!isplaced[cntr])
                {
                    unplres += ps.Name + "\tNot placed\n";
                }
                cntr++;
            }

            string psres = "Panel\tSchedule Location\n\n";
            foreach(string ps in placedsheet)
            {
                if(ps!=null)
                {
                    psres += ps + "\n";
                }
            }

            MsgBox mb = new MsgBox();
            bool resize = true;

            mb.richTextBox1.ContentsResized += (object sender, ContentsResizedEventArgs e) =>
            {
                if (resize)
                {
                    mb.Height = e.NewRectangle.Height + 50;
                }
            };

            mb.richTextBox1.Text = psres + "\n" + unplres;

            
            mb.Text = "Panel Schedules";
            mb.MinimumSize = new System.Drawing.Size(300,100);
            mb.Show();
            resize = false;


            using (Transaction tx = new Transaction(doc, "commandname"))
            {
                if (tx.Start() == TransactionStatus.Started)
                {
                }

                tx.Commit();
            }

            return Result.Succeeded;
        }
    }
}
