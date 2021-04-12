using Autodesk.Revit.DB;
using Autodesk.Revit.DB.ExternalService;
using Autodesk.Revit.UI;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Windows.Media.Imaging;

namespace Proficient
{
    [Autodesk.Revit.Attributes.Transaction(Autodesk.Revit.Attributes.TransactionMode.Manual)]
    public class AddPanel : IExternalApplication
    {
        static string _namespace_prefix = typeof(AddPanel).Namespace + ".";
        public Result OnStartup(UIControlledApplication application)
        {
            application.ControlledApplication.DocumentOpened
                += new EventHandler<Autodesk.Revit.DB.Events.DocumentOpenedEventArgs>(Application_DocumentOpened);
            application.ViewActivated += Application_ViewActivated;

            application.CreateRibbonTab("Proficient");
            RibbonPanel genrib = application.CreateRibbonPanel("Proficient", "General");
            RibbonPanel knrib = application.CreateRibbonPanel("Proficient", "Keynotes");
            RibbonPanel mechrib = application.CreateRibbonPanel("Proficient", "Mechanical");
            RibbonPanel elecrib = application.CreateRibbonPanel("Proficient", "Electrical");
            genrib.Title = "General";
            knrib.Title = "Keynotes";
            mechrib.Title = "Mechanical";
            elecrib.Title = "Electrical";

            Assembly thisAssembly = Assembly.GetExecutingAssembly();
            string thisAssemblyPath = thisAssembly.Location;
            string thisAssemblyVersion = thisAssembly.GetName().Version.ToString();

            #region create ribbon
            RibbonButton settingsbtn = genrib.AddItem(new PushButtonData("cmdstg", "Edit\nSettings", thisAssemblyPath, "Proficient.EditSettings")) as RibbonButton;
            SplitButton txtsplit = genrib.AddItem(new SplitButtonData("splttxttools", "Text Tools")) as SplitButton;
            RibbonButton calloutbutton = genrib.AddItem(new PushButtonData("cmdchangecallout", "Change Callout\nReference", thisAssemblyPath, "Proficient.ChangeCalloutRef")) as RibbonButton;
            RibbonButton elplcbutton = genrib.AddItem(new PushButtonData("cmdelplc", "Element\nPlacer", thisAssemblyPath, "Proficient.ElementPlacer")) as RibbonButton;
            IList<RibbonItem> stackedGroupFlip = genrib.AddStackedItems(new PushButtonData("cmdflip", "Flip Element", thisAssemblyPath, "Proficient.FlipElements"),
                                                                           new PushButtonData("cmdflipplane", "Flip Workplane", thisAssemblyPath, "Proficient.FlipWorkPlane"));
            RibbonButton xl2Revitbutton = genrib.AddItem(new PushButtonData("cmdexcelassign", "Excel Assigner\n(beta)", thisAssemblyPath, "Proficient.ExcelAssign")) as RibbonButton;

            RibbonButton knbutton = knrib.AddItem(new PushButtonData("cmdreloadkn", "Reload\nKeynotes", thisAssemblyPath, "Proficient.KNReload")) as RibbonButton;
            RibbonButton xlbutton = knrib.AddItem(new PushButtonData("cmdlaunchkn", "Open\nKeynotes", thisAssemblyPath, "Proficient.KNLauncher")) as RibbonButton;
            RibbonButton knutilbtn = knrib.AddItem(new PushButtonData("cmdknutil", "Keynote\nUtility", thisAssemblyPath, "Proficient.KeynoteUtil")) as RibbonButton;
            PulldownButton legacyknpulldown = knrib.AddItem(new PulldownButtonData("spltknlegacy", "Legacy\nKeynote\nTools")) as PulldownButton;

            RibbonButton pipebutton = mechrib.AddItem(new PushButtonData("cmdpipespace", "Space\nPipes", thisAssemblyPath, "Proficient.PipeSpacer")) as RibbonButton;
            RibbonButton ducttagbtn = mechrib.AddItem(new PushButtonData("cmdducttag", "Tag\nDucts", thisAssemblyPath, "Proficient.DuctTag")) as RibbonButton;
            RibbonButton ductbutton = mechrib.AddItem(new PushButtonData("cmdlaunchduct", "Launch\nDuctulator", thisAssemblyPath, "Proficient.DuctLauncher")) as RibbonButton;
            RibbonButton damperbtn = mechrib.AddItem(new PushButtonData("cmddampertoggle", "Damper\nToggle", thisAssemblyPath, "Proficient.DamperToggle")) as RibbonButton;
            RibbonButton elbowbtn = mechrib.AddItem(new PushButtonData("cmdductelbowtoggle", "Duct Elbow\nToggle", thisAssemblyPath, "Proficient.DuctElbowToggle")) as RibbonButton;

            RibbonButton elpanelbtn = elecrib.AddItem(new PushButtonData("cmdpanelcheck", "Panel\nChecker", thisAssemblyPath, "Proficient.PanelUtil")) as RibbonButton;

            PushButton cmbtxtbutton = txtsplit.AddPushButton(new PushButtonData("cmdcombinetext", "Combine\nText", thisAssemblyPath, "Proficient.CombineText"));
            PushButton txtldrbtn = txtsplit.AddPushButton(new PushButtonData("cmdtextleader", "Add Text\nWith Leader", thisAssemblyPath, "Proficient.TextLeader"));
            PushButton addldrbtn = txtsplit.AddPushButton(new PushButtonData("cmdaddleader", "Add\nLeader", thisAssemblyPath, "Proficient.AddLeader"));
            PushButton flattxtbtn = txtsplit.AddPushButton(new PushButtonData("cmdflattenText", "Flatten\nText", thisAssemblyPath, "Proficient.FlattenText"));


            PushButton legknopen = legacyknpulldown.AddPushButton(new PushButtonData("cmdlgknopen", "Open Keynotes", thisAssemblyPath, "Proficient.LegacyKNLauncher"));
            PushButton legknreload = legacyknpulldown.AddPushButton(new PushButtonData("cmdlegknrl", "Reload Keynotes", thisAssemblyPath, "Proficient.LegacyKNReload"));
            #endregion create ribbon

            //add images
            (stackedGroupFlip.ElementAt(0) as PushButton).Image = NewBitmapImage("flipel.png");
            (stackedGroupFlip.ElementAt(1) as PushButton).Image = NewBitmapImage("flipwp.png");
            knbutton.LargeImage = NewBitmapImage("reload.png");
            pipebutton.LargeImage = NewBitmapImage("spacepipe.png");
            calloutbutton.LargeImage = NewBitmapImage("callout.png");
            cmbtxtbutton.LargeImage = NewBitmapImage("combine.png");
            ductbutton.LargeImage = NewBitmapImage("duct.png");
            xlbutton.LargeImage = NewBitmapImage("knxl.png");
            settingsbtn.LargeImage = NewBitmapImage("wkst.png");
            elplcbutton.LargeImage = NewBitmapImage("elplc.png");
            txtldrbtn.LargeImage = NewBitmapImage("leadertext.png");
            addldrbtn.LargeImage = NewBitmapImage("addleader.png");
            flattxtbtn.LargeImage = NewBitmapImage("flattentext.png");
            ducttagbtn.LargeImage = NewBitmapImage("tagduct.png");
            elpanelbtn.LargeImage = NewBitmapImage("elecpanel.png");
            xl2Revitbutton.LargeImage = NewBitmapImage("xl2rvt.png");
            knutilbtn.LargeImage = NewBitmapImage("keynoteutil.png");
            damperbtn.LargeImage = NewBitmapImage("damper.png");
            elbowbtn.LargeImage = NewBitmapImage("ducttoggle.png");

            //add external resource servers for keynotes
            ExternalService externalResourceService = ExternalServiceRegistry.GetService(ExternalServices.BuiltInExternalServices.ExternalResourceService);
            externalResourceService.AddServer(new ExternalResourceDBServer());
            ExternalService externalResourceUIService = ExternalServiceRegistry.GetService(ExternalServices.BuiltInExternalServices.ExternalResourceUIService);
            externalResourceUIService.AddServer(new ExternalResourceUIServer());

            if (thisAssemblyVersion != FileVersionInfo.GetVersionInfo(@"Z:\Revit\Custom Add Ins\Proficient.bundle\Contents\Proficient.dll").FileVersion.ToString())
                Util.BalloonTip("Proficient", "New version of Proficient available.\nClick here, close Revit, and run the installer.", "Proficient Out Of Date", "Z:\\Revit\\Custom Add Ins");

            return Result.Succeeded;

        }

