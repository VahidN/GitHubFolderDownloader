using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;

namespace GitHubFolderDownloader.Models
{
    public class GuiModel : INotifyPropertyChanged
    {
        private string _gitHubToken;
        private string _logs;
        private string _outputPath = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
        private double _progressBarValue;
        private string _repositoryFolderFullUrl;
        private string _repositoryName;
        private string _repositoryOwner;
        private string _repositorySubDir;
        private string _selectedBranch = string.Empty;
        private List<string> _branches = new List<string> { "master" };

        public GuiModel()
        {
            GitHubEntries = new ObservableCollection<GitHubEntry>();
        }

        public ObservableCollection<GitHubEntry> GitHubEntries { set; get; }

        public string GitHubToken
        {
            get { return _gitHubToken; }
            set
            {
                _gitHubToken = value;
                notifyPropertyChanged("GitHubToken");
            }
        }

        public string Logs
        {
            get { return _logs; }
            set
            {
                _logs = value;
                notifyPropertyChanged("Logs");
            }
        }

        public string OutputPath
        {
            get { return _outputPath; }
            set
            {
                _outputPath = value;
                notifyPropertyChanged("OutputPath");
            }
        }

        public double ProgressBarValue
        {
            get { return _progressBarValue; }
            set
            {
                _progressBarValue = value;
                notifyPropertyChanged("ProgressBarValue");
            }
        }

        public string RepositoryFolderFullUrl
        {
            get { return _repositoryFolderFullUrl; }
            set
            {
                _repositoryFolderFullUrl = value;
                notifyPropertyChanged("RepositoryFolderFullUrl");
            }
        }

        public string RepositoryName
        {
            get { return _repositoryName; }
            set
            {
                if (value == null) value = string.Empty;
                _repositoryName = value.Trim('/');
                notifyPropertyChanged("RepositoryName");
            }
        }

        public string RepositoryOwner
        {
            get { return _repositoryOwner; }
            set
            {
                if (value == null) value = string.Empty;
                _repositoryOwner = value.Trim('/');
                notifyPropertyChanged("RepositoryOwner");
            }
        }

        public string RepositorySubDir
        {
            get { return _repositorySubDir; }
            set
            {
                if (value == null) value = string.Empty;
                _repositorySubDir = value.Trim('/');
                notifyPropertyChanged("RepositorySubDir");
            }
        }

        public string SelectedBranch
        {
            get { return _selectedBranch; }
            set
            {
                if (value == null) value = string.Empty;
                _selectedBranch = value.Trim('/');
                notifyPropertyChanged("SelectedBranch");
            }
        }

        public List<string> Branches
        {
            get { return _branches; }
            set
            {
                _branches = value;
                notifyPropertyChanged("Branches");
            }
        }

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