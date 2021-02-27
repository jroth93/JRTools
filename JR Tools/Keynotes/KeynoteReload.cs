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
using Autodesk.Revit.ApplicationServices;

namespace Proficient
{
    [Autodesk.Revit.Attributes.Transaction(Autodesk.Revit.Attributes.TransactionMode.Manual)]
    class KeynoteReload : IExternalCommand
    {
        public Autodesk.Revit.UI.Result Execute(ExternalCommandData revit, ref string message, ElementSet elements)
        {
            UIDocument uidoc = revit.Application.ActiveUIDocument;
            UIApplication uiapp = revit.Application;
            Document doc = uidoc.Document;
            ProjectInfo projectInfo = doc.ProjectInformation;

            bool oldfile = false;
            string filepath = Autodesk.Revit.DB.ModelPathUtils.ConvertModelPathToUserVisiblePath(doc.GetWorksharingCentralModelPath());
            (string filedirectory, bool blcont1) = filepath.Substring(0,7)=="BIM 360" ? GetCloudProjectFolder(doc, uiapp) : (Path.GetDirectoryName(filepath),true);
            if (!blcont1) { return Result.Cancelled; }
            (string pn, bool blcont2) = GetProjectNumber(doc, uiapp);
            if (!blcont2) { return Result.Cancelled; }

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
                    if (ex.Message.Contains("being used by another process"))
                    {
                        File.Copy(xlpath, xlpath + "temp.xlsx");
                        xlpath += "temp.xlsx";
                        isopen = true;
                    }
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

            GC.Collect();
            GC.WaitForPendingFinalizers();
            xlwb.Close(false);
            Marshal.FinalReleaseComObject(xlwb);
            xl.Quit();
            Marshal.FinalReleaseComObject(xl);

            File.WriteAllText(txtfilepath, kntext, Encoding.Default);


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

        public static (string,bool) GetCloudProjectFolder(Document doc, UIApplication uiapp)
        {
            Autodesk.Revit.Creation.Application cap = uiapp.Application.Create;
            string filedirectory = "";
            ProjectInfo projectInfo = doc.ProjectInformation;
            IList<Autodesk.Revit.DB.Parameter> parlist = projectInfo.GetParameters("MEI Project Folder");

            if (parlist.Count == 0 || parlist[0].AsString() == "" || parlist[0].AsString() == null)
            {
                System.Windows.Forms.FolderBrowserDialog fb = new System.Windows.Forms.FolderBrowserDialog();
                fb.Description = "Browse to project folder containing keynotes file (legacy) or where keynotes text file should be located.";
                if(fb.ShowDialog() == System.Windows.Forms.DialogResult.Cancel)
                {
                    return ("", false);
                }
                
                filedirectory = fb.SelectedPath;

                if (parlist.Count == 0)
                {
                    Category cat = doc.Settings.Categories.get_Item(BuiltInCategory.OST_ProjectInformation);
                    CategorySet cset = cap.NewCategorySet();
                    cset.Insert(cat);
                    uiapp.Application.SharedParametersFilename = @"Z:\Revit MEI Content\Shared Parameters\MEI Shared Parameters.txt";
                    DefinitionFile spFile = uiapp.Application.OpenSharedParameterFile();
                    foreach (DefinitionGroup dg in spFile.Groups)
                    {
                        if (dg.Name == "Titleblock")
                        {
                            var v = (from ExternalDefinition d in dg.Definitions select d);
                            using (Transaction tx = new Transaction(doc, "Add Project Folder Parameter"))
                            {
                                if (tx.Start() == TransactionStatus.Started)
                                {
                                    foreach (ExternalDefinition eD in v)
                                    {
                                        InstanceBinding newIB = cap.NewInstanceBinding(cset);
                                        if (eD.Name == "MEI Project Folder")
                                        {
                                            doc.ParameterBindings.Insert(eD, newIB, BuiltInParameterGroup.PG_GENERAL);
                                        }
                                    }
                                }
                                tx.Commit();
                            }
                        }
                    }
                }

                using (Transaction tx = new Transaction(doc, "Assign Project Folder Parameter"))
                {
                    if (tx.Start() == TransactionStatus.Started)
                    {
                        parlist = projectInfo.GetParameters("MEI Project Folder");
                        parlist[0].Set(filedirectory);
                    }
                    tx.Commit();
                }

            }
            else
            {
                filedirectory = parlist[0].AsString();
            }

            return (filedirectory, true);
        }

        public static (string,bool) GetProjectNumber(Document doc, UIApplication uiapp)
        {
            Autodesk.Revit.Creation.Application cap = uiapp.Application.Create;
            ProjectInfo projectInfo = doc.ProjectInformation;
            double dpn = 0.0;
            string pn = "";
            IList<Autodesk.Revit.DB.Parameter> parlist = projectInfo.GetParameters("MEI Project Number");

            if (parlist.Count == 0 || parlist[0].AsString() == "" || parlist[0].AsDouble() == 0)
            {
                EntryBox eb = new EntryBox();
                eb.label1.Text = "Enter Project Number (YY### or YY###.#):";
                eb.Text = "Project Number Input";

                while(true)
                {
                    if (eb.ShowDialog() == System.Windows.Forms.DialogResult.Cancel)
                    {
                        return ("", false);
                    }

                    try
                    {
                        dpn = Convert.ToDouble(eb.textBox1.Text);
                    }
                    catch(FormatException)
                    {
                        eb.label1.Text = "Invalid format - please try again.\n\nEnter Project Number (YY### or YY###.#):";
                        continue;
                    }
                    eb.Close();
                    break;                   
                }

                if (parlist.Count == 0)
                {
                    Category cat = doc.Settings.Categories.get_Item(BuiltInCategory.OST_ProjectInformation);
                    CategorySet cset = cap.NewCategorySet();
                    cset.Insert(cat);
                    DefinitionFile spFile = uiapp.Application.OpenSharedParameterFile();
                    foreach (DefinitionGroup dg in spFile.Groups)
                    {
                        if (dg.Name == "Titleblock")
                        {
                            var v = (from ExternalDefinition d in dg.Definitions select d);
                            using (Transaction tx = new Transaction(doc, "Add Project Number Parameter"))
                            {
                                if (tx.Start() == TransactionStatus.Started)
                                {
                                    foreach (ExternalDefinition eD in v)
                                    {
                                        InstanceBinding newIB = cap.NewInstanceBinding(cset);
                                        if (eD.Name == "MEI Project Number")
                                        {
                                            doc.ParameterBindings.Insert(eD, newIB, BuiltInParameterGroup.PG_GENERAL);
                                        }
                                    }
                                }
                                tx.Commit();
                            }
                        }
                    }
                }

                using (Transaction tx = new Transaction(doc, "Assign Project Number Parameter"))
                {
                    if (tx.Start() == TransactionStatus.Started)
                    {
                        parlist = projectInfo.GetParameters("MEI Project Number");
                        parlist[0].Set(dpn);
                    }
                    tx.Commit();
                }
            }

            pn = Convert.ToString(parlist[0].AsDouble());
            


            return (pn,true);
        }
    }
}