        BitmapImage NewBitmapImage(string imageName)
        {
            Stream s = Assembly.GetExecutingAssembly().GetManifestResourceStream(_namespace_prefix + "images." + imageName);
            BitmapImage img = new BitmapImage();

            img.BeginInit();
            img.StreamSource = s;
            img.EndInit();

            return img;
        }

        public Result OnShutdown(UIControlledApplication application)
        {
            // nothing to clean up in this simple case
            application.ControlledApplication.DocumentOpened -= Application_DocumentOpened;
            application.ViewActivated -= Application_ViewActivated;
            return Result.Succeeded;
        }

        [System.Runtime.InteropServices.DllImport("gdi32.dll")]
        public static extern bool DeleteObject(IntPtr hObject);

        private void Application_DocumentOpened(object sender, Autodesk.Revit.DB.Events.DocumentOpenedEventArgs args)
        {
            if (args.Document.IsWorkshared)
            {
                Document doc = args.Document;
                WorksetTable wst = doc.GetWorksetTable();

                FilteredWorksetCollector wscol = new FilteredWorksetCollector(doc);
                Workset workset = wscol.FirstOrDefault<Workset>(e => e.Name.Equals(Properties.Settings.Default.workset)) as Workset;

                Transaction transaction = new Transaction(doc, "Change Workset");
                if (transaction.Start() == TransactionStatus.Started)
                {
                    wst.SetActiveWorksetId(workset.Id);
                    transaction.Commit();
                }
            }
        }

