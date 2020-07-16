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
    public class PipeSpacer : IExternalCommand
    {
        public Result Execute(ExternalCommandData revit, ref string message, ElementSet elements)
        {
            UIDocument uidoc = revit.Application.ActiveUIDocument;
            Document doc = revit.Application.ActiveUIDocument.Document;
            ElementId viewid = uidoc.ActiveView.Id;
            View view = doc.GetElement(viewid) as View;

            while (true)
            {
                Reference ref1 = null, ref2 = null;
                try
                {
                    ref1 = uidoc.Selection.PickObject(ObjectType.Element, "Pick Anchor Pipe");
                    ref2 = uidoc.Selection.PickObject(ObjectType.Element, "Pick Pipe To Be Moved");
                }
                catch (Autodesk.Revit.Exceptions.OperationCanceledException)
                {
                    return Result.Succeeded;
                }

                try
                {
                    var pipe1 = doc.GetElement(ref1);
                    var pipe2 = doc.GetElement(ref2);
                    LocationCurve loc1 = pipe1.Location as LocationCurve;
                    LocationCurve loc2 = pipe2.Location as LocationCurve;

                    bool ishor = Math.Round(loc1.Curve.GetEndPoint(0).Y,5) == Math.Round(loc1.Curve.GetEndPoint(1).Y,5);
                    Curve l2curve = loc2.Curve;
                    XYZ linedir = (loc1.Curve as Line).Direction;
                    XYZ dirvect = new XYZ(-linedir.Y, linedir.X, 0.0);
                    Line intersectline = Line.CreateUnbound(loc1.Curve.Evaluate(0.5, true), dirvect);
                    intersectline.Intersect(l2curve, out IntersectionResultArray resarray);
                    XYZ intersectpnt = resarray.get_Item(0).XYZPoint;

                    double curdist = intersectpnt.DistanceTo(loc1.Curve.Evaluate(0.5, true));
                    double pipedist = Convert.ToDouble(view.Scale) * Properties.Settings.Default.pipedist / 1152;
                    double movedist = curdist - pipedist;
                    
                    XYZ vector = new XYZ();
                    if (ishor || loc1.Curve.Evaluate(0.5, true).X < intersectpnt.X)
                    {
                        vector = movedist * dirvect;
                    }
                    else if (loc1.Curve.Evaluate(0.5, true).X > intersectpnt.X)
                    {
                        vector = -movedist * dirvect;
                    }

                    using (Transaction tx = new Transaction(doc, "Space Piping"))
                    {
                        if (tx.Start() == TransactionStatus.Started)
                        {
                            loc2.Move(vector);
                        }

                        tx.Commit();
                    }
                }
                catch (NullReferenceException)
                {
                    TaskDialog td = new TaskDialog("Invalid Selection");
                    td.MainContent = "One or more of the items picked was not a pipe.";
                    td.Show();
                    return Result.Failed;
                }
            return Result.Succeeded;
            }
        }
    }


}
