using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Autodesk.Revit.DB;
using Autodesk.Revit.UI;


namespace JR_Tools
{
    [Autodesk.Revit.Attributes.Transaction(Autodesk.Revit.Attributes.TransactionMode.Manual)]
    public class WorksetSettings : IExternalCommand
    {
        public Result Execute(ExternalCommandData revit, ref string message, ElementSet elements)
        {
            Document doc = revit.Application.ActiveUIDocument.Document;
            WorksetTable wst = doc.GetWorksetTable();

            FilteredWorksetCollector wscol = new FilteredWorksetCollector(doc);
            WorksetSettingsForm form1 = new WorksetSettingsForm();
            form1.checkBox1.Checked = Properties.Settings.Default.switchenlarged;
            
            foreach (Workset ws in wscol)
            {
                if(ws.Kind == WorksetKind.UserWorkset)
                {
                    form1.defaultworkset.Items.Add(ws.Name);
                }

            }
            form1.defaultworkset.SelectedItem = Properties.Settings.Default.workset;

            form1.ShowDialog();

            if (!form1.iscancelled)
            {
                Properties.Settings.Default.workset = form1.defaultworkset.SelectedItem as String;
                Properties.Settings.Default.switchenlarged = form1.checkBox1.Checked;
            }

            form1.Close();

            return Result.Succeeded;
        }
                
    }
}
