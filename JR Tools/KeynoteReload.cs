using System; 
using System.Text;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Runtime.InteropServices;
using Autodesk.Revit.DB;
using Autodesk.Revit.UI;
using Excel = Microsoft.Office.Interop.Excel;


namespace JR_Tools
{
    [Autodesk.Revit.Attributes.Transaction(Autodesk.Revit.Attributes.TransactionMode.Manual)]
    class KeynoteReload : IExternalCommand
    {
        public Autodesk.Revit.UI.Result Execute(ExternalCommandData revit, ref string message, ElementSet elements)
        {
            UIDocument uidoc = revit.Application.ActiveUIDocument;
            Document doc = uidoc.Document;
            ModelPath modelpath = doc.GetWorksharingCentralModelPath();
            bool oldfile = false;

            string filepath = Autodesk.Revit.DB.ModelPathUtils.ConvertModelPathToUserVisiblePath(modelpath);
            string filedirectory = Path.GetDirectoryName(filepath);    
            ProjectInfo projectInfo = doc.ProjectInformation;
            Parameter pnpar = projectInfo.GetParameters("MEI Project Number")[0];
            string pn = Convert.ToString(pnpar.AsDouble());
            string xlpath = "";
            bool xl365 = false;

            if (File.Exists($"{filedirectory}\\{pn} Keynotes.xlsx"))
            {
                xlpath = $"{filedirectory}\\{pn} Keynotes.xlsx";
            }
            else if (File.Exists($"{filedirectory}\\{pn} Keynotes.xlsm"))
            {
                xlpath = $"{filedirectory}\\{pn} Keynotes.xlsm";
                oldfile = true;
            }
            else if(File.Exists(Environment.GetFolderPath(Environment.SpecialFolder.UserProfile) + "\\Morrissey Engineering, Inc\\All Morrissey - Documents\\Keynotes\\" + pn + ".xlsx"))
            {
                xlpath = Environment.GetFolderPath(Environment.SpecialFolder.UserProfile) + "\\Morrissey Engineering, Inc\\All Morrissey - Documents\\Keynotes\\" + pn + ".xlsx";
                xl365 = true;
            }
            else
            {
                TaskDialog td = new TaskDialog("No Excel File");
                td.MainContent = "No Excel file found. Please create file manually or press Open Keynotes button";
                td.Show();
                return Result.Failed;
            }

            String txtfilepath = filedirectory + "\\" + pn + " Keynotes.txt";

            Excel.Application xl = new Excel.Application();
            Excel.Workbook xlwb;
            Excel.Worksheet xlws;
            bool isopen = false;

            if(xl365)
            {
                FileStream stream = null;
                try
                {
                    stream = File.Open(xlpath, System.IO.FileMode.Open, System.IO.FileAccess.Read);
                }
                catch (IOException ex)
                {
                    if (ex.Message.Contains("being used by another process"));
                    File.Copy(xlpath, xlpath + "temp.xlsx");
                    xlpath += "temp.xlsx";
                    isopen = true;
                }
                finally
                {
                    if (stream != null)
                        stream.Close();
                }
            }

            xl.Workbooks.Open(xlpath, 0, true);
            xlwb = xl.ActiveWorkbook;
            string kntext = "";

            for (int i=1; i < xlwb.Worksheets.Count+1; i++)
            {
                int n = 1;  
                xlws = xlwb.Worksheets[i];
                Excel.Range xlrange = xlws.UsedRange;
                kntext += xlws.Name + "\r\n";
                do
                {
                    if (oldfile) 
                    {
                        if (xlrange.Cells[n, 2].Value != null)
                        {   if(xlws.Name.Contains("DEMO"))
                            {
                                if (n < 10)
                                {
                                    kntext += $"{xlws.Name[0]}00{xlrange.Cells[n, 1].Value}\t{xlrange.Cells[n, 2].Value}\t{xlws.Name}\r\n";
                                }
                                else if(n>= 10 && n<100)
                                {
                                    kntext += $"{xlws.Name[0]}0{xlrange.Cells[n, 1].Value}\t{xlrange.Cells[n, 2].Value}\t{xlws.Name}\r\n";
                                }
                                else
                                {
                                    kntext += $"{xlws.Name[0]}{xlrange.Cells[n, 1].Value}\t{xlrange.Cells[n, 2].Value}\t{xlws.Name}\r\n";
                                }
                            }
                            else
                            {
                                kntext += $"{xlws.Name[0]}{xlrange.Cells[n, 1].Value}\t{xlrange.Cells[n, 2].Value}\t{xlws.Name}\r\n";
                            }
                        }
                    }
                    else { kntext += $"{xlrange.Cells[n, 1].Value}\t{xlrange.Cells[n, 2].Value}\t{xlws.Name}\r\n"; }
                    n++;
                } while (xlrange.Cells[n, 1].Value != null) ;
            }

            File.WriteAllText(txtfilepath, kntext, Encoding.Default);

            xlwb.Close(false);
            xl.Quit();

            if (isopen)
            {
                File.Delete(xlpath);
            }

            using (Transaction tx = new Transaction(doc, "Reload Keynotes"))
            {
                if (tx.Start() == TransactionStatus.Started)
                {
                    ModelPath p = ModelPathUtils.ConvertUserVisiblePathToModelPath(txtfilepath);
                    ExternalResourceReference s = ExternalResourceReference.CreateLocalResource(doc, ExternalResourceTypes.BuiltInExternalResourceTypes.KeynoteTable, p, PathType.Absolute);
                    KeyBasedTreeEntryTable kt = KeynoteTable.GetKeynoteTable(doc);
                    kt.LoadFrom(s, null);
                }
                tx.Commit();
            }
            return Autodesk.Revit.UI.Result.Succeeded;
        }
    }
}
