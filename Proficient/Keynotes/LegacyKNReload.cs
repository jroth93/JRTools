using Autodesk.Revit.DB;
using Autodesk.Revit.DB.ExternalService;
using Autodesk.Revit.UI;
using System;
using System.Collections.Generic;
using System.IO;
using System.Windows.Forms;
using XL = Microsoft.Office.Interop.Excel;

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

            string fileDir = filePath.Substring(0, 7) == "BIM 360" ? Util.GetProjectFolder(revit) : Path.GetDirectoryName(filePath);
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
                    KeynoteEntry ke = null;
                    if (rng.Cells[row, 1].Value != null && rng.Cells[row, 1].Value != String.Empty && rng.Cells[row, 2].Value != null && rng.Cells[row, 2].Value != String.Empty)
                    {
                        ke = oldFile ? MacroFilePatch(ws.Name, rng, row) : new KeynoteEntry(rng.Cells[row, 1].Value, ws.Name, rng.Cells[row, 2].Value);
                        knList.Add(ke);
                    }
                }

            }

            wb.Close(false);
            xl.Quit();

            ExternalService externalResourceService = ExternalServiceRegistry.GetService(ExternalServices.BuiltInExternalServices.ExternalResourceService);
            ExternalResourceDBServer knSrv = externalResourceService.GetServer(KNReload.dbID) as ExternalResourceDBServer;

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
