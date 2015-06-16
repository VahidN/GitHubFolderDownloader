using System;
using System.Windows;
using System.Windows.Threading;

namespace GitHubFolderDownloader.Toolkit
{
    public static class DispatcherHelper
    {
        public static void DispatchAction(Action action,
            DispatcherPriority dispatcherPriority = DispatcherPriority.Background)
        {
            var dispatcher = Application.Current != null ? Application.Current.Dispatcher : Dispatcher.CurrentDispatcher;

            if (action == null || dispatcher == null)
                return;

            dispatcher.Invoke(dispatcherPriority, action);
        }
    }
}