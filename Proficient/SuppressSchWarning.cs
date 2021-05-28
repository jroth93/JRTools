using Autodesk.Revit.DB;
using Autodesk.Revit.UI;

namespace Proficient
{
    [Autodesk.Revit.Attributes.Transaction(Autodesk.Revit.Attributes.TransactionMode.Manual)]
    class SuppressSchWarning : IExternalCommand
    {
        public Result Execute(ExternalCommandData revit, ref string message, ElementSet elements)
        {

            if (Proficient.Instance.suppressSchWarningBtn.ItemText == "Schedule\nWarning On")
            {
                Proficient.Instance.suppressSchWarningBtn.ItemText = "Schedule\nWarning Off";
                Proficient.Instance.suppressSchWarningBtn.LargeImage = Proficient.NewBitmapImage("nomsg.png");
            }
            else
            {
                Proficient.Instance.suppressSchWarningBtn.ItemText = "Schedule\nWarning On";
                Proficient.Instance.suppressSchWarningBtn.LargeImage = Proficient.NewBitmapImage("msg.png");
            }

            return Result.Succeeded;
        }
    }
}
