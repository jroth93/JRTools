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
    public class DuctTag : IExternalCommand
    {
        public Autodesk.Revit.UI.Result Execute(ExternalCommandData revit, ref string message, ElementSet elements)
        {

            UIDocument uidoc = revit.Application.ActiveUIDocument;
            Document doc = revit.Application.ActiveUIDocument.Document;
            ElementId viewid = uidoc.ActiveView.Id;
            View view = doc.GetElement(viewid) as View;


            FilteredElementCollector collector = new FilteredElementCollector(doc,viewid);
            collector.OfCategory(BuiltInCategory.OST_DuctCurves);
            ICollection<Element> allducts = collector.ToElements();


            using (Transaction tx = new Transaction(doc, "Add leader"))
            {
                if (tx.Start() == TransactionStatus.Started)
                {
                    foreach(Element ductel in allducts)
                    {
                        Reference ductref = new Reference(ductel);
                        Location loc = ductel.Location;
                        LocationCurve loccurve = loc as LocationCurve;
                        bool boolductlongenough = loccurve.Curve.Length > 3 ? true : false;
                        double ductwidth = ductel.get_Parameter(BuiltInParameter.RBS_CURVE_WIDTH_PARAM).AsDouble();
                        double minnoleadersize = 144 / Convert.ToDouble(view.Scale);
                        bool isvertical = Math.Round(loccurve.Curve.GetEndPoint(0).X,4) == Math.Round(loccurve.Curve.GetEndPoint(1).X,4);


                        if (boolductlongenough)
                        {
                            //vertical large duct
                            if (isvertical & ductwidth >= minnoleadersize)
                            {
                                XYZ point = loccurve.Curve.Evaluate(0.5, true) as XYZ;
                                IndependentTag tag = IndependentTag.Create(doc, viewid, ductref, false, TagMode.TM_ADDBY_CATEGORY, TagOrientation.Vertical, point);
                            }
                            //horizontal/angled large duct
                            else if (!isvertical & ductwidth >= minnoleadersize)
                            {
                                XYZ point = loccurve.Curve.Evaluate(0.5, true) as XYZ;
                                IndependentTag tag = IndependentTag.Create(doc, viewid, ductref, false, TagMode.TM_ADDBY_CATEGORY, TagOrientation.Horizontal, point);
                            }
                            //horizontal/angled small duct
                            else if (!isvertical & ductwidth < minnoleadersize)
                            {
                                XYZ point = loccurve.Curve.Evaluate(0.5, true) as XYZ;
                                IndependentTag tag = IndependentTag.Create(doc, viewid, ductref, true, TagMode.TM_ADDBY_CATEGORY, TagOrientation.Horizontal, point);
                            }
                            //vertical small duct
                            else if (isvertical & ductwidth < minnoleadersize)
                            {
                                XYZ point = loccurve.Curve.Evaluate(0.5, true) as XYZ;
                                IndependentTag.Create(doc, viewid, ductref, true, TagMode.TM_ADDBY_CATEGORY, TagOrientation.Horizontal, point);
                            }
                        }

                    }

                    
                }

                tx.Commit();
            }

            return Result.Succeeded;
        }
    }
}
