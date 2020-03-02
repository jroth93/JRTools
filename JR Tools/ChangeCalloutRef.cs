using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Autodesk.Revit.UI;
using Autodesk.Revit.DB;
using Autodesk.Revit.ApplicationServices;
using Autodesk.Revit.UI.Selection;

namespace JR_Tools
{
    [Autodesk.Revit.Attributes.Transaction(Autodesk.Revit.Attributes.TransactionMode.Manual)]
    class ChangeCalloutRef : IExternalCommand
    {
        public Result Execute(ExternalCommandData revit, ref string message, ElementSet elements)
        {
            UIDocument uidoc = revit.Application.ActiveUIDocument;
            Document doc = revit.Application.ActiveUIDocument.Document;
            ElementId viewid = uidoc.ActiveView.Id;
            View view = doc.GetElement(viewid) as View;

            IEnumerable<ElementId> selectedids = uidoc.Selection.GetElementIds();
            if(selectedids.Count() == 0) { return Result.Cancelled; }
            var selectedelements = selectedids.Select(curid => doc.GetElement(curid));
            IList<ElementId> viewelementsid = new List<ElementId>();

            foreach(Element curelement in selectedelements)
            {
                foreach(ElementId curtype in (curelement.GetValidTypes()))
                {
                    ElementType cureltype = doc.GetElement(curtype) as ElementType;
                    if(cureltype.FamilyName == "Floor Plan")
                    {
                        viewelementsid.Add(curelement.Id);
                    }
                    
                }
            }

            IEnumerable<ElementId> calloutids = viewelementsid
                .Where(curview => doc.GetElement(curview).get_Parameter(BuiltInParameter.SECTION_PARENT_VIEW_NAME) != null)
                .Where(curview => doc.GetElement(curview).OwnerViewId.IntegerValue != -1);

            if(calloutids.Count() == 0) { return Result.Cancelled; }

            IEnumerable<Element> calloutelements = calloutids.Select(curid => doc.GetElement(curid));

            FilteredElementCollector coll = new FilteredElementCollector(doc);
            coll.WherePasses(new ElementClassFilter(typeof(View)));
            IList<View> calloutviews = new List<View>();

            ViewForm form1 = new ViewForm();

            foreach (View v in coll)
            {
                if (Convert.ToString(v.ViewType) == "FloorPlan" && v.get_Parameter(BuiltInParameter.SECTION_PARENT_VIEW_NAME) != null)
                {
                    calloutviews.Add(v);
                    form1.viewdropdown.Items.Add(v.Name.ToString());
                }
            }
            
            form1.ShowDialog();

            if (form1.iscancelled == true) { return Result.Cancelled; }

            View newview = calloutviews[form1.viewdropdown.SelectedIndex];

            using (Transaction tx = new Transaction(doc, "Change Callout Reference"))
            {
                if (tx.Start() == TransactionStatus.Started)
                {
                    foreach(ElementId currentid in calloutids)
                    {
                            ReferenceableViewUtils.ChangeReferencedView(doc, currentid, newview.Id);

                    }
                }

                tx.Commit();
            }
            form1.Close();
            return Result.Succeeded;
        }
    }
}
