using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Autodesk.Revit.UI;
using Autodesk.Revit.DB;
using Autodesk.Revit.UI.Selection;
using Autodesk.Revit.DB.Electrical;

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
            View view = doc.GetElement(uidoc.ActiveView.Id) as View;
            FilteredElementCollector picol = new FilteredElementCollector(doc);
            picol.OfClass(typeof(PanelScheduleSheetInstance));
            FilteredElementCollector pscol = new FilteredElementCollector(doc);
            pscol.OfClass(typeof(PanelScheduleView));
            bool[] isplaced = new bool[pscol.Count()];
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
                        break;
                    }
                }

                if(!isplaced[cntr])
                {
                    unplres += "Panel schedule for " + ps.Name + " is not placed.\n";
                }
                cntr++;
            }

            TaskDialog td = new TaskDialog("Unplaced Panel Schedules");
            td.MainContent = unplres;
            td.Show();

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
