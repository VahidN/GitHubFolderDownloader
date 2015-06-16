using System.Windows;
using System.Windows.Forms;
using System.Windows.Interactivity;

namespace GitHubFolderDownloader.Behaviors
{
    public class FolderBrowserDialogBehavior : TargetedTriggerAction<System.Windows.Controls.Button>
    {
        public static readonly DependencyProperty FolderBrowserDescriptionProperty =
           DependencyProperty.Register("FolderBrowserDescription", typeof(string),
           typeof(FolderBrowserDialogBehavior), null);

        public static readonly DependencyProperty FolderBrowserDialogResultCommandProperty =
            DependencyProperty.Register("FolderBrowserDialogResultCommand",
            typeof(object), typeof(FolderBrowserDialogBehavior), null);

        public string FolderBrowserDescription
        {
            get { return (string)GetValue(FolderBrowserDescriptionProperty); }
            set { SetValue(FolderBrowserDescriptionProperty, value); }
        }

        public object FolderBrowserDialogResultCommand
        {
            get { return GetValue(FolderBrowserDialogResultCommandProperty); }
            set { SetValue(FolderBrowserDialogResultCommandProperty, value); }
        }

        protected override void Invoke(object parameter)
        {
            using (var folderBrowserDialog = new FolderBrowserDialog { ShowNewFolderButton = true })
            {
                if (!string.IsNullOrEmpty(FolderBrowserDescription))
                {
                    folderBrowserDialog.Description = FolderBrowserDescription;
                }

                var result = folderBrowserDialog.ShowDialog();
                if (result == DialogResult.OK)
                    FolderBrowserDialogResultCommand = folderBrowserDialog.SelectedPath;
            }
        }
    }
}