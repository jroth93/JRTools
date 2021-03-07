using System; 
using System.Text;
using System.IO;
using Autodesk.Revit.DB;
using Autodesk.Revit.UI;
using XL = Microsoft.Office.Interop.Excel;

namespace Proficient
{
    [Autodesk.Revit.Attributes.Transaction(Autodesk.Revit.Attributes.TransactionMode.Manual)]
    class KeynoteReload : IExternalCommand
    {
        public Result Execute(ExternalCommandData revit, ref string message, ElementSet elements)
        {
            UIDocument uidoc = revit.Application.ActiveUIDocument;
            UIApplication uiapp = revit.Application;
            Document doc = uidoc.Document;

            string pn = Util.GetProjectNumber(revit);
            if (String.IsNullOrEmpty(pn)) return Result.Cancelled;

            string filePath = ModelPathUtils.ConvertModelPathToUserVisiblePath(doc.GetWorksharingCentralModelPath());
            bool oldFile = Path.GetExtension(filePath) == ".xlsm";
            string fileDir = filePath.Substring(0,7)== "BIM 360" ? Util.GetProjectFolder(revit) : Path.GetDirectoryName(filePath);
            if (String.IsNullOrEmpty(fileDir)) return Result.Cancelled;
            
            string xlPath = Util.GetKNXLPath(fileDir, pn);
            String txtFilePath = $@"{fileDir}\{pn} Keynotes.txt";
            bool xlReadOnly = false;

            if(fileDir.Contains("Morrissey"))
            {
                try
                {
                    FileStream stream = File.Open(xlPath, FileMode.Open, FileAccess.Read);
                    stream.Close();
                }
                catch (IOException ex)
                {
                    if (ex.Message.Contains("being used by another process"))
                    {
                        File.Copy(xlPath, xlPath + "temp.xlsx");
                        xlPath += "temp.xlsx";
                        xlReadOnly = true;
                    }
                }
            }

            XL.Application xl = new XL.Application();
            XL.Workbook wb = xl.Workbooks.Open(Filename: xlPath, ReadOnly: true);

            string knText = String.Empty;

            foreach(XL.Worksheet ws in wb.Worksheets)
            {
                knText += ws.Name + "\r\n";
                XL.Range rng = ws.UsedRange;
                for(int row=1; row<=rng.Rows.Count; row++)
                {
                    knText += oldFile ? MacroFilePatch(ws.Name, rng, row) : $"{rng.Cells[row, 1].Value}\t{rng.Cells[row, 2].Value}\t{ws.Name}\r\n";
                }
            }

            if (xlReadOnly) File.Delete(wb.FullName);
            else wb.Close(false);
            xl.Quit();

            File.WriteAllText(txtFilePath, knText, Encoding.Default);

            using (Transaction tx = new Transaction(doc, "Reload Keynotes"))
            {
                if (tx.Start() == TransactionStatus.Started)
                {
                    ModelPath p = ModelPathUtils.ConvertUserVisiblePathToModelPath(txtFilePath);
                    ExternalResourceReference s = ExternalResourceReference.CreateLocalResource(doc, ExternalResourceTypes.BuiltInExternalResourceTypes.KeynoteTable, p, PathType.Absolute);
                    KeynoteTable.GetKeynoteTable(doc).LoadFrom(s, null);
                }
                tx.Commit();
            }
            return Result.Succeeded;
        }

        private string MacroFilePatch(string wsName, XL.Range rng, int row)
        {
            if (wsName.Contains("DEMO") && row <= 100)
            {
                if (row <= 10) return $"{wsName[0]}00{rng.Cells[row, 1].Value}\t{rng.Cells[row, 2].Value}\t{wsName}\r\n";
                else return $"{wsName[0]}0{rng.Cells[row, 1].Value}\t{rng.Cells[row, 2].Value}\t{wsName}\r\n";
            }
            else
            {
                return $"{wsName[0]}{rng.Cells[row, 1].Value}\t{rng.Cells[row, 2].Value}\t{wsName}\r\n";
            }
        }        
    }
}
