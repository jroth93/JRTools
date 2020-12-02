using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Autodesk.Revit.UI;
using Autodesk.Revit.DB;
using Autodesk.Revit.UI.Selection;

namespace JR_Tools
{
    [Autodesk.Revit.Attributes.Transaction(Autodesk.Revit.Attributes.TransactionMode.Manual)]
    class KeynoteUtil : IExternalCommand
    {
        public Result Execute(ExternalCommandData revit, ref string message, ElementSet elements)
        {
            UIApplication app = revit.Application; 
            UIDocument uidoc = revit.Application.ActiveUIDocument;
            Document doc = uidoc.Document;
            KeynoteUtilFrm kuf = new KeynoteUtilFrm();

            FilteredElementCollector fecViewSheet = new FilteredElementCollector(doc).OfClass(typeof(ViewSheet));
            var fecPlacedKeynotes = new FilteredElementCollector(doc).OfClass(typeof(IndependentTag)).Where(x => x.Name.Contains("Keynote"));
            KeyBasedTreeEntries kte = (KeynoteTable.GetKeynoteTable(doc) as KeyBasedTreeEntryTable).GetKeyBasedTreeEntries();
            List<string> sheetsPlaced = new List<string>();

            foreach(KeynoteEntry ke in kte)
            {
                if (ke.KeynoteText != "")
                {
                    sheetsPlaced.Clear();
                    sheetsPlaced.Add("None");
                    foreach (IndependentTag it in fecPlacedKeynotes)
                    {
                        try
                        {
                            if (it.LookupParameter("Key Value").AsString()  == ke.Key)
                            {
                                ViewPlan vp = doc.GetElement(it.OwnerViewId) as ViewPlan;
                                if (sheetsPlaced[0] == "None")
                                {
                                    sheetsPlaced[0] = getSheetNumber(doc, vp, fecViewSheet);
                                }
                                else
                                {
                                    sheetsPlaced.Add(getSheetNumber(doc, vp, fecViewSheet));
                                }
                            }
                        }
                        catch { }
                    }
                    sheetsPlaced.Sort();
                    string[] row = { ke.Key, ke.KeynoteText, String.Join(", ", sheetsPlaced.Distinct().ToArray()) };
                    kuf.dgv.Rows.Add(row);
                }
            }

            kuf.Show();
            
            return Result.Succeeded;
        }
        public string getSheetNumber(Document doc, ViewPlan viewPlan, FilteredElementCollector fecVS)
        {

            foreach(ViewSheet vs in fecVS)
            {
                var vsViews = vs.GetAllPlacedViews().Select(x=> doc.GetElement(x)).Where(x => x.GetType() == typeof(ViewPlan));
                foreach(ViewPlan vp in vsViews)
                {
                    if(vp.Id == viewPlan.Id)
                    {
                        return vs.SheetNumber;
                    }
                }
                
            }

            return null;
        }
    }


}
