using Moq;
using WPFStarter.ImportAndExport.Import.Interfaces;
using WPFStarter.ImportAndExport.Import;
using WPFStarter.Model;

namespace WPFStarterTests
{
    public class CsvReaderTests
    {
        [Fact]
        public async Task ReadingDataAsync_ReturnsCorrectBatches()
        {
            var mockParser = new Mock<ICsvParser>();
            var people = new Queue<Person>(new[]
            {
                new Person { Id = 1, FirstName = "Иван", LastName = "Иванов", SurName = "Иванович",
                    City = "Гомель", Country = "Беларусь", Date = new DateTime(2024, 2, 1) },
                new Person { Id = 2, FirstName = "Пётр", LastName = "Петров", SurName = "Петрович",
                    City = "Москва", Country = "Россия", Date = new DateTime(2024, 2, 2) }});

            mockParser.Setup(p => p.EndOfData).Returns(() => people.Count == 0);
            mockParser.Setup(p => p.ReadFields()).Returns(() =>
            {
                var person = people.Dequeue();
                return new[]
                {
                    person.Date.ToString("yyyy-MM-dd"),
                    person.FirstName,
                    person.LastName,
                    person.SurName,
                    person.City,
                    person.Country
                };
            });

            var reader = new CsvReader(mockParser.Object);
            var result = new List<List<Person>>();

            await foreach (var batch in reader.ReadingDataAsync("ignored.csv", 2))
            {
                result.Add(batch);
            }

            Assert.Equal(1, result.Count);
            Assert.Equal("Иван", result[0][0].FirstName);
            Assert.Equal("Пётр", result[0][1].FirstName);
        }
    }
}
