using Moq;
using System.Windows;
using WPFStarter.ProgramLogic;
using WPFStarter.ProgramLogic.Interfaces;
namespace WPFStarterTests
{
    public class SortDataServiceTests
    {
        [Fact]
        public async Task SortDataAsync_ValidData_Confirmed_ExportsData()
        {
            var messageBox = new Mock<IMessageBox>();
            var validator = new Mock<IInputValidator>();
            var exporter = new Mock<IDataExporter>();

            var validationResult = new ValidationResult
            {
                IsValid = true,
                OutDate = true,
                OutFromDate = false,
                OutToDate = false,
                OutFirstName = true,
                OutLastName = true,
                OutSurName = true,
                OutCity = true,
                OutCountry = true
            };

            validator.Setup(v => v.Validate(
                It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>(),
                It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>(),
                It.IsAny<string>(), It.IsAny<string>())).Returns(validationResult);

            messageBox.Setup(m => m.ShowConfirmation(It.IsAny<string>(), It.IsAny<string>())).Returns(MessageBoxResult.Yes);

            var service = new SortDataService(messageBox.Object, validator.Object, exporter.Object);
            await service.SortDataAsync("01.01.2025", null, null, "Иван", "Петров", "Сергеевич", "Минск", "Беларусь", "csv", "output.csv");
            exporter.Verify(e => e.ExportAsync("output.csv", "csv", "01.01.2025", null, null, "Иван", "Петров", "Сергеевич", "Минск", "Беларусь", true, false, false, true, true, true, true, true), Times.Once);

            Assert.False(service.StatusExport);
        }

        [Fact]
        public async Task SortDataAsync_InvalidData_ShowsErrorMessage()
        {
            var messageBox = new Mock<IMessageBox>();
            var validator = new Mock<IInputValidator>();
            var exporter = new Mock<IDataExporter>();

            validator.Setup(v => v.Validate(It.IsAny<string>(), 
                It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>(), 
                It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>(), 
                It.IsAny<string>())).Returns(new ValidationResult { IsValid = false });

            var service = new SortDataService(messageBox.Object, validator.Object, exporter.Object);
            await service.SortDataAsync("2025-01-01", null, null, null, null, null, null, null, "csv", "output.csv");
            messageBox.Verify(m => m.Show("Исправьте данные!"), Times.Once);
            Assert.False(service.StatusExport);
        }
        [Fact]
        public async Task SortDataAsync_ValidData_UserDeclinesExport_ShowsCancelMessage()
        {
            var messageBox = new Mock<IMessageBox>();
            var validator = new Mock<IInputValidator>();
            var exporter = new Mock<IDataExporter>();

            var validationResult = new ValidationResult
            {
                IsValid = true,
                OutDate = true,
                OutFromDate = false,
                OutToDate = false,
                OutFirstName = true,
                OutLastName = true,
                OutSurName = true,
                OutCity = true,
                OutCountry = true
            };

            validator.Setup(v => v.Validate(
                It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>(),
                It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>(),
                It.IsAny<string>(), It.IsAny<string>()))
                .Returns(validationResult);

            messageBox.Setup(m => m.ShowConfirmation(It.IsAny<string>(), It.IsAny<string>())).Returns(MessageBoxResult.No);

            var service = new SortDataService(messageBox.Object, validator.Object, exporter.Object);
            await service.SortDataAsync("01.01.2025", null, null, "Иван", "Петров", "Сергеевич", "Минск", "Беларусь", "csv", "output.csv");
            messageBox.Verify(m => m.Show("Операция отменена."), Times.Once);
            exporter.Verify(e => e.ExportAsync(
                It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>(),
                It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>(),
                It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>(),
                It.IsAny<bool>(), It.IsAny<bool>(), It.IsAny<bool>(),
                It.IsAny<bool>(), It.IsAny<bool>(), It.IsAny<bool>(),
                It.IsAny<bool>(), It.IsAny<bool>()), Times.Never);

            Assert.False(service.StatusExport);
        }
    }
}