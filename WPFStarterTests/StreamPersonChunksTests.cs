using Moq;
using WPFStarter.ImportAndExport.Export.Interfaces;
using WPFStarter.ImportAndExport.Export;
using WPFStarter.Model;

namespace WPFStarterTests
{
    public class StreamPersonChunksTests
    {
        [Fact]
        public async Task StreamPersonChunksAsync_ReturnsExpectedChunks()
        {
            var connectionFactory = new Mock<ISqlConnection>();
            var connectionWrapper = new Mock<ISqlConnectionWrapper>();
            var commandWrapper = new Mock<ISqlCommandWrapper>();
            var readerWrapper = new Mock<ISqlDataReaderWrapper>();

            var chunkSize = 2;
            var connectionString = "Server=test;Database=test;";
            var query = "SELECT * FROM People";

            var peopleData = new List<Dictionary<string, object>>
            {
                new() { ["Id"] = 1, ["Date"] = DateTime.Today, ["FirstName"] = "Иван", ["LastName"] = "Иванов", ["SurName"] = "Иванович", ["City"] = "Минск", ["Country"] = "Беларусь" },
                new() { ["Id"] = 2, ["Date"] = DateTime.Today, ["FirstName"] = "Пётр", ["LastName"] = "Петров", ["SurName"] = "Петрович", ["City"] = "Москва", ["Country"] = "Россия" },
                new() { ["Id"] = 3, ["Date"] = DateTime.Today, ["FirstName"] = "Сергей", ["LastName"] = "Сергеев", ["SurName"] = "Сергеевич", ["City"] = "Гомель", ["Country"] = "Беларусь" }
            };

            var readIndex = -1;
            readerWrapper.Setup(r => r.ReadAsync()).ReturnsAsync(() =>
            {
                readIndex++;
                return readIndex < peopleData.Count;
            });

            readerWrapper.Setup(r => r[It.IsAny<string>()]).Returns((string key) => peopleData[readIndex][key]);
            commandWrapper.Setup(c => c.ExecuteReaderAsync()).ReturnsAsync(readerWrapper.Object);
            connectionWrapper.Setup(c => c.OpenAsync()).Returns(Task.CompletedTask);
            connectionWrapper.Setup(c => c.CreateCommand(query)).Returns(commandWrapper.Object);
            connectionFactory.Setup(f => f.Create(connectionString)).Returns(connectionWrapper.Object);

            var streamer = new StreamPersonChunks(connectionFactory.Object);
            var resultChunks = new List<List<Person>>();

            await foreach (var chunk in streamer.StreamPersonChunksAsync(connectionString, query, chunkSize))
            {
                resultChunks.Add(chunk);
            }

            Assert.Equal(2, resultChunks.Count);
            Assert.Equal(2, resultChunks[0].Count);
            Assert.Single(resultChunks[1]);
        }
    }
}
