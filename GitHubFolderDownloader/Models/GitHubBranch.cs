using Newtonsoft.Json;

namespace GitHubFolderDownloader.Models
{
    public class GitHubBranch
    {
        [JsonProperty(PropertyName = "name")]
        public string Name { set; get; }
    }
}