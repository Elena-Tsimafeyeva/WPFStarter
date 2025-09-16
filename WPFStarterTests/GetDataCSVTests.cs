using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WPFStarter.ImportAndExport.Export.Interfaces;
using WPFStarter.ImportAndExport.Export;
using WPFStarter.Model;
using WPFStarter.ProgramLogic.Interfaces;

namespace WPFStarterTests
{
    public class GetDataCSVTests
    {
        [Fact]
        public async Task GetDataCSVAsync_ReturnsExpectedCsvLines()
        {
            var personRepository = new Mock<IPersonRepository>();
            var databaseReader = new Mock<IDatabaseReader>();

            var testChunk = new List<Person>
            {
                new Person { Id = 1, FirstName = "Иван", LastName = "Иванов", SurName = "Иванович",
                    City = "Гомель", Country = "Беларусь", Date = new DateTime(2025, 2, 1) },
                new Person { Id = 2, FirstName = "Пётр", LastName = "Петров", SurName = "Петрович",
                    City = "Москва", Country = "Россия", Date = new DateTime(2025, 2, 2) }};

            var filteredChunk = new List<Person> { testChunk[0] };

            databaseReader.Setup(r => r.ReadDataInChunksAsync(It.IsAny<int>())).Returns(GetAsyncEnumerable(testChunk));
            personRepository.Setup(r => r.FilterPeople(It.IsAny<List<Person>>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>(),It.IsAny<bool>(), It.IsAny<bool>(), It.IsAny<bool>(), It.IsAny<bool>(), It.IsAny<bool>(), It.IsAny<bool>(),It.IsAny<bool>(), It.IsAny<bool>())).Returns(filteredChunk);

            var service = new GetDataCSV(personRepository.Object, databaseReader.Object);
            var results = new List<string>();
            await foreach (var line in service.GetDataCSVAsync(null, null, null, null, null, null, null, null, false, false, false, false, false, false, false, false))
            {
                results.Add(line);
            }
            Assert.Single(results);
            Assert.Equal("1;2025-02-01;Иван;Иванов;Иванович;Гомель;Беларусь", results[0]);
        }

        private async IAsyncEnumerable<List<Person>> GetAsyncEnumerable(List<Person> chunk)
        {
            yield return chunk;
            await Task.CompletedTask;
        }
    }
}
