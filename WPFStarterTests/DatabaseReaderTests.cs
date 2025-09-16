using Moq;
using WPFStarter.ImportAndExport.Export.Interfaces;
using WPFStarter.ImportAndExport.Export;
using WPFStarter.Model;
using WPFStarter.ProgramLogic.Interfaces;

namespace WPFStarterTests
{
        public class DatabaseReaderTests
        {
            [Fact]
            public async Task ReadDataInChunksAsync_ReturnsExpectedChunks_WhenConnectionIsValid()
            {
                // Arrange
                var exportStates = new Mock<IExportStates>();
                var messageBox = new Mock<IMessageBox>();
                var connection = new Mock<ITestConnection>();
                var streamChunks = new Mock<IStreamPersonChunks>();
                var fileReader = new Mock<IFileReader>();
                var connectionStringProvider = new Mock<IConnectionString>();

                var initialConnectionString = "Server=main;Database=main;";
                var fallbackConnectionString = "Server=fallback;Database=fallback;";
                var chunkSize = 2;

                var chunk1 = new List<Person>
        {
            new Person { Id = 1, FirstName = "Иван", LastName = "Иванов", SurName = "Иванович", City = "Минск", Country = "Беларусь", Date = DateTime.Today },
            new Person { Id = 2, FirstName = "Пётр", LastName = "Петров", SurName = "Петрович", City = "Москва", Country = "Россия", Date = DateTime.Today }
        };

                var chunk2 = new List<Person>
        {
            new Person { Id = 3, FirstName = "Сергей", LastName = "Сергеев", SurName = "Сергеевич", City = "Гомель", Country = "Беларусь", Date = DateTime.Today }
        };

                var expectedChunks = new List<List<Person>> { chunk1, chunk2 };

                
                connectionStringProvider
                    .Setup(p => p.GetConnectionString("MainConnection"))
                    .Returns(initialConnectionString);

                connection
                    .Setup(c => c.TestConnectionAsync(initialConnectionString))
                    .ReturnsAsync(true); 

                streamChunks
                    .Setup(s => s.StreamPersonChunksAsync(initialConnectionString, It.IsAny<string>(), chunkSize))
                    .Returns(MockAsyncEnumerable(expectedChunks));

                var reader = new DatabaseReader(
                    exportStates.Object,
                    messageBox.Object,
                    connection.Object,
                    streamChunks.Object,
                    fileReader.Object,
                    connectionStringProvider.Object);

                var resultChunks = new List<List<Person>>();

               
                await foreach (var chunk in reader.ReadDataInChunksAsync(chunkSize))
                {
                    resultChunks.Add(chunk);
                }

                
                Assert.Equal(2, resultChunks.Count);
                Assert.Equal(2, resultChunks[0].Count);
                Assert.Single(resultChunks[1]);

                exportStates.VerifySet(e => e.ExportRunning = true);
                exportStates.VerifySet(e => e.ExportRunning = false);
            }

            private async IAsyncEnumerable<List<Person>> MockAsyncEnumerable(List<List<Person>> chunks)
            {
                foreach (var chunk in chunks)
                {
                    yield return chunk;
                    await Task.Delay(1); 
                }
            }
        }
}
