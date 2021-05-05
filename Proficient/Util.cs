﻿using Autodesk.Revit.DB;
using Autodesk.Revit.UI;
using System;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;

namespace Proficient
{
    class Util
    {
        [DllImport("user32.dll", SetLastError = true, CharSet = CharSet.Auto)]
        static extern int SetWindowText(IntPtr hWnd, string lpString);
        [DllImport("user32.dll", SetLastError = true)]
        static extern IntPtr FindWindowEx(IntPtr hwndParent, IntPtr hwndChildAfter, string lpszClass, string lpszWindow);

        public static void BalloonTip(string category, string title, string text)
        {
            Autodesk.Internal.InfoCenter.ResultItem ri = new Autodesk.Internal.InfoCenter.ResultItem();

            ri.Category = category;
            ri.Title = title;
            ri.TooltipText = text;

            Autodesk.Windows.ComponentManager.InfoCenterPaletteManager.ShowBalloon(ri);
        }

        public static void BalloonTip(string category, string title, string text, string uri)
        {
            Autodesk.Internal.InfoCenter.ResultItem ri = new Autodesk.Internal.InfoCenter.ResultItem();

            ri.Category = category;
            ri.Title = title;
            ri.TooltipText = text;
            ri.Uri = new Uri(uri);

            Autodesk.Windows.ComponentManager.InfoCenterPaletteManager.ShowBalloon(ri);
        }

        public static void SetStatusText(string text)
        {
            IntPtr mainWindow = Process.GetCurrentProcess().MainWindowHandle;
            IntPtr statusBar = FindWindowEx(mainWindow, IntPtr.Zero, "msctls_statusbar32", "");

            if (statusBar != IntPtr.Zero)
            {
                SetWindowText(statusBar, text);
            }
        }

        public static string GetProjectFolder(ExternalCommandData revit)
        {
            Document doc = revit.Application.ActiveUIDocument.Document;
            bool parExists = doc.ProjectInformation.GetParameters("MEI Project Folder").Count > 0;
            string projFolder = parExists ? doc.ProjectInformation.GetParameters("MEI Project Folder")[0].AsString() : String.Empty;

            if (String.IsNullOrEmpty(projFolder))
            {
                System.Windows.Forms.FolderBrowserDialog fb = new System.Windows.Forms.FolderBrowserDialog();
                fb.Description = "Browse to project folder containing keynotes file (legacy) or where keynotes text file should be located.";
                if (fb.ShowDialog() == System.Windows.Forms.DialogResult.Cancel) return String.Empty;
                projFolder = fb.SelectedPath;

                if (!parExists)
                {
                    AddSharedParameter(doc, revit.Application, BuiltInCategory.OST_ProjectInformation, BuiltInParameterGroup.PG_GENERAL, "Titleblock", "MEI Project Folder");
                }

                using (Transaction tx = new Transaction(doc, "Assign Project Folder Parameter"))
                {
                    if (tx.Start() == TransactionStatus.Started)
                    {
                        doc.ProjectInformation.GetParameters("MEI Project Folder")[0].Set(projFolder);
                    }
                    tx.Commit();
                }
            }

            return projFolder;

        }

        public static string GetProjectNumber(ExternalCommandData revit)
        {
            Document doc = revit.Application.ActiveUIDocument.Document;
            bool parExists = doc.ProjectInformation.GetParameters("MEI Project Number").Count > 0;

            string projNum = parExists ? Convert.ToString(doc.ProjectInformation.GetParameters("MEI Project Number")[0].AsDouble()) : String.Empty;

            if (String.IsNullOrEmpty(projNum) || projNum == "0")
            {
                EntryBox eb = new EntryBox("Project Number Input", "Enter Project Number (YY### or YY###.#):", typeof(double));
                if (eb.ShowDialog() == System.Windows.Forms.DialogResult.Cancel) return String.Empty;
                projNum = eb.Entry;

                if (!parExists)
                {
                    AddSharedParameter(doc, revit.Application, BuiltInCategory.OST_ProjectInformation, BuiltInParameterGroup.PG_GENERAL, "Titleblock", "MEI Project Number");
                }

                using (Transaction tx = new Transaction(doc, "Assign Project Number Parameter"))
                {
                    if (tx.Start() == TransactionStatus.Started)
                    {
                        doc.ProjectInformation.GetParameters("MEI Project Number")[0].Set(Convert.ToDouble(projNum));
                    }
                    tx.Commit();
                }
            }

            return projNum;
        }

        private static void AddSharedParameter(Document doc, UIApplication uiapp, BuiltInCategory bic, BuiltInParameterGroup bipg, string defGroup, string parName)
        {
            CategorySet cset = uiapp.Application.Create.NewCategorySet();
            cset.Insert(doc.Settings.Categories.get_Item(bic));
            uiapp.Application.SharedParametersFilename = @"Z:\Revit MEI Content\Shared Parameters\MEI Shared Parameters.txt";
            DefinitionFile spFile = uiapp.Application.OpenSharedParameterFile();

            ExternalDefinition eDef = spFile.Groups.Where(dg => dg.Name == defGroup).FirstOrDefault().Definitions.Where(ed => ed.Name == parName).FirstOrDefault() as ExternalDefinition;

            using (Transaction tx = new Transaction(doc, $"Add {parName} Parameter"))
            {
                if (tx.Start() == TransactionStatus.Started)
                {
                    doc.ParameterBindings.Insert(eDef, uiapp.Application.Create.NewInstanceBinding(cset), bipg);
                }
                tx.Commit();
            }
        }

        public static string GetKNXLPath(string fileDir, string pn)
        {
            string xlPath;

            if (File.Exists($"{fileDir}\\{pn} Keynotes.xlsx"))
            {
                return $"{fileDir}\\{pn} Keynotes.xlsx";
            }
            else if (File.Exists($"{fileDir}\\{pn} Keynotes.xlsm"))
            {
                return $"{fileDir}\\{pn} Keynotes.xlsm";
            }
            else
            {
                string spDir = Environment.GetFolderPath(Environment.SpecialFolder.UserProfile) + @"\Morrissey Engineering, Inc\All Morrissey - Documents\Keynotes\";
                xlPath = $"{spDir}{pn}.xlsx";
                //create new file or generate error if file does not exist
                if (!File.Exists(xlPath))
                {
                    try
                    {
                        File.Copy($"{spDir}Template.xlsx", xlPath);
                    }
                    catch
                    {
                        TaskDialog td = new TaskDialog("SharePoint Sync Required");
                        td.MainContent = @"All Morrissey Team SharePoint sync is required to use this feature.";
                        td.Show();
                        return String.Empty;
                    }

                    if (!File.Exists(fileDir + $"\\Keynotes.lnk"))
                    {
                        IWshRuntimeLibrary.WshShell shell = new IWshRuntimeLibrary.WshShell();
                        IWshRuntimeLibrary.IWshShortcut shortcut = (IWshRuntimeLibrary.IWshShortcut)shell.CreateShortcut(fileDir + $"\\Keynotes.lnk");
                        shortcut.TargetPath = xlPath;
                        shortcut.Save();
                    }
                }
            }
            return xlPath;
        }
    }
}
