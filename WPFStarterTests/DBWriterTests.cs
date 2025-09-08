using Moq;
using WPFStarter.ImportAndExport.Import;
using WPFStarter.ImportAndExport.Import.Interfaces;
using WPFStarter.Model;
using WPFStarter.ProgramLogic.Interfaces;

namespace WPFStarterTests
{
    public class DBWriterTests
    {
        [Fact]
        public async Task GetDataAsync_ReturnsCorrectBatches()
        {
            var people = new List<Person>
            {
                new Person { Id = 1, FirstName = "Иван", LastName = "Иванов", SurName = "Иванович",
                    City = "Гомель", Country = "Беларусь", Date = new DateTime(2024, 2, 1) },
                new Person { Id = 2, FirstName = "Пётр", LastName = "Петров", SurName = "Петрович",
                    City = "Москва", Country = "Россия", Date = new DateTime(2024, 2, 2) },
                new Person { Id = 3, FirstName = "Сергей", LastName = "Сергеев", SurName = "Сергеевич",
                    City = "Минск", Country = "Беларусь", Date = new DateTime(2024, 2, 3) }
            };
            var messageBox = new Mock<IMessageBox>();
            var importStates = new Mock<IImportStates>();
            var appContext = new Mock<IApplicationContext>();
            var result = new List<List<Person>>();
            var dbWriter = new DBWriter(messageBox.Object, appContext.Object, importStates.Object);
            await foreach (var batch in dbWriter.GetDataAsync(people, 2))
            {
                result.Add(batch);
            }

            Assert.Equal(2, result.Count);
            Assert.Equal("Иван", result[0][0].FirstName);
            Assert.Equal("Сергей", result[1][0].FirstName);
        }
        [Fact]
        public async Task RecordDBAsync_ConnectionIsAvailable_SavesDataAndUpdatesState()
        {
            var mockContext = new Mock<IApplicationContext>();
            var mockStates = new Mock<IImportStates>();
            var mockMessageBox = new Mock<IMessageBox>();

            mockContext.Setup(c => c.CanConnect()).Returns(true);

            var writer = new DBWriter(mockMessageBox.Object, mockContext.Object, mockStates.Object);
            var people = new List<Person>
            {
                new Person { Id = 1, FirstName = "Иван", LastName = "Иванов", SurName = "Иванович",
                    City = "Гомель", Country = "Беларусь", Date = new DateTime(2024, 2, 1) },
                new Person { Id = 2, FirstName = "Пётр", LastName = "Петров", SurName = "Петрович",
                    City = "Москва", Country = "Россия", Date = new DateTime(2024, 2, 2) },
                new Person { Id = 3, FirstName = "Сергей", LastName = "Сергеев", SurName = "Сергеевич",
                    City = "Минск", Country = "Беларусь", Date = new DateTime(2024, 2, 3) }
            };

            await writer.RecordDBAsync(people);

            mockStates.VerifySet(s => s.ImportRunning = true, Times.Once());
            mockContext.Verify(c => c.AddPeople(It.IsAny<IEnumerable<Person>>()), Times.AtLeastOnce);
            mockContext.Verify(c => c.SaveChangesAsync(), Times.AtLeastOnce);
            mockStates.VerifySet(s => s.ImportRunning = false, Times.Once());
            mockMessageBox.Verify(m => m.Show(It.IsAny<string>()), Times.Never);
        }
        [Fact]
        public async Task RecordDBAsync_WhenConnectionFails_ShowsErrorAndUpdatesState()
        {
            var mockContext = new Mock<IApplicationContext>();
            var mockStates = new Mock<IImportStates>();
            var mockMessageBox = new Mock<IMessageBox>();

            mockContext.Setup(c => c.CanConnect()).Returns(false);

            var writer = new DBWriter(mockMessageBox.Object, mockContext.Object, mockStates.Object);
            var records = new List<Person> { new Person() };

            await writer.RecordDBAsync(records);
           
            mockStates.VerifySet(s => s.ImportRunning = true, Times.Never);
            mockStates.VerifySet(s => s.StatusImport = false, Times.Once());
            mockStates.VerifySet(s => s.ImportRunning = false, Times.Once());
            mockStates.VerifySet(s => s.WindowDB = true, Times.Once());
            mockMessageBox.Verify(m => m.Show(It.Is<string>(msg => msg.Contains("Ошибка подключения"))), Times.Once);
        }
    }

}
