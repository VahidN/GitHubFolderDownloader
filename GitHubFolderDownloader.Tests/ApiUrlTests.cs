using GitHubFolderDownloader.Core;
using GitHubFolderDownloader.ViewModels;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace GitHubFolderDownloader.Tests
{
    [TestClass]
    public class ApiUrlTests
    {
        [TestMethod]
        public void TestSetApiSegmentsWithUrlsContainingSpaces()
        {
            var viewModel = new MainWindowViewModel
            {
                GuiModelData =
                {
                    RepositoryFolderFullUrl =
                        "https://github.com/EdxStudents/W3C_HTML5.1x/tree/master/About%20W3C%20and%20the%20Web"
                }
            };

            Assert.AreEqual("EdxStudents", viewModel.GuiModelData.RepositoryOwner);
            Assert.AreEqual("W3C_HTML5.1x", viewModel.GuiModelData.RepositoryName);
            Assert.AreEqual("About W3C and the Web", viewModel.GuiModelData.RepositorySubDir);

            var actual = new ApiUrl(viewModel.GuiModelData).GetApiUrl(viewModel.GuiModelData.RepositorySubDir);
            Assert.AreEqual("https://api.github.com/repos/EdxStudents/W3C_HTML5.1x/contents/About%20W3C%20and%20the%20Web", actual);
        }

        [TestMethod]
        public void TestSetApiSegmentsWithNormalUrls()
        {
            var viewModel = new MainWindowViewModel
            {
                GuiModelData =
                {
                    RepositoryFolderFullUrl =
                        "https://github.com/VahidN/KendoUI-Samples/tree/master/KendoUI02_MVC/Controllers"
                }
            };

            Assert.AreEqual("VahidN", viewModel.GuiModelData.RepositoryOwner);
            Assert.AreEqual("KendoUI-Samples", viewModel.GuiModelData.RepositoryName);
            Assert.AreEqual("KendoUI02_MVC/Controllers", viewModel.GuiModelData.RepositorySubDir);

            var actual = new ApiUrl(viewModel.GuiModelData).GetApiUrl(viewModel.GuiModelData.RepositorySubDir);
            Assert.AreEqual("https://api.github.com/repos/VahidN/KendoUI-Samples/contents/KendoUI02_MVC/Controllers", actual);
        }
    }
}