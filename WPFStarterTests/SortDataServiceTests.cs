using Moq;
using Xunit;
using System.Threading.Tasks;
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
            var mockMessageBox = new Mock<IMessageBox>();
            var mockValidator = new Mock<IInputValidator>();
            var mockExporter = new Mock<IDataExporter>();

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

            mockValidator.Setup(v => v.Validate(
                It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>(),
                It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>(),
                It.IsAny<string>(), It.IsAny<string>())).Returns(validationResult);

            mockMessageBox.Setup(m => m.ShowConfirmation(It.IsAny<string>(), It.IsAny<string>())).Returns(MessageBoxResult.Yes);

            var service = new SortDataService(mockMessageBox.Object, mockValidator.Object, mockExporter.Object);
            
            await service.SortDataAsync("01.01.2025", null, null, "Иван", "Петров", "Сергеевич", "Минск", "Беларусь", "csv", "output.csv");
            
            mockExporter.Verify(e => e.ExportAsync(
                "output.csv", "csv", "01.01.2025", null, null,
                "Иван", "Петров", "Сергеевич", "Минск", "Беларусь",
                true, false, false,
                true, true, true, true, true), Times.Once);

            Assert.True(service.StatusExport);
        }

        [Fact]
        public async Task SortDataAsync_InvalidData_ShowsErrorMessage()
        {
            var mockMessageBox = new Mock<IMessageBox>();
            var mockValidator = new Mock<IInputValidator>();
            var mockExporter = new Mock<IDataExporter>();

            mockValidator.Setup(v => v.Validate(It.IsAny<string>(), 
                It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>(), 
                It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>(), 
                It.IsAny<string>())).Returns(new ValidationResult { IsValid = false });

            var service = new SortDataService(mockMessageBox.Object, mockValidator.Object, mockExporter.Object);

            await service.SortDataAsync("2025-01-01", null, null, null, null, null, null, null, "csv", "output.csv");
           
            mockMessageBox.Verify(m => m.Show("Исправьте данные!"), Times.Once);
            Assert.False(service.StatusExport);
        }
        [Fact]
        public async Task SortDataAsync_ValidData_UserDeclinesExport_ShowsCancelMessage()
        {
            var mockMessageBox = new Mock<IMessageBox>();
            var mockValidator = new Mock<IInputValidator>();
            var mockExporter = new Mock<IDataExporter>();

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

            mockValidator.Setup(v => v.Validate(
                It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>(),
                It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>(),
                It.IsAny<string>(), It.IsAny<string>()))
                .Returns(validationResult);

            mockMessageBox.Setup(m => m.ShowConfirmation(It.IsAny<string>(), It.IsAny<string>())).Returns(MessageBoxResult.No);

            var service = new SortDataService(mockMessageBox.Object, mockValidator.Object, mockExporter.Object);

            await service.SortDataAsync("01.01.2025", null, null, "Иван", "Петров", "Сергеевич", "Минск", "Беларусь", "csv", "output.csv");

            mockMessageBox.Verify(m => m.Show("Операция отменена."), Times.Once);
            mockExporter.Verify(e => e.ExportAsync(
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