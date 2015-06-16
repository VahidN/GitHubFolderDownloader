using System;
using System.ComponentModel;
using System.Linq;
using System.Text;
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
                    try
                    {
                        var uri = new Uri(GuiModelData.RepositoryFolderFullUrl);
                        GuiModelData.RepositoryOwner = uri.Segments[1];
                        GuiModelData.RepositoryName = uri.Segments[2];

                        var segments = new StringBuilder();
                        foreach (var segment in uri.Segments.Skip(5))
                        {
                            segments.Append(segment);
                        }
                        GuiModelData.RepositorySubDir = segments.ToString();
                    }
                    catch
                    {
                        /* doesn't matter */
                    }
                    break;
            }
        }
        private void manageAppExit()
        {
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