using Moq;
using WPFStarter.ImportAndExport.Export;
using WPFStarter.ImportAndExport.Export.Interfaces;
using WPFStarter.ImportAndExport.Export.Interfaces.IFileExporterService;
using WPFStarter.ProgramLogic.Interfaces;

namespace WPFStarterTests
{
    public class FileAvailabilityTests
    {
        [Fact]
        public async Task FileAvailabilityAsync_FileExists()
        {
            var states = new Mock<IExportStates>();
            var messageBox = new Mock<IMessageBox>();
            var fileSystem = new Mock<IFileSystem>();
            var saveFileCSV = new Mock<ISaveFileCSV>();
            var saveFileXML = new Mock<ISaveFileXML>();

            string fileName = "test";
            string type = ".csv";
            string fullFileName = fileName + type;

            fileSystem.Setup(fs => fs.FileExists(fullFileName)).Returns(true);

            var fileAvailability = new FileAvailability(states.Object, messageBox.Object, fileSystem.Object, saveFileCSV.Object, saveFileXML.Object);
            await fileAvailability.FileAvailabilityAsync(fullFileName, type, null, null, null, null, null, null, null, null, false, false, false, false, false, false, false, false);

            messageBox.Verify(mb => mb.Show($"Файл {fullFileName} уже есть"), Times.Once);
            states.VerifySet(es => es.StatusExport = false);
            saveFileCSV.Verify(c => c.SaveCSVAsync(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<bool>(), It.IsAny<bool>(), It.IsAny<bool>(), It.IsAny<bool>(), It.IsAny<bool>(), It.IsAny<bool>(), It.IsAny<bool>(), It.IsAny<bool>()), Times.Never);
            saveFileXML.Verify(x => x.SaveXMLAsync(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<bool>(), It.IsAny<bool>(), It.IsAny<bool>(), It.IsAny<bool>(), It.IsAny<bool>(), It.IsAny<bool>(), It.IsAny<bool>(), It.IsAny<bool>()), Times.Never);
        }

        [Fact]
        public async Task FileAvailabilityAsync_TypeFileCSV()
        {
            var states = new Mock<IExportStates>();
            var messageBox = new Mock<IMessageBox>();
            var fileSystem = new Mock<IFileSystem>();
            var saveFileCSV = new Mock<ISaveFileCSV>();
            var saveFileXML = new Mock<ISaveFileXML>();

            string fileName = "test";
            string type = ".csv";
            string fullFileName = fileName + type;

            fileSystem.Setup(fs => fs.FileExists(fullFileName)).Returns(false);

            var fileAvailability = new FileAvailability(states.Object, messageBox.Object, fileSystem.Object, saveFileCSV.Object, saveFileXML.Object);
            await fileAvailability.FileAvailabilityAsync(fullFileName, type, null, null, null, null, null, null, null, null, false, false, false, false, false, false, false, false);

            messageBox.Verify(mb => mb.Show("Data saved .CSV"), Times.Once);
            states.VerifySet(es => es.StatusExport = false);
            saveFileCSV.Verify(c => c.SaveCSVAsync(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<bool>(), It.IsAny<bool>(), It.IsAny<bool>(), It.IsAny<bool>(), It.IsAny<bool>(), It.IsAny<bool>(), It.IsAny<bool>(), It.IsAny<bool>()), Times.Once);
            saveFileXML.Verify(x => x.SaveXMLAsync(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<bool>(), It.IsAny<bool>(), It.IsAny<bool>(), It.IsAny<bool>(), It.IsAny<bool>(), It.IsAny<bool>(), It.IsAny<bool>(), It.IsAny<bool>()), Times.Never);
        }

        [Fact]
        public async Task FileAvailabilityAsync_TypeFileXML()
        {
            var states = new Mock<IExportStates>();
            var messageBox = new Mock<IMessageBox>();
            var fileSystem = new Mock<IFileSystem>();
            var saveFileCSV = new Mock<ISaveFileCSV>();
            var saveFileXML = new Mock<ISaveFileXML>();

            string fileName = "test";
            string type = ".xml";
            string fullFileName = fileName + type;

            fileSystem.Setup(fs => fs.FileExists(fullFileName)).Returns(false);

            var fileAvailability = new FileAvailability(states.Object, messageBox.Object, fileSystem.Object, saveFileCSV.Object, saveFileXML.Object);
            await fileAvailability.FileAvailabilityAsync(fullFileName, type, null, null, null, null, null, null, null, null, false, false, false, false, false, false, false, false);

            messageBox.Verify(mb => mb.Show("Data saved .XML"), Times.Once);
            states.VerifySet(es => es.StatusExport = false);
            saveFileCSV.Verify(c => c.SaveCSVAsync(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<bool>(), It.IsAny<bool>(), It.IsAny<bool>(), It.IsAny<bool>(), It.IsAny<bool>(), It.IsAny<bool>(), It.IsAny<bool>(), It.IsAny<bool>()), Times.Never);
            saveFileXML.Verify(x => x.SaveXMLAsync(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<bool>(), It.IsAny<bool>(), It.IsAny<bool>(), It.IsAny<bool>(), It.IsAny<bool>(), It.IsAny<bool>(), It.IsAny<bool>(), It.IsAny<bool>()), Times.Once);
        }
    }
}
