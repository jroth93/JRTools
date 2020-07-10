using System;
using System.Collections.Generic;
using System.Linq;
using Autodesk.Revit.UI;
using Autodesk.Revit.DB;
using Autodesk.Revit.UI.Selection;


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
            Autodesk.Revit.DB.View view = doc.GetElement(viewid) as Autodesk.Revit.DB.View;
            Reference cref = uidoc.Selection.PickObject(ObjectType.Element, "Pick path detail line");
            Reference elref = uidoc.Selection.PickObject(ObjectType.Element, "Pick element to be placed");

            Curve pathcurve = (doc.GetElement(cref).Location as LocationCurve).Curve;
            if(!pathcurve.IsBound)
            {
                TaskDialog td = new TaskDialog("Unbounded Curve");
                td.MainContent = "Please try again with a bounded curve.";
                td.Show();
                return Result.Failed;
            }

            FamilySymbol elfs = (doc.GetElement(elref.ElementId) as FamilyInstance).Symbol;
            PlaceElFrm pef = new PlaceElFrm();
            pef.ShowDialog();

            bool rbnm = pef.radionumber.Checked;
            double usrin = Convert.ToDouble(pef.textBox1.Text);
            double offset = rbnm ? 0 : Convert.ToDouble(pef.startoffset.Text);
            pef.Close();

            double stepsize = rbnm ? pathcurve.Length/(usrin-1) : usrin;
            double dist = offset == 0 ? 0 : stepsize - offset;

            List<XYZ> tess = new List<XYZ>();
            List<XYZ> deriv = new List<XYZ>();
            List<XYZ> pts = new List<XYZ>();
            List<XYZ> newdir = new List<XYZ>();

            if (pathcurve as Line != null)
            {
                List<double> eval = new List<double>();

                if(rbnm)
                {
                    for(int i=0; i<usrin; i++)
                    {
                        pts.Add(pathcurve.Evaluate(i * stepsize / pathcurve.Length, true));
                        newdir.Add(pathcurve.ComputeDerivatives(i * stepsize / pathcurve.Length, true).get_Basis(0));
                    }

                }
                else
                {
                    double curlen = offset;
                    while(curlen < pathcurve.Length)
                    {
                        pts.Add(pathcurve.Evaluate(curlen / pathcurve.Length, true));
                        newdir.Add(pathcurve.ComputeDerivatives(curlen / pathcurve.Length, true).get_Basis(0));
                        curlen += stepsize;
                    }

                }
            }
            else
            {
                double curpar = pathcurve.GetEndParameter(0);
                while (curpar <= pathcurve.GetEndParameter(1))
                {
                    tess.Add(pathcurve.Evaluate(curpar, false));
                    deriv.Add(pathcurve.ComputeDerivatives(curpar, false).get_Basis(0));
                    curpar += 0.001;
                }

                XYZ p = pathcurve.GetEndPoint(0);

                foreach (XYZ q in tess)
                {
                    if (0 == pts.Count && 0 == offset)
                    {
                        pts.Add(p);
                        dist = 0.0;
                        newdir.Add(deriv[0]);
                    }
                    else
                    {
                        dist += p.DistanceTo(q);
                        if (dist == stepsize)
                        {
                            pts.Add(q);
                            newdir.Add(deriv[tess.IndexOf(q)]);
                            dist = 0;
                        }
                        else if (dist > stepsize)
                        {
                            pts.Add((p + q) / 2);
                            dist = 0;
                            newdir.Add(deriv[tess.IndexOf(q)]);
                        }
                        p = q;
                    }

                    if (rbnm && pts.Count == usrin - 1)
                    {
                        pts.Add(tess.Last());
                        newdir.Add(deriv.Last());
                        break;
                    }
                } 
            }

            using (Transaction tx = new Transaction(doc, "Place Elements"))
            {
                if (tx.Start() == TransactionStatus.Started)
                {
                    foreach (XYZ pt in pts)
                    {
                        FamilyInstance newel = doc.Create.NewFamilyInstance(pt, elfs, view.GenLevel, Autodesk.Revit.DB.Structure.StructuralType.NonStructural);
                        Line rotax = Line.CreateBound(pt, pt.Add(XYZ.BasisZ));
                        double rotangle = newdir[pts.IndexOf(pt)].Y < 0 ? -XYZ.BasisX.AngleTo(newdir[pts.IndexOf(pt)]) : XYZ.BasisX.AngleTo(newdir[pts.IndexOf(pt)]);
                        ElementTransformUtils.RotateElement(doc, newel.Id, rotax, rotangle);

                    }
                }

                tx.Commit();
            }
            return Result.Succeeded;
        }
    }
}
