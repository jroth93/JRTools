using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Autodesk.Revit.UI;
using Autodesk.Revit.DB;
using Autodesk.Revit.ApplicationServices;
using Autodesk.Revit.UI.Selection;
using Autodesk.Revit.Creation;

namespace JR_Tools
{
    [Autodesk.Revit.Attributes.Transaction(Autodesk.Revit.Attributes.TransactionMode.Manual)]
    public class ElementPlacer : IExternalCommand
    {
        public Autodesk.Revit.UI.Result Execute(ExternalCommandData revit, ref string message, ElementSet elements)
        {
            UIApplication app = revit.Application;
            UIDocument uidoc = revit.Application.ActiveUIDocument;
            Autodesk.Revit.DB.Document doc = revit.Application.ActiveUIDocument.Document;
            ElementId viewid = uidoc.ActiveView.Id;
            View view = doc.GetElement(viewid) as View;
            Reference cref = uidoc.Selection.PickObject(ObjectType.Element, "Pick Path Detail Line");
            Reference elref = uidoc.Selection.PickObject(ObjectType.Element, "Pick element to be placed");

            Curve pathcurve = (doc.GetElement(cref).Location as LocationCurve).Curve;
            FamilySymbol elfs = (doc.GetElement(elref.ElementId) as FamilyInstance).Symbol;
            double numel = 8;
            double spacing = 70;
            
            using (Transaction tx = new Transaction(doc, "Place Elements"))
            {
                if (tx.Start() == TransactionStatus.Started)
                {
                    /*
                    for (int i = 0; i < numel; i++)
                    {
                        XYZ newloc = pathcurve.Evaluate(i / (numel - 1), true);
                        doc.Create.NewFamilyInstance(newloc, elfs, view.GenLevel, Autodesk.Revit.DB.Structure.StructuralType.NonStructural);
                    }
                    */
                    double curlen = 10;
                    while(curlen < pathcurve.Length)
                    {
                        XYZ newloc = pathcurve.Evaluate(curlen / pathcurve.Length, true);
                        Line rotax = Line.CreateBound(newloc, newloc.Add(XYZ.BasisZ));
                        XYZ newdir = pathcurve.ComputeDerivatives(curlen / pathcurve.Length, true).get_Basis(0);
                        double rotangle = XYZ.BasisX.AngleTo(newdir);

                        FamilyInstance newel = doc.Create.NewFamilyInstance(newloc, elfs, view.GenLevel, Autodesk.Revit.DB.Structure.StructuralType.NonStructural);
                        ElementTransformUtils.RotateElement(doc, newel.Id, rotax, rotangle);
                        curlen += spacing;
                    }
                    
                    
                }

                tx.Commit();
            }

            return Result.Succeeded;
        }
    }
}
