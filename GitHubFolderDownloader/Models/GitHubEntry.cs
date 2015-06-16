using System.ComponentModel;
using Newtonsoft.Json;

namespace GitHubFolderDownloader.Models
{
    public class GitHubEntry : INotifyPropertyChanged
    {
        private int _downloadPercent;

        public int DownloadPercent
        {
            set
            {
                if (_downloadPercent == value) return;
                _downloadPercent = value;
                notifyPropertyChanged("DownloadPercent");
            }
            get { return _downloadPercent; }
        }

        [JsonProperty(PropertyName = "download_url")]
        public string DownloadUrl { set; get; }

        [JsonProperty(PropertyName = "path")]
        public string Path { set; get; }

        [JsonProperty(PropertyName = "size")]
        public long Size { set; get; }

        [JsonProperty(PropertyName = "type")]
        public string Type { set; get; }

        #region INotifyPropertyChanged Members
        public event PropertyChangedEventHandler PropertyChanged;
        private void notifyPropertyChanged(string propertyName)
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
        }
        #endregion
    }
}