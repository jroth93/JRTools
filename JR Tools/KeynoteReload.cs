using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Autodesk.Revit.DB;
using Autodesk.Revit.UI;
using System.Runtime.InteropServices;
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
            Boolean oldfile = false;
            String filepath = Autodesk.Revit.DB.ModelPathUtils.ConvertModelPathToUserVisiblePath(modelpath);
            String filedirectory = Path.GetDirectoryName(filepath);
            String filename = Path.GetFileName(filepath);
            String projectnumber = filename.Remove(5);
            String excelfilepath = filedirectory + "\\" + projectnumber + " Keynotes.xlsx";
            if(!File.Exists(excelfilepath)) { excelfilepath = filedirectory + "\\" + projectnumber + " Keynotes.xlsm"; oldfile = true; }
            String txtfilepath = filedirectory + "\\" + projectnumber + " Keynotes.txt";

            Excel.Application myExcel;
            Excel.Workbook myWorkbook;
            Excel.Worksheet worksheet;

            myExcel = new Excel.Application();
            myExcel.Workbooks.Open(excelfilepath);

            myWorkbook = myExcel.ActiveWorkbook;
            string kntext = "";

            for (int i=1; i < myWorkbook.Worksheets.Count+1; i++)
            {
                int n = 1;  
                worksheet = myWorkbook.Worksheets[i];
                Excel.Range xlrange = worksheet.UsedRange;
                kntext += worksheet.Name + "\r\n";
                do
                {
                    if (oldfile) 
                    {
                        if (xlrange.Cells[n, 2].Value != null)
                        {   if(worksheet.Name.Contains("DEMO"))
                            {
                                if (n < 10)
                                {
                                    kntext += $"{worksheet.Name[0]}00{xlrange.Cells[n, 1].Value}\t{xlrange.Cells[n, 2].Value}\t{worksheet.Name}\r\n";
                                }
                                else if(n>= 10 && n<100)
                                {
                                    kntext += $"{worksheet.Name[0]}0{xlrange.Cells[n, 1].Value}\t{xlrange.Cells[n, 2].Value}\t{worksheet.Name}\r\n";
                                }
                                else
                                {
                                    kntext += $"{worksheet.Name[0]}{xlrange.Cells[n, 1].Value}\t{xlrange.Cells[n, 2].Value}\t{worksheet.Name}\r\n";
                                }
                            }
                            else
                            {
                                kntext += $"{worksheet.Name[0]}{xlrange.Cells[n, 1].Value}\t{xlrange.Cells[n, 2].Value}\t{worksheet.Name}\r\n";
                            }
                        }
                    }
                    else { kntext += $"{xlrange.Cells[n, 1].Value}\t{xlrange.Cells[n, 2].Value}\t{worksheet.Name}\r\n"; }
                    n++;
                } while (xlrange.Cells[n, 1].Value != null) ;
            }

            File.WriteAllText(txtfilepath, kntext, Encoding.Default);

            myWorkbook.Close(false);
            myExcel.Quit();

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
