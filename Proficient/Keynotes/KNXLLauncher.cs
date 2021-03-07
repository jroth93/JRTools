using System;
using System.IO;
using Autodesk.Revit.DB;
using Autodesk.Revit.UI;
using System.Runtime.InteropServices;
using Excel = Microsoft.Office.Interop.Excel;

namespace Proficient
{
    [Autodesk.Revit.Attributes.Transaction(Autodesk.Revit.Attributes.TransactionMode.Manual)]
    class KNXLLauncher : IExternalCommand
    {
        public Result Execute(ExternalCommandData revit, ref string message, ElementSet elements)
        {
            Document doc = revit.Application.ActiveUIDocument.Document;

            string pn = Util.GetProjectNumber(revit);
            if (String.IsNullOrEmpty(pn)) return Result.Cancelled; 
            
            string filePath = ModelPathUtils.ConvertModelPathToUserVisiblePath(doc.GetWorksharingCentralModelPath()) ?? doc.PathName;
            string fileDir = Path.GetDirectoryName(filePath).Substring(0, 7) == "BIM 360" ? Util.GetProjectFolder(revit) : Path.GetDirectoryName(filePath);
            if (String.IsNullOrEmpty(fileDir)) return Result.Cancelled;

            string xlPath = Util.GetKNXLPath(fileDir, pn);
            if (String.IsNullOrEmpty(xlPath)) return Result.Failed;
            bool isOpen = false;
            Excel.Application xl;
            try
            {
                xl = Marshal.GetActiveObject("Excel.Application") as Excel.Application;
                foreach(Excel.Workbook wb in xl.Workbooks) if (wb.Name == Path.GetFileName(xlPath)) isOpen = true;
            }
            catch(COMException)
            {
                xl = new Excel.Application();
            }

            if(!isOpen) xl.Workbooks.Open(xlPath);

            SetForegroundWindow(FindWindow(null, xl.Caption));
            xl.Visible = true;            
            
            return Result.Succeeded;
        }

        [DllImport("user32.dll")]
        [return: MarshalAs(UnmanagedType.Bool)]
        static extern bool SetForegroundWindow(IntPtr hWnd);

        [DllImport("user32.dll", SetLastError = true)]
        static extern IntPtr FindWindow(string lpClassName, string lpWindowName);
    }
}
