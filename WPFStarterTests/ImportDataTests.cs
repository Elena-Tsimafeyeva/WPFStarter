using Moq;
using WPFStarter.ImportAndExport.Import;
using WPFStarter.ImportAndExport.Import.Interfaces;
using WPFStarter.Model;
using WPFStarter.ProgramLogic.Interfaces;

namespace WPFStarterTests
{
    public class ImportDataTests
    {
        [Fact]
        public async Task ImportCsvAsync_ValidData_Successfully()
        {
            var messageBox = new Mock<IMessageBox>();
            var importStates = new Mock<IImportStates>();
            var dbWriter = new Mock<IDBWriter>();
            var csvReader = new Mock<ICsvReader>();

            var people = new List<Person>
            {
                new Person { Id = 1, FirstName = "Иван", LastName = "Иванов", SurName = "Иванович",
                    City = "Гомель", Country = "Беларусь", Date = new DateTime(2024, 2, 1) },
                new Person { Id = 2, FirstName = "Пётр", LastName = "Петров", SurName = "Петрович",
                    City = "Москва", Country = "Россия", Date = new DateTime(2024, 2, 2) },
                new Person { Id = 3, FirstName = "Сергей", LastName = "Сергеев", SurName = "Сергеевич",
                    City = "Минск", Country = "Беларусь", Date = new DateTime(2024, 2, 3) }
            };

            async IAsyncEnumerable<List<Person>> GetAsyncEnumerable()
            {
                yield return people;
                await Task.CompletedTask;
            }

            csvReader
                .Setup(r => r.ReadingDataAsync(It.IsAny<string>(), It.IsAny<int>()))
                .Returns(GetAsyncEnumerable());

            var importData = new ImportData(messageBox.Object,dbWriter.Object,csvReader.Object,importStates.Object);

            await importData.ImportCsvAsync("testpath.csv");

            dbWriter.Verify(w => w.RecordDBAsync(It.Is<List<Person>>(p => p.Count == 3)), Times.Once);
            messageBox.Verify(m => m.Show("Успешно!"), Times.Once);
            importStates.VerifySet(s => s.StatusImport = true);
            importStates.VerifySet(s => s.StatusImport = false);

        }
        [Fact]
        public async Task ImportCsvAsync_WhenDbWriterThrowsException_ShowsErrorMessage()
        {
            var messageBox = new Mock<IMessageBox>();
            var importStates = new Mock<IImportStates>();
            var dbWriter = new Mock<IDBWriter>();
            var csvReader = new Mock<ICsvReader>();

            var people = new List<Person>
            {
                new Person { Id = 1, FirstName = "Иван", LastName = "Иванов", SurName = "Иванович",
                    City = "Гомель", Country = "Беларусь", Date = new DateTime(2024, 2, 1) },
                new Person { Id = 2, FirstName = "Пётр", LastName = "Петров", SurName = "Петрович",
                    City = "Москва", Country = "Россия", Date = new DateTime(2024, 2, 2) },
                new Person { Id = 3, FirstName = "Сергей", LastName = "Сергеев", SurName = "Сергеевич",
                    City = "Минск", Country = "Беларусь", Date = new DateTime(2024, 2, 3) }
            };

            async IAsyncEnumerable<List<Person>> GetAsyncEnumerable()
            {
                yield return people;
                await Task.CompletedTask;
            }

            csvReader
                .Setup(r => r.ReadingDataAsync(It.IsAny<string>(), It.IsAny<int>()))
                .Returns(GetAsyncEnumerable());

            dbWriter
                .Setup(w => w.RecordDBAsync(It.IsAny<List<Person>>()))
                .ThrowsAsync(new Exception("Ошибка записи в базу"));

            var importData = new ImportData(messageBox.Object, dbWriter.Object, csvReader.Object, importStates.Object);

            await importData.ImportCsvAsync("testpath.csv");

            messageBox.Verify(m => m.Show(It.Is<string>(s => s.Contains("Ошибка импорта"))), Times.Once);
            importStates.VerifySet(s => s.StatusImport = true);
            importStates.VerifySet(s => s.StatusImport = false);
        }
    }
}
