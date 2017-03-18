using System.Collections.Generic;
using System.Net;
using System.Net.NetworkInformation;

namespace GitHubFolderDownloader.Toolkit
{
    public static class NetworkStatus
    {
        public static bool IsConnectedToInternet(int timeoutPerHostMillis = 1000, string[] hostsToPing = null)
        {
            var networkAvailable = NetworkInterface.GetIsNetworkAvailable();
            if (!networkAvailable) return false;

            var hosts = hostsToPing ?? new[] { "www.google.com", "http://www.google.com" };

            return canPing(timeoutPerHostMillis, hosts) || canOpenRead(hosts);
        }

        private static bool canPing(int timeoutPerHostMillis, IEnumerable<string> hosts)
        {
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
                    catch
                    {
                    }
                }
            }
            return false;
        }

        private static bool canOpenRead(IEnumerable<string> hosts)
        {
            foreach (var host in hosts)
            {
                try
                {
                    using (var webClient = new WebClient())
                    {
                        webClient.Headers.Add("user-agent", Downloader.UA);
                        using (var stream = webClient.OpenRead(host))
                        {
                            return true;
                        }
                    }
                }
                catch
                {
                }
            }
            return false;
        }
    }
}