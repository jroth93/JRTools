using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Autodesk.Revit.UI;
using Autodesk.Revit.DB;
using Autodesk.Revit.UI.Selection;
using Autodesk.Revit.DB.Mechanical;

namespace Proficient
{
    [Autodesk.Revit.Attributes.Transaction(Autodesk.Revit.Attributes.TransactionMode.Manual)]
    class DamperToggle : IExternalCommand
    {
        public Result Execute(ExternalCommandData revit, ref string message, ElementSet elements)
        {
            UIApplication app = revit.Application; 
            UIDocument uidoc = revit.Application.ActiveUIDocument;
            Document doc = uidoc.Document;
            View view = doc.GetElement(uidoc.ActiveView.Id) as View;

            FilteredElementCollector ductTypeFec = new FilteredElementCollector(doc).OfClass(typeof(DuctType));

            foreach(DuctType dt in ductTypeFec)
            {
                RoutingPreferenceManager rpm = dt.RoutingPreferenceManager;
                for(int i = 1; i < rpm.GetNumberOfRules(RoutingPreferenceRuleGroupType.Junctions); i++)
                {
                    RoutingPreferenceRule rpr = rpm.GetRule(RoutingPreferenceRuleGroupType.Junctions, i - 1);
                    rpr.Equals(null);
                }
            }



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
