using Moq;
using WPFStarter.ImportAndExport.Export;
using WPFStarter.ImportAndExport.Export.Interfaces.IFileExporterService;

namespace WPFStarterTests
{
    public class FileExporterTests
    {
        [Fact]

        public async Task CreateFile_WhenFileNameAndFileTypeIsProvided()
        {
            var fileAvailability = new Mock<IFileAvailability>();

            string fileName = "test";
            string type = ".csv";

            var fileExporter = new FileExporter(fileAvailability.Object);
            await fileExporter.CreateFile(fileName, type, null, null, null, null, null, null, null, null, false, false, false, false, false, false, false, false);

            fileAvailability.Verify(x => x.FileAvailabilityAsync("test.csv", ".csv", null, null, null, null, null, null, null, null, false, false, false, false, false, false, false, false),Times.Once);
        }

        [Fact]

        public async Task CreateFile_WhenFileNameAndTypeFileAreEmpty()
        {
            var fileAvailability = new Mock<IFileAvailability>();

            var fileExporter = new FileExporter(fileAvailability.Object);
            await fileExporter.CreateFile(null, null, null, null, null, null, null, null, null, null, false, false, false, false, false, false, false, false);

            fileAvailability.Verify(x => x.FileAvailabilityAsync(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<bool>(), It.IsAny<bool>(), It.IsAny<bool>(), It.IsAny<bool>(), It.IsAny<bool>(), It.IsAny<bool>(), It.IsAny<bool>(), It.IsAny<bool>()),Times.Never);
        }
    }
}
