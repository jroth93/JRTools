using Autodesk.Revit.DB;
using Autodesk.Revit.UI;
using System;
using System.Linq;
using System.Reflection;


namespace Proficient
{
    [Autodesk.Revit.Attributes.Transaction(Autodesk.Revit.Attributes.TransactionMode.Manual)]
    public class EditSettings : IExternalCommand
    {
        public Result Execute(ExternalCommandData revit, ref string message, ElementSet elements)
        {
            Document doc = revit.Application.ActiveUIDocument.Document;
            WorksetTable wst = doc.GetWorksetTable();
            
            FilteredWorksetCollector wscol = new FilteredWorksetCollector(doc);
            SettingsForm form1 = new SettingsForm();
            form1.checkBox1.Checked = Proficient.Settings.switchEnlarged;
            form1.pipespaceupdown.Value = Proficient.Settings.pipeDist;
            string assemblyVersion = Assembly.GetExecutingAssembly().GetName().Version.ToString();
            form1.lblVersion.Text = $"Proficient Version {assemblyVersion}";

            foreach (Workset ws in wscol)
            {
                if (ws.Kind == WorksetKind.UserWorkset)
                {
                    form1.defaultworkset.Items.Add(ws.Name);
                }

            }
            form1.defaultworkset.SelectedItem = Proficient.Settings.defWorkset;

            FilteredElementCollector coll = new FilteredElementCollector(doc);
            var txttypes = coll.WherePasses(new ElementClassFilter(typeof(TextNoteType))).Select(txt => txt.Name).ToArray();
            form1.defaulttext.Items.AddRange(txttypes);
            form1.defaulttext.SelectedItem = txttypes.Where(type => type == Proficient.Settings.defFont).FirstOrDefault();

            form1.ShowDialog();

            if (!form1.iscancelled)
            {
                try
                {
                    Proficient.Settings.defWorkset = form1.defaultworkset.SelectedItem as String;
                    Proficient.Settings.switchEnlarged = form1.checkBox1.Checked;
                    Proficient.Settings.pipeDist = Convert.ToInt32(form1.pipespaceupdown.Value);
                    Proficient.Settings.defFont = form1.defaulttext.SelectedItem.ToString();
                }
                catch
                {

                }
            }


            form1.Close();

            return Result.Succeeded;
        }

    }
}
