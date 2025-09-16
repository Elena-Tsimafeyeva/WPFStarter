using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using WPFStarter.ImportAndExport.Export.Interfaces;
using WPFStarter.ImportAndExport.Export;
using WPFStarter.Model;
using WPFStarter.ProgramLogic.Interfaces;

namespace WPFStarterTests
{
    public class SaveFileXMLTest
    {
        [Fact]
        public async Task SaveXMLAsync_CreatesAndSavesXmlWithFilteredData()
        {
            var states = new Mock<IExportStates>();
            var personRepository = new Mock<IPersonRepository>();
            var databaseReader = new Mock<IDatabaseReader>();
            var fileSystem = new Mock<IFileSystem>();

            var testChunk = new List<Person>
        {
            new Person { Id = 1, Date = new DateTime(2025, 2, 1), FirstName = "Иван", LastName = "Иванов", SurName = "Иванович", City = "Гомель", Country = "Беларусь" }
        };

            var filteredChunk = new List<Person> { testChunk[0] };
            var xdoc = new XDocument(new XElement("TestProgram"));

            fileSystem.Setup(fs => fs.LoadOrCreateXmlAsync(It.IsAny<string>(), "TestProgram")).ReturnsAsync(xdoc);
            databaseReader.Setup(dr => dr.ReadDataInChunksAsync(It.IsAny<int>())).Returns(GetAsyncEnumerable(testChunk));
            personRepository.Setup(pr => pr.FilterPeople(It.IsAny<List<Person>>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<bool>(), It.IsAny<bool>(), It.IsAny<bool>(),It.IsAny<bool>(), It.IsAny<bool>(), It.IsAny<bool>(), It.IsAny<bool>(), It.IsAny<bool>())).Returns(filteredChunk);

            var service = new SaveFileXML(states.Object, personRepository.Object, databaseReader.Object, fileSystem.Object);
            await service.SaveXMLAsync("test.xml", null, null, null, null, null, null, null, null, false, false, false, false, false, false, false, false);

            fileSystem.Verify(fs => fs.SaveXmlAsync("test.xml", xdoc), Times.Once);
            Assert.Equal(false, states.Object.StatusExport);
            Assert.Single(xdoc.Root.Elements("Record"));
            var record = xdoc.Root.Element("Record");
            Assert.Equal("1", record.Attribute("id")?.Value);
            Assert.Equal("2025-02-01", record.Element("Date")?.Value);
        }

        private async IAsyncEnumerable<List<Person>> GetAsyncEnumerable(List<Person> chunk)
        {
            yield return chunk;
            await Task.CompletedTask;
        }
    }
}
