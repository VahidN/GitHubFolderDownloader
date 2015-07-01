using System;
using System.IO;
using System.Net;
using System.Threading;

namespace GitHubFolderDownloader.Toolkit
{
    public static class Downloader
    {
        public static readonly string UA = "GitHubFolderDownloader";

        public static void DownloadFile(
            string url,
            string outFolder,
            long expectedFileSize,
            Action<int> onPercentChange,
            CancellationToken cancellationToken)
        {
            if (string.IsNullOrWhiteSpace(url))
            {
                onPercentChange(100);
                return;
            }

            var segments = new Uri(url).Segments;
            var fileName = Uri.UnescapeDataString(segments[segments.Length - 1]);
            var filePath = Path.Combine(outFolder, fileName);

            if (File.Exists(filePath))
            {
                if (new FileInfo(filePath).Length == expectedFileSize)
                {
                    onPercentChange(100);
                    return;
                }
            }

            var request = (HttpWebRequest)WebRequest.Create(url);
            request.UserAgent = UA;

            request.AllowAutoRedirect = true;
            request.KeepAlive = false;
            request.Timeout = 15000;
            request.ReadWriteTimeout = 15000;
            request.Headers.Add("Authorization", string.Format("Token {0}", ConfigSetGet.GetConfigData("GitHubToken")));

            using (var webResponse = (HttpWebResponse)request.GetResponse())
            {
                using (var responseStream = webResponse.GetResponseStream())
                {
                    if (responseStream == null) return;
                    var urlFileSize = webResponse.ContentLength;

                    using (var fileStream = new FileStream(filePath, FileMode.Create, FileAccess.Write, FileShare.None))
                    {
                        var buffer = new byte[8192];
                        var bufLen = buffer.Length;
                        int readSize;
                        long downFileSize = 0;
                        while ((readSize = responseStream.Read(buffer, 0, bufLen)) > 0)
                        {
                            if (cancellationToken.IsCancellationRequested)
                            {
                                break;
                            }

                            fileStream.Write(buffer, 0, readSize);
                            fileStream.Flush();

                            downFileSize += readSize;

                            var percentDone = (int)(downFileSize * 100 / urlFileSize);
                            if (percentDone % 5 == 0 && percentDone > 0)
                            {
                                if (onPercentChange != null) onPercentChange(percentDone);
                            }
                        }
                    }
                }
            }
        }
    }
}