using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Autodesk.Revit.UI;
using Autodesk.Revit.DB;
using Autodesk.Revit.ApplicationServices;
using Autodesk.Revit.UI.Selection;
using System.Diagnostics;


namespace Proficient
{
    [Autodesk.Revit.Attributes.Transaction(Autodesk.Revit.Attributes.TransactionMode.Manual)]
    public class DuctLauncher : IExternalCommand
    {
        public Result Execute(ExternalCommandData revit, ref string message, ElementSet elements)
        {
            var pathWithEnv = @"%USERPROFILE%\AppData\Roaming\Microsoft\Windows\Start Menu\Programs\Duct\Duct.appref-ms";
            var filePath = Environment.ExpandEnvironmentVariables(pathWithEnv);
            Process.Start(filePath);
            return Result.Succeeded;
        }
    }
}
