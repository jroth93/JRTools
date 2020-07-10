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

                    bool isvertical = Math.Round(loc1.Curve.GetEndPoint(0).X, 4) == Math.Round(loc1.Curve.GetEndPoint(1).X, 4) ? true : false;

                    double pipedistance = Convert.ToDouble(view.Scale) / 128;
                    double currentdistance = isvertical ? loc1.Curve.GetEndPoint(0).X - loc2.Curve.GetEndPoint(0).X : loc1.Curve.GetEndPoint(0).Y - loc2.Curve.GetEndPoint(0).Y;
                    double movedistance = currentdistance < 0 ? currentdistance + pipedistance : currentdistance - pipedistance;
                    XYZ vector = isvertical ? new XYZ(movedistance, 0, 0) : new XYZ(0, movedistance, 0);

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
            }
        }
    }

    class PipeSpacerSettings
    {
        public Result Execute(ExternalCommandData revit, ref string message, ElementSet elements)
        {


            return Result.Succeeded;
        }
    }
}
