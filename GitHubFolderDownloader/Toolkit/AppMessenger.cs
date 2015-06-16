using System.Windows.Forms;

namespace GitHubFolderDownloader.Toolkit
{
    public class AppMessenger
    {
        readonly static Messenger _messenger = new Messenger();

        public static string ExecutablePathDir
        {
            get { return System.IO.Path.GetDirectoryName(Application.ExecutablePath); }
        }

        public static Messenger Messenger
        {
            get { return _messenger; }
        }

        public static string Path
        {
            get { return Application.StartupPath; }
        }
    }
}