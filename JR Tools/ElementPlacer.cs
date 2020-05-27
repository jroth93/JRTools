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
            Reference levref = new Reference(view.GenLevel);

            Curve pathcurve = (doc.GetElement(cref).Location as LocationCurve).Curve;
            FamilyInstance elfi = doc.GetElement(elref.ElementId) as FamilyInstance;
            FamilySymbol elfs = elfi.Symbol;
            XYZ newloc = pathcurve.Evaluate(0.2, true);
            XYZ refdir = new XYZ(0, 0, 1);
            
            using (Transaction tx = new Transaction(doc, "Place Elements"))
            {
                if (tx.Start() == TransactionStatus.Started)
                {
                    doc.Create.NewFamilyInstance(newloc,elfs,view.GenLevel,Autodesk.Revit.DB.Structure.StructuralType.NonStructural);


                    
                }

                tx.Commit();
            }

            return Result.Succeeded;
        }
    }
}
