using System;
using System.ComponentModel;
using System.Windows;
using GitHubFolderDownloader.Core;
using GitHubFolderDownloader.Models;
using GitHubFolderDownloader.Toolkit;

namespace GitHubFolderDownloader.ViewModels
{
    public class MainWindowViewModel
    {
        private readonly GitHubDownloader _gitHubDownloader;
        private bool _isStarted;

        public MainWindowViewModel()
        {
            GuiModelData = new GuiModel
            {
                GitHubToken = ConfigSetGet.GetConfigData("GitHubToken")
            };
            GuiModelData.PropertyChanged += guiModelDataPropertyChanged;

            _gitHubDownloader = new GitHubDownloader(GuiModelData)
            {
                Finished = url =>
                {
                    addLog(string.Format("Finished {0}.", url));
                    _isStarted = false;
                }
            };

            StartCommand = new DelegateCommand<string>(doStart, canDoStart);
            StopCommand = new DelegateCommand<string>(doStop, stat => true);

            AppMessenger.Messenger.Register<string>("ShowLog", log => addLog(log));
            manageAppExit();
        }

        public GuiModel GuiModelData { set; get; }

        public DelegateCommand<string> StartCommand { set; get; }

        public DelegateCommand<string> StopCommand { set; get; }

        private void addLog(string log)
        {
            DispatcherHelper.DispatchAction(() =>
            {
                GuiModelData.Logs += string.Format("{0} {1}", DateTime.Now, log) + Environment.NewLine;
            });
        }

        private bool canDoStart(string data)
        {
            return !string.IsNullOrWhiteSpace(GuiModelData.RepositoryName) &&
                   !string.IsNullOrWhiteSpace(GuiModelData.RepositoryOwner) &&
                   !string.IsNullOrWhiteSpace(GuiModelData.OutputPath) &&
                   !_isStarted;
        }

        private void currentExit(object sender, ExitEventArgs e)
        {
            saveSettings();
            _gitHubDownloader.Stop();
        }

        private void currentSessionEnding(object sender, SessionEndingCancelEventArgs e)
        {
            saveSettings();
            _gitHubDownloader.Stop();
        }

        private void doStart(string data)
        {
            _isStarted = true;

            saveSettings();

            _gitHubDownloader.Start();
        }

        private void doStop(string data)
        {
            _gitHubDownloader.Stop();
            _isStarted = false;
        }

        private void guiModelDataPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            switch (e.PropertyName)
            {
                case "RepositoryFolderFullUrl":
                    new ApiUrl(GuiModelData).SetApiSegments();
                    new GitHubBranchList(GuiModelData).SetBranchesList();
                    break;
            }
        }

        private void manageAppExit()
        {
            if (Application.Current == null) return;
            Application.Current.Exit += currentExit;
            Application.Current.SessionEnding += currentSessionEnding;
        }

        private void saveSettings()
        {
            if (!string.IsNullOrWhiteSpace(GuiModelData.GitHubToken))
            {
                ConfigSetGet.SetConfigData("GitHubToken", GuiModelData.GitHubToken);
            }
        }
    }
}