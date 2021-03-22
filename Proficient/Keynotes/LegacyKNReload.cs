using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using System.IO;
using Autodesk.Revit.DB;
using Autodesk.Revit.UI;
using Autodesk.Revit.DB.ExternalService;
using XL = Microsoft.Office.Interop.Excel;
using System.Windows.Forms;

namespace Proficient
{
    [Autodesk.Revit.Attributes.Transaction(Autodesk.Revit.Attributes.TransactionMode.Manual)]
    class LegacyKNReload : IExternalCommand
    {

        public Result Execute(ExternalCommandData revit, ref string message, ElementSet elements)
        {
            Document doc = revit.Application.ActiveUIDocument.Document;

            List<KeynoteEntry> knList = new List<KeynoteEntry>();

            string pn = Util.GetProjectNumber(revit);
            if (String.IsNullOrEmpty(pn)) return Result.Cancelled;

            string filePath = ModelPathUtils.ConvertModelPathToUserVisiblePath(doc.GetWorksharingCentralModelPath());
            
            string fileDir = doc.IsModelInCloud ? Util.GetProjectFolder(revit) : Path.GetDirectoryName(filePath);
            if (String.IsNullOrEmpty(fileDir)) return Result.Cancelled;

            string xlPath = Util.GetKNXLPath(fileDir, pn);
            bool oldFile = Path.GetExtension(xlPath) == ".xlsm";

            if (xlPath.Contains("Morrissey"))
            {
                MessageBox.Show("Keynotes located on the cloud. Please use new keynotes tools");
                return Result.Failed;
            }

            XL.Application xl = new XL.Application();
            XL.Workbook wb = xl.Workbooks.Open(Filename: xlPath, ReadOnly: true);

            foreach (XL.Worksheet ws in wb.Worksheets)
            {
                knList.Add(new KeynoteEntry(ws.Name, string.Empty, string.Empty));

                XL.Range rng = ws.UsedRange;
                for (int row = 1; row <= rng.Rows.Count; row++)
                {
                    KeynoteEntry ke = oldFile ? MacroFilePatch(ws.Name, rng, row) : new KeynoteEntry(rng.Cells[row, 1].Value, ws.Name, rng.Cells[row, 2].Value);
                    if(ke != null) knList.Add(ke);
                }
                    
            }

            wb.Close(false);
            xl.Quit();

            ExternalService externalResourceService = ExternalServiceRegistry.GetService(ExternalServices.BuiltInExternalServices.ExternalResourceService);
            ExternalResourceDBServer knSrv = externalResourceService.GetServer(new Guid("5F3CAA13-F073-4F93-BDC2-B7F4B806CDAF")) as ExternalResourceDBServer;

            knSrv.knList = knList;

            using (Transaction tx = new Transaction(doc, "Reload Keynotes"))
            {
                if (tx.Start() == TransactionStatus.Started)
                {
                    ModelPath p = ModelPathUtils.ConvertUserVisiblePathToModelPath("KNServer://Keynotes.txt");
                    ExternalResourceReference s = ExternalResourceReference.CreateLocalResource(doc, ExternalResourceTypes.BuiltInExternalResourceTypes.KeynoteTable, p, PathType.Absolute);
                    KeynoteTable.GetKeynoteTable(doc).LoadFrom(s, null);
                }
                tx.Commit();
            }
            return Result.Succeeded;
        }

        private static KeynoteEntry MacroFilePatch(string wsName, XL.Range rng, int row)
        {
            if (rng.Cells[row, 1].Value == null || rng.Cells[row, 2].Value == null) return null;

            if (wsName.Contains("DEMO") && row <= 100)
            {
                if (row <= 10) return new KeynoteEntry($"{wsName[0]}00{rng.Cells[row, 1].Value}", wsName, rng.Cells[row, 2].Value);
                else return new KeynoteEntry($"{wsName[0]}0{rng.Cells[row, 1].Value}", wsName, rng.Cells[row, 2].Value);
            }
            else
            {
                return new KeynoteEntry($"{wsName[0]}{rng.Cells[row, 1].Value}", wsName, rng.Cells[row, 2].Value);
            }
        }        
    }
}
