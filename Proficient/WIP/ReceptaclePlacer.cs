﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Autodesk.Revit.UI;
using Autodesk.Revit.DB;
using Autodesk.Revit.DB.Mechanical;
using Autodesk.Revit.UI.Selection;

namespace Proficient
{
    [Autodesk.Revit.Attributes.Transaction(Autodesk.Revit.Attributes.TransactionMode.Manual)]
    class ReceptaclePlacer : IExternalCommand
    {
        public Result Execute(ExternalCommandData revit, ref string message, ElementSet elements)
        {
            UIApplication app = revit.Application; 
            UIDocument uidoc = revit.Application.ActiveUIDocument;
            Document doc = uidoc.Document;
            Selection sel = uidoc.Selection;
            Space sp = doc.GetElement(sel.GetElementIds().First()) as Space;
            var bsList = sp.GetBoundarySegments(new SpatialElementBoundaryOptions())[0].Select(x=>x.GetCurve() as Curve);
            double per = bsList.Select(x => x.Length).Sum();

            View view = doc.GetElement(uidoc.ActiveView.Id) as View;

            

            using (Transaction tx = new Transaction(doc, "commandname"))
            {
                if (tx.Start() == TransactionStatus.Started)
                {
                    foreach (Line l in bsList)
                    {
                        doc.Create.NewDetailCurve(view, l);
                    }
                }

                tx.Commit();
            }

            XYZ pl = uidoc.Selection.PickPoint();

            Util.BalloonTip("", pl.ToString(), "");

            return Result.Succeeded;
        }
    }
}
