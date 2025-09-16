using Moq;
using WPFStarter.ImportAndExport.Export;
using WPFStarter.ImportAndExport.Export.Interfaces;
using WPFStarter.ImportAndExport.Export.Interfaces.IFileExporterService;
using WPFStarter.Model;

namespace WPFStarterTests
{
    public class SaveFileCSVTests
    {
        [Fact]
        public async Task SaveCSVAsync_WritesCSVAndSetsExportStates()
        {
            var states = new Mock<IExportStates>();
            var fileSystem = new Mock<IFileSystem>();
            var getDataCSV = new Mock<IGetDataCSV>();

            string filePath = "test.csv";

            var content = GetDataCSVContent();
            getDataCSV.Setup(g => g.GetDataCSVAsync(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<bool>(), It.IsAny<bool>(), It.IsAny<bool>(), It.IsAny<bool>(), It.IsAny<bool>(), It.IsAny<bool>(), It.IsAny<bool>(), It.IsAny<bool>())).Returns(content);
            fileSystem.Setup(fs => fs.WriteTextAsync(filePath, content)).Returns(Task.CompletedTask);

            var saveFileCSV = new SaveFileCSV(states.Object, fileSystem.Object, getDataCSV.Object);
            await saveFileCSV.SaveCSVAsync(filePath, null, null, null, null, null, null, null, null, false, false, false, false, false, false, false, false);

            getDataCSV.Verify(g => g.GetDataCSVAsync( null, null, null, null, null, null, null, null, false, false, false, false, false, false, false, false), Times.Once);
            fileSystem.Verify(fs => fs.WriteTextAsync(filePath, content), Times.Once);
            states.VerifySet(s => s.StatusExport = false);
        }

        private async IAsyncEnumerable<string> GetDataCSVContent()
        {
            yield return "1;2025-01-01;Пётр;Петровв;Петрович;Минск;Беларусь";
            await Task.CompletedTask;
        }
    }
}
