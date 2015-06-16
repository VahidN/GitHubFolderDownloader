using System.Net.NetworkInformation;

namespace GitHubFolderDownloader.Toolkit
{
    public static class NetworkStatus
    {
        public static bool IsConnectedToInternet(int timeoutPerHostMillis = 1000, string[] hostsToPing = null)
        {
            var networkAvailable = NetworkInterface.GetIsNetworkAvailable();
            if (!networkAvailable) return false;

            var hosts = hostsToPing ?? new[] { "www.google.com" };

            using (var ping = new Ping())
            {
                foreach (var host in hosts)
                {
                    try
                    {
                        var pingReply = ping.Send(host, timeoutPerHostMillis);
                        if (pingReply != null && pingReply.Status == IPStatus.Success)
                            return true;
                    }
                    catch { }
                }
            }

            return false;
        }
    }
}