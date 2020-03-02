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

namespace JR_Tools
{
    [Autodesk.Revit.Attributes.Transaction(Autodesk.Revit.Attributes.TransactionMode.Manual)]
    public class AddPanel : IExternalApplication
    {
        public Result OnStartup(UIControlledApplication application)
        {
            application.CreateRibbonTab("JR Tools");
            RibbonPanel basicribbonpanel = application.CreateRibbonPanel("JR Tools","General");
            basicribbonpanel.Title = "General";
            string thisAssemblyPath = Assembly.GetExecutingAssembly().Location;
            string imagelocation = Path.GetDirectoryName(typeof(AddPanel).Assembly.Location) + "\\images";

            PushButtonData button1data = new PushButtonData("cmdflip","Flip Element", thisAssemblyPath, "JR_Tools.FlipElements");
            PushButtonData button2data = new PushButtonData("cmdpipespace", "Space\nPipes", thisAssemblyPath, "JR_Tools.PipeSpacer");
            PushButtonData button3data = new PushButtonData("cmdreloadkn", "Reload\nKeynotes", thisAssemblyPath, "JR_Tools.KeynoteReload");
            PushButtonData button4data = new PushButtonData("cmdflipplane", "Flip Workplane", thisAssemblyPath, "JR_Tools.FlipWorkPlane");
            PushButtonData button5data = new PushButtonData("cmdchangecallout", "Change Callout\nReference", thisAssemblyPath, "JR_Tools.ChangeCalloutRef");
            PushButtonData button6data = new PushButtonData("cmdcombinetext", "Combine\nText", thisAssemblyPath, "JR_Tools.CombineText");
            PushButtonData button7data = new PushButtonData("cmdlaunchduct", "Launch\nDuctulator", thisAssemblyPath, "JR_Tools.DuctLauncher");

            button1data.Image = new BitmapImage(new Uri(imagelocation + "\\flipel.png"));
            button4data.Image = new BitmapImage(new Uri(imagelocation + "\\flipwp.png"));

            IList<RibbonItem> stackedGroup1 = basicribbonpanel.AddStackedItems(button1data, button4data);
            RibbonButton knbutton = basicribbonpanel.AddItem(button3data) as RibbonButton;
            RibbonButton pipebutton = basicribbonpanel.AddItem(button2data) as RibbonButton;
            RibbonButton calloutbutton = basicribbonpanel.AddItem(button5data) as RibbonButton;
            RibbonButton cmbtxtbutton = basicribbonpanel.AddItem(button6data) as RibbonButton;
            RibbonButton ductbutton = basicribbonpanel.AddItem(button7data) as RibbonButton;

            knbutton.LargeImage =  new BitmapImage(new Uri(imagelocation + "\\reload.png"));
            pipebutton.LargeImage = new BitmapImage(new Uri(imagelocation + "\\spacepipe.png"));
            calloutbutton.LargeImage = new BitmapImage(new Uri(imagelocation + "\\callout.png"));
            cmbtxtbutton.LargeImage = new BitmapImage(new Uri(imagelocation + "\\combine.png"));
            ductbutton.LargeImage = new BitmapImage(new Uri(imagelocation + "\\duct.png"));

            return Result.Succeeded;
        }

        public Result OnShutdown(UIControlledApplication application)
        {
            // nothing to clean up in this simple case
            return Result.Succeeded;
        }

        [System.Runtime.InteropServices.DllImport("gdi32.dll")]
        public static extern bool DeleteObject(IntPtr hObject);
    }


}