        public static void Application_ViewActivated(object sender, EventArgs args)
        {
            UIApplication uiapp = sender as UIApplication;
            UIDocument uidoc = uiapp.ActiveUIDocument;
            Document doc = uidoc.Document;
            View view = doc.ActiveView;

            WorksetTable wst = doc.GetWorksetTable();
            FilteredWorksetCollector wscol = new FilteredWorksetCollector(doc);
            string viewname = view.Name;
            String viewsub = view.GetParameters("MEI Discipline-Sub").Count() > 0 ? view.GetParameters("MEI Discipline-Sub")[0].AsString() : "";

            if (viewname.ToLower().Contains("enlarged") || viewsub.ToLower().Contains("enlarged"))
            {
                if (doc.IsWorkshared && Properties.Settings.Default.switchenlarged)
                {
                    string enlWkst = Properties.Settings.Default.workset[0] == 'M' ? "M-Enlarged Plans" : "E-Enlarged Plans";
                    Workset enlWorkset = wscol.FirstOrDefault<Workset>(e => e.Name.Equals(enlWkst)) as Workset;

                    Transaction transaction = new Transaction(doc, "Change Workset");
                    if (transaction.Start() == TransactionStatus.Started)
                    {
                        wst.SetActiveWorksetId(enlWorkset.Id);
                        transaction.Commit();
                    }
                }
            }
            else if ((viewname.ToLower().Contains("site") || viewsub.ToLower().Contains("site")) && Properties.Settings.Default.workset[0] == 'E')
            {
                if (doc.IsWorkshared && Properties.Settings.Default.switchenlarged)
                {
                    string siteWkst = "E-Site";
                    Workset siteWorkset = wscol.FirstOrDefault<Workset>(e => e.Name.Equals(siteWkst)) as Workset;

                    Transaction transaction = new Transaction(doc, "Change Workset");
                    if (transaction.Start() == TransactionStatus.Started)
                    {
                        wst.SetActiveWorksetId(siteWorkset.Id);
                        transaction.Commit();
                    }
                }
            }
            else
            {
                Workset workset = wscol.FirstOrDefault<Workset>(e => e.Name.Equals(Properties.Settings.Default.workset)) as Workset;

                Transaction transaction = new Transaction(doc, "Change Workset");
                if (transaction.Start() == TransactionStatus.Started)
                {
                    wst.SetActiveWorksetId(workset.Id);
                    transaction.Commit();
                }
            }


        }
    }


}
