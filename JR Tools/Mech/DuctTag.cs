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
                        bool ishorizontal = Math.Round(loccurve.Curve.GetEndPoint(0).Y, 4) == Math.Round(loccurve.Curve.GetEndPoint(1).Y, 4);


                        if (boolductlongenough && !IsTagged(doc, viewid, ductel))
                        {
                            FilteredElementCollector fec = new FilteredElementCollector(doc);
                            XYZ point = loccurve.Curve.Evaluate(0.5, true) as XYZ;
                            bool ldr = false;
                            var tor = TagOrientation.Horizontal;
                            Family tagfam = fec.OfClass(typeof(Family)).Where(f => f.Name == "MEI Mech Tag Duct").FirstOrDefault() as Family;
                            ElementId symid = tagfam.GetFamilySymbolIds().FirstOrDefault();

                            if (isvertical)
                            {
                                if(ductwidth >= minnoleadersize)
                                {
                                    tor = TagOrientation.Vertical;
                                }
                                else
                                {
                                    ldr = true;
                                }
                            }
                            else if(ishorizontal && ductwidth < minnoleadersize)
                            {
                                ldr = true;
                            }
                            else
                            {
                                tagfam = fec.OfClass(typeof(Family)).Where(f => f.Name == "MEI Mech Tag Duct - Rotating").FirstOrDefault() as Family;
                                symid = tagfam.GetFamilySymbolIds().FirstOrDefault();
                                if(ductwidth < minnoleadersize)
                                {
                                    ldr = true;
                                }
                            }

                            IndependentTag tag = IndependentTag.Create(doc, symid, viewid, ductref, ldr, tor, point);
                        }

                    }

                    
                }

                tx.Commit();
            }

            return Result.Succeeded;
        }
        public bool IsTagged(Document doc, ElementId viewid, Element el)
        {
            FilteredElementCollector fec = new FilteredElementCollector(doc, viewid);
            var itfec = fec.OfClass(typeof(IndependentTag));
            foreach(IndependentTag it in itfec)
            {
                if (it.TaggedLocalElementId == el.Id)
                {
                    return true;
                }
            }
            return false;
        }
    }
}
