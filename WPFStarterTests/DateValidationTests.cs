using Moq;
using System.Globalization;
using WPFStarter.ProgramLogic;
using WPFStarter.ProgramLogic.Interfaces;

namespace WPFStarterTests
{
    public class DateValidationTests
    {
        [Fact]
        public void SortDate_InvalidDate_ShowsErrorMessage()
        {
            var mockMessageBox = new Mock<IMessageBox>();
            string date = "invalid-date";
            bool outDate;

            var dateValidations = new DateValidation(mockMessageBox.Object);
            dateValidations.SortDate(date, out outDate);

            mockMessageBox.Verify(m => m.Show($"Неправильный формат даты.\nДата должна иметь вид: Год-Месяц-День\nПример: 2024-02-01\nВы ввели: {date}"), Times.Once);
            Assert.False(outDate);
        }
        [Fact]
        public void SortDate_ValidDate()
        {
            var mockMessageBox = new Mock<IMessageBox>();
            string date = "01.02.2024";
            DateTime dateFormat = DateTime.ParseExact("2024-02-01", "yyyy-MM-dd", CultureInfo.InvariantCulture);
            bool outDate;

            var dateValidations = new DateValidation(mockMessageBox.Object);
            dateValidations.SortDate(date, out outDate);

            mockMessageBox.Verify(m => m.Show($"{dateFormat}"), Times.Once);
            Assert.True(outDate);
        }
        [Fact]
        public void CheckingDate_ValidData_outDateTrue()
        {
            var mockMessageBox = new Mock<IMessageBox>();

            string date = "01.02.2024";
            string? fromDate = null;
            string? toDate = null;

            var dateValidations = new DateValidation(mockMessageBox.Object);
            dateValidations.CheckingDate(date, fromDate, toDate, out bool outDate, out bool outFromDate, out bool outToDate);

            Assert.True(outDate);
            Assert.False(outFromDate);
            Assert.False(outToDate);
        }
        [Fact]
        public void CheckingDate_ValidData_outFromDateTrue_outToDateTrue()
        {
            var mockMessageBox = new Mock<IMessageBox>();

            string? date = null;
            string fromDate = "01.01.2024";
            string toDate = "01.02.2024";

            var dateValidations = new DateValidation(mockMessageBox.Object);
            dateValidations.CheckingDate(date, fromDate, toDate, out bool outDate, out bool outFromDate, out bool outToDate);

            Assert.False(outDate);
            Assert.True(outFromDate);
            Assert.True(outToDate);
        }
        [Fact]
        public void CheckingDate_FromDateAfterToDate_ShouldShowError()
        {
            var mockMessageBox = new Mock<IMessageBox>();
            var validator = new DateValidation(mockMessageBox.Object);

            string? date = null;
            string fromDate = "02.02.2024";
            string toDate = "01.01.2024";

            validator.CheckingDate(date, fromDate, toDate, out bool outDate, out bool outFromDate, out bool outToDate);

            Assert.False(outDate);
            Assert.False(outFromDate);
            Assert.False(outToDate);
            mockMessageBox.Verify(m => m.Show("Дата 'С' должна быть раньше даты 'По'"), Times.Once);
        }
        [Fact]
        public void CheckingDate_InvalidDate_OnlyDateOrOnlyFromDateToDate_ShowsErrorMessage()
        {
            var mockMessageBox = new Mock<IMessageBox>();

            string? date = "01.01.2024";
            string fromDate = "02.01.2024";
            string toDate = "01.02.2024";

            var dateValidations = new DateValidation(mockMessageBox.Object);
            dateValidations.CheckingDate(date, fromDate, toDate, out bool outDate, out bool outFromDate, out bool outToDate);

            Assert.False(outDate);
            Assert.False(outFromDate);
            Assert.False(outToDate);
            mockMessageBox.Verify(m => m.Show("Вы можете использовать даты для сортировки или 'ЗА Год-Месяц-День' или 'С Год-Месяц-День ПО Год-Месяц-День'."), Times.Once);
        }
        [Fact]
        public void CheckingDate_InvalidDate_FromDateOrToDateIsNullOrEmpty_ShowsErrorMessage()
        {
            var mockMessageBox = new Mock<IMessageBox>();

            string? date = null;
            string fromDate = "01.01.2024";
            string? toDate = null;

            var dateValidations = new DateValidation(mockMessageBox.Object);
            dateValidations.CheckingDate(date, fromDate, toDate, out bool outDate, out bool outFromDate, out bool outToDate);

            Assert.False(outDate);
            Assert.False(outFromDate);
            Assert.False(outToDate);
            mockMessageBox.Verify(m => m.Show("Чтобы использовать даты для сортировки 'С Год-Месяц-День ПО Год-Месяц-День',\n Вы должны заполнить оба поля."), Times.Once);
        }
    }
}
