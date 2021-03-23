﻿using System;
using System.Collections.Generic;
using Autodesk.Revit.DB;
using Autodesk.Revit.UI;
using Autodesk.Revit.DB.ExternalService;
using System.Windows.Forms;

namespace Proficient
{
    [Autodesk.Revit.Attributes.Transaction(Autodesk.Revit.Attributes.TransactionMode.Manual)]
    class KNReload : IExternalCommand
    {
        private static List<KeynoteEntry> knList;
        private static RevitTask rvtTask = new RevitTask();
        public static Guid dbID;
        public Result Execute(ExternalCommandData revit, ref string message, ElementSet elements)
        {
            AsyncGetKN(revit);
            return Result.Succeeded;
        }

        private static async void AsyncGetKN(ExternalCommandData revit)
        {
            Document doc = revit.Application.ActiveUIDocument.Document;

            knList =  await MSGraph.GetKNData(Util.GetProjectNumber(revit));

            rvtTask.Run((uiApp) => RvtKNWork(uiApp.ActiveUIDocument.Document));
        }

        private static void RvtKNWork(Document doc)
        {
            ExternalService externalResourceService = ExternalServiceRegistry.GetService(ExternalServices.BuiltInExternalServices.ExternalResourceService);
            ExternalResourceDBServer knSrv = externalResourceService.GetServer(dbID) as ExternalResourceDBServer;
            knSrv.knList = knList;

            using (Transaction tx = new Transaction(doc, "Reload Keynotes"))
            {
                if (tx.Start() == TransactionStatus.Started)
                {
                    ModelPath p = ModelPathUtils.ConvertUserVisiblePathToModelPath("KNServer://Keynotes.txt");
                    ExternalResourceReference s = ExternalResourceReference.CreateLocalResource(doc, ExternalResourceTypes.BuiltInExternalResourceTypes.KeynoteTable, p, PathType.Absolute);
                    KeynoteTable.GetKeynoteTable(doc).LoadFrom(s, null);
                }
                tx.Commit();
            }

            Util.BalloonTip("Keynotes", "Keynotes Reloaded!", string.Empty);
        }
    }
}
