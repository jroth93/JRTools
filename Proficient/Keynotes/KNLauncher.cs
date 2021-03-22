using System;
using System.IO;
using Autodesk.Revit.DB;
using Autodesk.Revit.UI;
using System.Runtime.InteropServices;
using Excel = Microsoft.Office.Interop.Excel;

namespace Proficient
{
    [Autodesk.Revit.Attributes.Transaction(Autodesk.Revit.Attributes.TransactionMode.Manual)]
    class KNLauncher : IExternalCommand
    {
        public Result Execute(ExternalCommandData revit, ref string message, ElementSet elements)
        {
            string pn = Util.GetProjectNumber(revit);

            MSGraph.OpenKNFile(pn);

            return Result.Succeeded;
        }
    }
}
