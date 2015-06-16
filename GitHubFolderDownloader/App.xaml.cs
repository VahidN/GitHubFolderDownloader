using System;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media.Animation;
using System.Windows.Threading;
using GitHubFolderDownloader.Toolkit;

namespace GitHubFolderDownloader
{
    public partial class App
    {
        public App()
        {
            this.Startup += appStartup;
            AppDomain.CurrentDomain.UnhandledException += currentDomainUnhandledException;
            Current.DispatcherUnhandledException += appDispatcherUnhandledException;
            TaskScheduler.UnobservedTaskException += taskSchedulerUnobservedTaskException;
        }

        private static void appDispatcherUnhandledException(object sender, DispatcherUnhandledExceptionEventArgs e)
        {
            e.Handled = true;
            AppMessenger.Messenger.NotifyColleagues("ShowLog", e.Exception.Message);
        }

        static void appStartup(object sender, StartupEventArgs e)
        {
            reducingCpuConsumptionForAnimations();
        }

        private static void currentDomainUnhandledException(object sender, UnhandledExceptionEventArgs e)
        {
            var ex = (Exception)e.ExceptionObject;
            AppMessenger.Messenger.NotifyColleagues("ShowLog", ex.Message);

            if (e.IsTerminating)
            {
                MessageBox.Show(ex.ToString(), "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        static void reducingCpuConsumptionForAnimations()
        {
            Timeline.DesiredFrameRateProperty.OverrideMetadata(
                 typeof(Timeline),
                 new FrameworkPropertyMetadata { DefaultValue = 20 }
                 );
        }

        void taskSchedulerUnobservedTaskException(object sender, UnobservedTaskExceptionEventArgs e)
        {
            e.SetObserved();
            e.Exception.Flatten().Handle(ex =>
            {
                AppMessenger.Messenger.NotifyColleagues("ShowLog", ex.Message);
                return true;
            });
        }
    }
}