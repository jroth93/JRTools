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
using System.Windows.Forms;
using System.IO;

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
            Reference cref = uidoc.Selection.PickObject(ObjectType.Element, "Pick Path Detail Line");
            Reference elref = uidoc.Selection.PickObject(ObjectType.Element, "Pick element to be placed");

            Curve pathcurve = (doc.GetElement(cref).Location as LocationCurve).Curve;


            FamilySymbol elfs = (doc.GetElement(elref.ElementId) as FamilyInstance).Symbol;
            PlaceElFrm pef = new PlaceElFrm();
            pef.ShowDialog();

            double startparam = pathcurve.GetEndParameter(0);
            bool rbnm = pef.radionumber.Checked;

            double usrin = Convert.ToDouble(pef.textBox1.Text);
            double stepsize = rbnm ? pathcurve.Length/(usrin-1) : usrin;
            double offset = rbnm ? 0 : Convert.ToDouble(pef.startoffset.Text);
            double dist = offset == 0 ? 0 : stepsize - offset;

            IList<XYZ> tess = new List<XYZ>();//pathcurve.Tessellate();
            IList<XYZ> deriv = new List<XYZ>();
            /*
            for(int i=0; i<=100000; i++)
            {
                tess.Add(pathcurve.Evaluate(i / 100000.0, true));
            }
            */
            double curpar = pathcurve.GetEndParameter(0);
            while(curpar <= pathcurve.GetEndParameter(1))
            {
                tess.Add(pathcurve.Evaluate(curpar, false));
                deriv.Add(pathcurve.ComputeDerivatives(curpar, false).get_Basis(0));
                curpar += 0.0001;
            }
            
            XYZ p = pathcurve.GetEndPoint(0);
            List<XYZ> pts = new List<XYZ>();
            List<XYZ> newdir = new List<XYZ>();

            int i = 0;
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
                        pts.Add((p+q)/2);
                        dist = 0;
                        newdir.Add(deriv[tess.IndexOf(q)]);
                    }
                    p = q;
                }

                if(rbnm && pts.Count == usrin - 1)
                {
                    pts.Add(tess.Last());
                    newdir.Add(deriv.Last());
                    break;
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
                        double rotangle = XYZ.BasisX.AngleTo(newdir[pts.IndexOf(pt)]);
                        ElementTransformUtils.RotateElement(doc, newel.Id, rotax, rotangle);

                        /*
                        double curpar = startparam;
                        double curlen = 0;
                        while (curpar < pathcurve.GetEndParameter(1))
                        {
                            XYZ newloc = pathcurve.Evaluate(curpar, false);
                            Line rotax = Line.CreateBound(newloc, newloc.Add(XYZ.BasisZ));
                            XYZ newdir = pathcurve.ComputeDerivatives(curpar, false).get_Basis(0);
                            double rotangle = XYZ.BasisX.AngleTo(newdir);

                            FamilyInstance newel = doc.Create.NewFamilyInstance(newloc, elfs, view.GenLevel, Autodesk.Revit.DB.Structure.StructuralType.NonStructural);
                            ElementTransformUtils.RotateElement(doc, newel.Id, rotax, rotangle);
                            curpar += stepsize;
                            curlen += stepsize;
                        }
                        */

                    }
                }

                tx.Commit();
            }
            pef.Close();
            return Result.Succeeded;
        }
    }
}
