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
            picol.OfClass(typeof(PanelScheduleSheetInstance));
            FilteredElementCollector pscol = new FilteredElementCollector(doc);
            pscol.OfClass(typeof(PanelScheduleView));
            bool[] isplaced = new bool[pscol.Count()];
            string[] placedsheet = new string[pscol.Count()];
            int cntr = 0;
            string unplres = "";

            foreach(PanelScheduleView ps in pscol)
            {
                isplaced[cntr] = false;
                foreach(PanelScheduleSheetInstance pi in picol)
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
            mb.richTextBox1.Text = psres + "\n" + unplres;
            mb.Text = "Panel Schedules";
            mb.Show();

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
