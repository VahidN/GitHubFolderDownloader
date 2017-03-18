using System.Linq;
using System.Net;
using System.Threading.Tasks;
using GitHubFolderDownloader.Models;
using GitHubFolderDownloader.Toolkit;
using Newtonsoft.Json;

namespace GitHubFolderDownloader.Core
{
    public class GitHubBranchList
    {
        private readonly GuiModel _guiModelData;

        public GitHubBranchList(GuiModel guiModelData)
        {
            _guiModelData = guiModelData;
        }

        public void SetBranchesList()
        {
            var taskScheduler = TaskScheduler.FromCurrentSynchronizationContext();
            Task.Factory.StartNew(() =>
            {
                if (!NetworkStatus.IsConnectedToInternet())
                {
                    AppMessenger.Messenger.NotifyColleagues("ShowLog", "The internet connection was not found.");
                    return null;
                }

                var entries = getGitHubBranches();
                if (!entries.Any())
                {
                    AppMessenger.Messenger.NotifyColleagues("ShowLog", "Failed to list branches.");
                    return null;
                }

                return entries;
            }).ContinueWith(task =>
            {
                if (task.IsFaulted)
                {
                    if (task.Exception != null)
                    {
                        task.Exception.Flatten().Handle(ex =>
                        {
                            AppMessenger.Messenger.NotifyColleagues("ShowLog", ex.Message);
                            return false;
                        });
                    }
                    return;
                }

                var entries = task.Result;
                _guiModelData.Branches = entries?.Select(x => x.Name).ToList();
            }, taskScheduler);
        }

        private GitHubBranch[] getGitHubBranches()
        {
            var url = new ApiUrl(_guiModelData).GetBranchesApiUrl();
            using (var webClient = new WebClient())
            {
                webClient.Headers.Add("user-agent", Downloader.UA);
                webClient.Headers.Add("Authorization", string.Format("Token {0}", _guiModelData.GitHubToken));
                var jsonData = webClient.DownloadString(url);
                return JsonConvert.DeserializeObject<GitHubBranch[]>(jsonData);
            }
        }
    }
}