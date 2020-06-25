using System;
using System.Reflection;
using System.IO;
using Autodesk.Revit.DB;
using Autodesk.Revit.UI;
using System.Windows;
using System.Windows.Media.Imaging;
using System.Linq;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Interop;

[assembly: System.Reflection.AssemblyVersion("1.0.0.1")]

namespace JR_Tools
{
    [Autodesk.Revit.Attributes.Transaction(Autodesk.Revit.Attributes.TransactionMode.Manual)]
    public class AddPanel : IExternalApplication
    {
        public Result OnStartup(UIControlledApplication application)
        {
            application.ControlledApplication.DocumentOpened
                += new EventHandler<Autodesk.Revit.DB.Events.DocumentOpenedEventArgs>(Application_DocumentOpened);
            application.ViewActivated += Application_ViewActivated;

            application.CreateRibbonTab("JR Tools");
            RibbonPanel genrib = application.CreateRibbonPanel("JR Tools","General");
            RibbonPanel knrib = application.CreateRibbonPanel("JR Tools", "Keynotes");
            RibbonPanel mechrib = application.CreateRibbonPanel("JR Tools", "Mechanical");
            genrib.Title = "General";
            knrib.Title = "Keynotes";
            mechrib.Title = "Mechanical";

            string thisAssemblyPath = Assembly.GetExecutingAssembly().Location;
            string imagelocation = Directory.GetParent(Directory.GetParent(Path.GetDirectoryName(typeof(AddPanel).Assembly.Location)).FullName).FullName + "\\images";
            
            PushButtonData button1data = new PushButtonData("cmdflip","Flip Element", thisAssemblyPath, "JR_Tools.FlipElements");
            PushButtonData button2data = new PushButtonData("cmdpipespace", "Space\nPipes", thisAssemblyPath, "JR_Tools.PipeSpacer");
            PushButtonData button3data = new PushButtonData("cmdreloadkn", "Reload\nKeynotes", thisAssemblyPath, "JR_Tools.KeynoteReload");
            PushButtonData button4data = new PushButtonData("cmdflipplane", "Flip Workplane", thisAssemblyPath, "JR_Tools.FlipWorkPlane");
            PushButtonData button5data = new PushButtonData("cmdchangecallout", "Change Callout\nReference", thisAssemblyPath, "JR_Tools.ChangeCalloutRef");
            PushButtonData button6data = new PushButtonData("cmdcombinetext", "Combine\nText", thisAssemblyPath, "JR_Tools.CombineText");
            PushButtonData button7data = new PushButtonData("cmdlaunchduct", "Launch\nDuctulator", thisAssemblyPath, "JR_Tools.DuctLauncher");
            PushButtonData button8data = new PushButtonData("cmdlaunchkn", "Open\nKeynotes", thisAssemblyPath, "JR_Tools.KNXLLauncher");
            PushButtonData button9data = new PushButtonData("cmdwkststg", "Workset\nSettings", thisAssemblyPath, "JR_Tools.WorksetSettings");

            button1data.Image = new BitmapImage(new Uri(imagelocation + "\\flipel.png"));
            button4data.Image = new BitmapImage(new Uri(imagelocation + "\\flipwp.png"));

            IList<RibbonItem> stackedGroup1 = genrib.AddStackedItems(button1data, button4data);
            RibbonButton wkstbutton = genrib.AddItem(button9data) as RibbonButton;
            RibbonButton calloutbutton = genrib.AddItem(button5data) as RibbonButton;
            RibbonButton cmbtxtbutton = genrib.AddItem(button6data) as RibbonButton;
            RibbonButton knbutton = knrib.AddItem(button3data) as RibbonButton;
            RibbonButton xlbutton = knrib.AddItem(button8data) as RibbonButton;
            RibbonButton pipebutton = mechrib.AddItem(button2data) as RibbonButton;
            RibbonButton ductbutton = mechrib.AddItem(button7data) as RibbonButton;

            knbutton.LargeImage =  new BitmapImage(new Uri(imagelocation + "\\reload.png"));
            pipebutton.LargeImage = new BitmapImage(new Uri(imagelocation + "\\spacepipe.png"));
            calloutbutton.LargeImage = new BitmapImage(new Uri(imagelocation + "\\callout.png"));
            cmbtxtbutton.LargeImage = new BitmapImage(new Uri(imagelocation + "\\combine.png"));
            ductbutton.LargeImage = new BitmapImage(new Uri(imagelocation + "\\duct.png"));
            xlbutton.LargeImage = new BitmapImage(new Uri(imagelocation + "\\knxl.png"));
            wkstbutton.LargeImage = new BitmapImage(new Uri(imagelocation + "\\wkst.png"));

            return Result.Succeeded;
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
            String viewname = view.Name;

            if (viewname.ToLower().Contains("enlarged") && Properties.Settings.Default.switchenlarged)
            {
                if (doc.IsWorkshared)
                {
                    string enlwkst = Properties.Settings.Default.workset[0] == 'M' ? "M-Enlarged Plans" : "E-Enlarged Plans";
                    Workset enlworkset = wscol.FirstOrDefault<Workset>(e => e.Name.Equals(enlwkst)) as Workset;

                    Transaction transaction = new Transaction(doc, "Change Workset");
                    if (transaction.Start() == TransactionStatus.Started)
                    {
                        wst.SetActiveWorksetId(enlworkset.Id);
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
