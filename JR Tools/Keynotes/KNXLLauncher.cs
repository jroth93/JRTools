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
            UIApplication uiapp = revit.Application;
            Document doc = uidoc.Document;
            ProjectInfo projectInfo = doc.ProjectInformation;

            (string pn, bool blcont1) = KeynoteReload.GetProjectNumber(doc, uiapp);
            if (!blcont1) { return Result.Cancelled; }

            string kndir = Environment.GetFolderPath(Environment.SpecialFolder.UserProfile) + "\\Morrissey Engineering, Inc\\All Morrissey - Documents\\Keynotes\\";
            string xlpath =  $"{kndir}{pn}.xlsx";
            string tmppath = $"{kndir}Template.xlsx";
            Excel.Application xl = new Excel.Application();

            ModelPath modelpath = doc.GetWorksharingCentralModelPath();
            string filepath = Autodesk.Revit.DB.ModelPathUtils.ConvertModelPathToUserVisiblePath(modelpath);
            (string filedirectory, bool blcont2) = Path.GetDirectoryName(filepath).Substring(0, 7) == "BIM 360" ? KeynoteReload.GetCloudProjectFolder(doc, uiapp) : (Path.GetDirectoryName(filepath), true);
            if (!blcont2) { return Result.Cancelled; }

            //legacy files
            bool oldfile = false;
            if (File.Exists($"{filedirectory}\\{pn} Keynotes.xlsx"))
            {
                xlpath = $"{filedirectory}\\{pn} Keynotes.xlsx";
                oldfile = true;
            }
            else if (File.Exists($"{filedirectory}\\{pn} Keynotes.xlsm"))
            {
                xlpath = $"{filedirectory}\\{pn} Keynotes.xlsm";
                oldfile = true;
            }

            if (!oldfile)
            {
                if (pn == "0")
                {
                    TaskDialog td = new TaskDialog("No Project Number");
                    td.MainContent = "No project number entered yet. Please enter project number under MEI Project Number parameter in Project Information.";
                    td.Show();
                    return Result.Failed;
                }
                //create new file or generate error if file does not exist
                else if (!File.Exists(xlpath))
                {
                    try
                    {
                        File.Copy(tmppath, xlpath);
                    }
                    catch
                    {
                        TaskDialog td = new TaskDialog("SharePoint Sync Required");
                        td.MainContent = @"All Morrissey Team SharePoint sync is required to use this feature. Please navigate here and click sync: https://morrisseyengineering.sharepoint.com/sites/AllMorrissey/Shared%20Documents/Forms/AllItems.aspx";
                        td.Show();
                        return Result.Failed;
                    }
                    string cfiledir = "";
                    if (doc.IsWorkshared)
                    {
                        cfiledir = Path.GetDirectoryName(Autodesk.Revit.DB.ModelPathUtils.ConvertModelPathToUserVisiblePath(doc.GetWorksharingCentralModelPath()));
                    }
                    else
                    {
                        cfiledir = Path.GetDirectoryName(doc.PathName);
                    }
                    if (!File.Exists(cfiledir + $"\\Keynotes.lnk"))
                    {
                        IWshRuntimeLibrary.WshShell shell = new IWshRuntimeLibrary.WshShell();
                        IWshRuntimeLibrary.IWshShortcut shortcut = (IWshRuntimeLibrary.IWshShortcut)shell.CreateShortcut(cfiledir + $"\\Keynotes.lnk");
                        shortcut.TargetPath = xlpath;
                        shortcut.Save();
                    }
                }
            }

            xl.Workbooks.Open(xlpath);
            
            //bring to front
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
