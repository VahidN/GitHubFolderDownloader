using System;
using System.Diagnostics;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Navigation;

namespace GitHubFolderDownloader.Behaviors
{
    public class WebHyperlink
    {
        #region Fields (1)

        public static readonly DependencyProperty LaunchDefaultBrowserProperty =
            DependencyProperty.RegisterAttached("LaunchDefaultBrowser",
            typeof(bool), typeof(WebHyperlink),
            new PropertyMetadata(false, hyperlinkUtilityLaunchDefaultBrowserChanged));

        #endregion Fields

        #region Methods (4)

        // Public Methods (2)

        public static bool GetLaunchDefaultBrowser(DependencyObject d)
        {
            return (bool)d.GetValue(LaunchDefaultBrowserProperty);
        }

        public static void SetLaunchDefaultBrowser(DependencyObject d, bool value)
        {
            d.SetValue(LaunchDefaultBrowserProperty, value);
        }
        // Private Methods (2)

        private static void hyperlinkRequestNavigateEvent(object sender, RequestNavigateEventArgs e)
        {
            if (e.Uri == null || string.IsNullOrWhiteSpace(e.Uri.AbsoluteUri))
                return;

            Process.Start(e.Uri.AbsoluteUri);
            e.Handled = true;
        }

        private static void hyperlinkUtilityLaunchDefaultBrowserChanged(DependencyObject d,
                                DependencyPropertyChangedEventArgs e)
        {
            var hl = d as Hyperlink;
            var tb = d as TextBlock;

            if (hl != null)
            {
                if ((bool)e.NewValue)
                    hl.AddHandler(Hyperlink.RequestNavigateEvent,
                       new RequestNavigateEventHandler(hyperlinkRequestNavigateEvent));
                else
                    hl.RemoveHandler(Hyperlink.RequestNavigateEvent,
                        new RequestNavigateEventHandler(hyperlinkRequestNavigateEvent));
            }
            else if (tb != null)
            {
                if ((bool)e.NewValue)
                    tb.AddHandler(Hyperlink.RequestNavigateEvent,
                       new RequestNavigateEventHandler(hyperlinkRequestNavigateEvent));
                else
                    tb.RemoveHandler(Hyperlink.RequestNavigateEvent,
                        new RequestNavigateEventHandler(hyperlinkRequestNavigateEvent));
            }
            else
            {
                throw new NotSupportedException("WebHyperlink should be applied to an hyperlink or a textblock.");
            }
        }

        #endregion Methods
    }
}