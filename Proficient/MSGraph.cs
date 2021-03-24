using System;
using System.Linq;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading.Tasks;
using System.Net.Http.Headers;
using Microsoft.Identity.Client;
using Microsoft.Identity.Client.Desktop;
using Microsoft.Graph;
using Autodesk.Revit.DB;

namespace Proficient
{
    class MSGraph
    {
        private const string ClientId = "426adfbc-cda1-4e53-9bae-aa82b37fc517";
        private const string AMGroupId = "38a7fa3e-0f52-4c33-b715-1340f32dad99";
        private const string KNFolderId = "01AAUGCRWA5BSGF2DQMBCL5V3BW7RSB7RG";
        private const string TemplateId = "01AAUGCRTZZR7LCABIIVA2HBKWWY4GF7BI";
        private static readonly ItemReference knFolderRef = new ItemReference
        {
            DriveId = "b!2pgXE-Qovk-1_2bf_HJCnVl303IJuEpEpfObtEUOEGL7uA0dLT_bT68uwGNQa5FO",
            Id = "01AAUGCRWA5BSGF2DQMBCL5V3BW7RSB7RG"
        };

        static string[] scopes = new string[] { "files.readwrite.all" };
        public static IPublicClientApplication PublicClientApp { get; set; }

        public static async void OpenKNFile(string pn)
        {
            GraphServiceClient graphClient = await SignInAndInitializeGraphServiceClient();

            var file = await GetKNFile(graphClient, pn);

            System.Diagnostics.Process.Start(file.WebUrl);
        }

        public static async Task<List<KeynoteEntry>> GetKNData(string pn)
        {
            List<KeynoteEntry> knList = new List<KeynoteEntry>();

            GraphServiceClient graphClient = await SignInAndInitializeGraphServiceClient();
            
            var curFile = await GetKNFile(graphClient, pn);

            var session = await graphClient.Groups[AMGroupId].Drive.Items[curFile.Id].Workbook
                .CreateSession(false)
                .Request()
                .PostAsync();

            var xlFile = await graphClient.Groups[AMGroupId].Drive.Items[curFile.Id].Workbook.Worksheets
                .Request()
                .Header("workbook-session-id", $"{session.Id}")
                .GetAsync();

            foreach (var ws in xlFile)
            {
                knList.Add(new KeynoteEntry(ws.Name, string.Empty, string.Empty));
                Console.WriteLine(ws.Name);
                WorkbookRange rng = await graphClient.Groups[AMGroupId].Drive.Items[curFile.Id].Workbook.Worksheets[ws.Id].Range().UsedRange()
                    .Request()
                    .Header("workbook-session-id", $"{session.Id}")
                    .GetAsync();
                string[][] rngarray = rng.Text.ToObject<string[][]>();
                foreach (string[] row in rngarray)
                {
                    if(row[0] != null && row[0] != String.Empty && row[1] != null && row[1] != String.Empty)
                        knList.Add(new KeynoteEntry(row[0], ws.Name, row[1]));
                }
            }

            await graphClient.Groups[AMGroupId].Drive.Items[curFile.Id].Workbook
                .CloseSession()
                .Request()
                .Header("workbook-session-id", $"{session.Id}")
                .PostAsync();            

            return knList;
        }

        private static async Task<DriveItem> GetKNFile(GraphServiceClient graphClient, string pn)
        {
            var files = await graphClient.Groups[AMGroupId].Drive.Items[KNFolderId].Children
                .Request()
                .GetAsync();

            if (files.Where(f => f.Name == $"{pn}.xlsx").Count() > 0)
            {
                return files.Where(f => f.Name == $"{pn}.xlsx").FirstOrDefault();
            }
            else
            {
                var name = $"{pn}.xlsx";

                await graphClient.Groups[AMGroupId].Drive.Items[TemplateId]
                            .Copy(name, knFolderRef)
                            .Request()
                            .PostAsync();

                do
                {
                    files = await graphClient.Groups[AMGroupId].Drive.Items[KNFolderId].Children
                        .Request()
                        .GetAsync();
                }
                while (files.Where(f => f.Name == $"{pn}.xlsx").Count() == 0);

                return files.Where(f => f.Name == $"{pn}.xlsx").FirstOrDefault();
            }
        }

        private async static Task<GraphServiceClient> SignInAndInitializeGraphServiceClient()
        {
            GraphServiceClient graphClient = new GraphServiceClient("https://graph.microsoft.com/v1.0/",
                new DelegateAuthenticationProvider(async (requestMessage) => {
                    requestMessage.Headers.Authorization = new AuthenticationHeaderValue("bearer", await SignInUserAndGetTokenUsingMSAL());
                }));

            return await Task.FromResult(graphClient);
        }

        private static async Task<string> SignInUserAndGetTokenUsingMSAL()
        {
            // Initialize the MSAL library by building a public client application
            PublicClientApp = PublicClientApplicationBuilder.Create(ClientId)
                .WithAuthority(@"https://login.microsoftonline.com/common")
                .WithDefaultRedirectUri()
                .WithExperimentalFeatures()
                .WithWindowsBroker(true)
                .Build();

            IAccount firstAccount = Microsoft.Identity.Client.PublicClientApplication.OperatingSystemAccount;

            AuthenticationResult authResult;
            try
            {
                authResult = await PublicClientApp.AcquireTokenSilent(scopes, firstAccount)
                                                  .ExecuteAsync();
            }
            catch (MsalUiRequiredException ex)
            {
                PublicClientApp = PublicClientApplicationBuilder.Create(ClientId)
                    .WithAuthority(@"https://login.microsoftonline.com/common")
                    .WithDefaultRedirectUri()
                    .Build();
                // A MsalUiRequiredException happened on AcquireTokenSilentAsync. This indicates you need to call AcquireTokenAsync to acquire a token
                Debug.WriteLine($"MsalUiRequiredException: {ex.Message}");

                authResult = await PublicClientApp.AcquireTokenInteractive(scopes)
                                                  .ExecuteAsync()
                                                  .ConfigureAwait(false);

            }
            return authResult.AccessToken;
        }
    }
}
