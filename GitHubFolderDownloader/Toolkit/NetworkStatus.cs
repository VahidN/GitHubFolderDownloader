using System.Net.NetworkInformation;

namespace GitHubFolderDownloader.Toolkit
{
    public static class NetworkStatus
    {
        public static bool IsConnectedToInternet(int timeoutPerHostMillis = 1000, string[] hostsToPing = null)
        {
            var networkAvailable = NetworkInterface.GetIsNetworkAvailable();
            if (!networkAvailable) return false;

            var hosts = hostsToPing ?? new[] { "http://www.google.com" };

            foreach (var host in hosts)
            {
                try
                {
                    using (var client = new System.Net.WebClient())
                    using (var stream = client.OpenRead(host))
                    {
                        return true;
                    }
                }
                catch (System.Exception ex)
                {
                    int g;
                }
            }

            return false;
        }
    }
}