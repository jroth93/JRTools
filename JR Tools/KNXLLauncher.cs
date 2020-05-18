using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
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
            ProjectInfo projectInfo = doc.ProjectInformation;
            Autodesk.Revit.DB.Parameter pnpar = projectInfo.GetParameters("MEI Project Number")[0];
            double pn = pnpar.AsDouble();

            String excelfilepath = Environment.GetFolderPath(Environment.SpecialFolder.UserProfile) + "\\Morrissey Engineering, Inc\\All Morrissey - Documents\\Keynotes\\" + $"{pn}" + ".xlsx";
            Excel.Application xl = new Excel.Application();

            if (pn == 0)
            {
                TaskDialog td = new TaskDialog("No Project Number");
                td.MainContent = "No project number entered yet. Please enter project number under MEI Project Number parameter in Project Information.";
                td.Show();
                return Result.Failed;
            }
            else if (!File.Exists(excelfilepath))
            {
                object misValue = System.Reflection.Missing.Value;
                xl.Workbooks.Add(Type.Missing);
                Excel.Workbook wb = xl.ActiveWorkbook;
                wb.SaveAs(excelfilepath);
            }
            else
            {
                xl.Workbooks.Open(excelfilepath);
            }

            
            string caption = xl.Caption;
            IntPtr handler = FindWindow(null, caption);
            SetForegroundWindow(handler);
            xl.Visible = true;
            
            return Result.Succeeded;
        }

        [DllImport("user32.dll")]
        [return: MarshalAs(UnmanagedType.Bool)]
        static extern bool SetForegroundWindow(IntPtr hWnd);

        [DllImport("user32.dll", SetLastError = true)]
        static extern System.IntPtr FindWindow(string lpClassName, string lpWindowName);
    }
}
