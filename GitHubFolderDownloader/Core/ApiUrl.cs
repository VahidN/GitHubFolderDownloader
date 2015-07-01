using System;
using System.Linq;
using System.Text;
using GitHubFolderDownloader.Models;

namespace GitHubFolderDownloader.Core
{
    public class ApiUrl
    {
        private readonly GuiModel _guiModelData;

        public ApiUrl(GuiModel guiModelData)
        {
            _guiModelData = guiModelData;
        }

        public string GetApiUrl(string repositorySubDir)
        {
            /*
            This API has an upper limit of 1,000 files for a directory.
            If you need to retrieve more files, use the Git Trees API.
            This API supports files up to 1 megabyte in size.
            */

            if (repositorySubDir == null)
            {
                repositorySubDir = string.Empty;
            }

            var url = string.Format("https://api.github.com/repos/{0}/{1}/contents/{2}",
                Uri.EscapeUriString(_guiModelData.RepositoryOwner),
                Uri.EscapeUriString(_guiModelData.RepositoryName),
                Uri.EscapeUriString(repositorySubDir));
            return url;
        }

        public void SetApiSegments()
        {
            try
            {
                var uri = new Uri(_guiModelData.RepositoryFolderFullUrl);
                _guiModelData.RepositoryOwner = Uri.UnescapeDataString(uri.Segments[1]);
                _guiModelData.RepositoryName = Uri.UnescapeDataString(uri.Segments[2]);

                var segments = new StringBuilder();
                foreach (var segment in uri.Segments.Skip(5))
                {
                    segments.Append(segment);
                }
                _guiModelData.RepositorySubDir = Uri.UnescapeDataString(segments.ToString());
            }
            catch
            {
                /* doesn't matter */
            }
        }

    }
}