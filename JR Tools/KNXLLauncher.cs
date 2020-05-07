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
    class KNXLLauncher : IExternalCommand
    {
        public Autodesk.Revit.UI.Result Execute(ExternalCommandData revit, ref string message, ElementSet elements)
        {
            UIDocument uidoc = revit.Application.ActiveUIDocument;
            Document doc = uidoc.Document;
            ModelPath modelpath = doc.GetWorksharingCentralModelPath();
            String filepath = Autodesk.Revit.DB.ModelPathUtils.ConvertModelPathToUserVisiblePath(modelpath);
            String filedirectory = Path.GetDirectoryName(filepath);
            String filename = Path.GetFileName(filepath);
            String projectnumber = filename.Remove(5);
            String excelfilepath = Environment.GetFolderPath(Environment.SpecialFolder.UserProfile) + "\\Morrissey Engineering, Inc\\All Morrissey - Documents\\Keynotes\\" + projectnumber + ".xlsx";
            if (!File.Exists(excelfilepath)) { excelfilepath = filedirectory + "\\" + projectnumber + " Keynotes.xlsm"; oldfile = true; }
            String txtfilepath = filedirectory + "\\" + projectnumber + " Keynotes.txt";

            Excel.Application myExcel;
            Excel.Workbook myWorkbook;
            Excel.Worksheet worksheet;

            myExcel = new Excel.Application();
            myExcel.Workbooks.Open(excelfilepath);
            myWorkbook = myExcel.ActiveWorkbook;
        }
    }
}
