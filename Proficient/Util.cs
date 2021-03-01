using System;
using System.Diagnostics;
using System.Runtime.InteropServices;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Autodesk.Revit.UI;
using Autodesk.Revit.DB;
using Autodesk.Revit.UI.Selection;

namespace Proficient
{
    class Util
    {
        public static void BalloonTip(string category, string title, string text)
        {
            Autodesk.Internal.InfoCenter.ResultItem ri = new Autodesk.Internal.InfoCenter.ResultItem();

            ri.Category = category;
            ri.Title = title;
            ri.TooltipText = text;

            Autodesk.Windows.ComponentManager.InfoCenterPaletteManager.ShowBalloon(ri);
        }

        [DllImport("user32.dll",SetLastError = true,CharSet = CharSet.Auto)]
        static extern int SetWindowText(IntPtr hWnd,string lpString);
        [DllImport("user32.dll", SetLastError = true)]
        static extern IntPtr FindWindowEx(IntPtr hwndParent, IntPtr hwndChildAfter, string lpszClass, string lpszWindow);
        public static void SetStatusText(string text)
        {

            IntPtr mainWindow = Process.GetCurrentProcess().MainWindowHandle;
            IntPtr statusBar = FindWindowEx(mainWindow, IntPtr.Zero, "msctls_statusbar32", "");

            if (statusBar != IntPtr.Zero)
            {
                SetWindowText(statusBar, text);
            }
        }
    }
}
