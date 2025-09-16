using System.Diagnostics;
using System.Xml.Linq;
using WPFStarter.ImportAndExport.Export.Interfaces;
using WPFStarter.ImportAndExport.Export.Interfaces.IFileExporterService;
using WPFStarter.ProgramLogic.Interfaces;

namespace WPFStarter.ImportAndExport.Export
{
    public class SaveFileXML: ISaveFileXML
    {
        private readonly IExportStates _exportStates;
        private readonly IPersonRepository _personRepository;
        private readonly IDatabaseReader _databaseReader;
        private readonly IFileSystem _fileSystem;
        public SaveFileXML(IExportStates exportStates, IPersonRepository personRepository, IDatabaseReader databaseReader, IFileSystem fileSystem) {
            _exportStates = exportStates;
            _personRepository = personRepository;
            _databaseReader = databaseReader;
            _fileSystem = fileSystem;
        }
        ///<summary>
        /// E.A.T. 11-February-2025
        /// Data export to .xml.
        ///</summary>
        public async Task SaveXMLAsync(string filePath, string? date, string? fromDate, string? toDate, string? firstName, string? lastName, string? surName, string? city, string? country, bool outDate, bool outFromDate, bool outToDate, bool outFirstName, bool outLastName, bool outSurName, bool outCity, bool outCountry)
        {
            Debug.WriteLine("### Start of method SaveXMLAsync ###");
            string rootElementName = "TestProgram";
            XDocument xdoc = await _fileSystem.LoadOrCreateXmlAsync(filePath, rootElementName);
            XElement root = xdoc.Element(rootElementName)!;
            await foreach (var chunk in _databaseReader.ReadDataInChunksAsync(3000))
            {
                var filtered = _personRepository.FilterPeople(chunk, date, fromDate, toDate, firstName, lastName, surName, city, country, outDate, outFromDate, outToDate, outFirstName, outLastName, outSurName, outCity, outCountry);
                foreach (var person in filtered)
                {
                    XElement personElement = new XElement("Record",
                        new XAttribute("id", person.Id),
                        new XElement("Date", person.Date.ToString("yyyy-MM-dd")),
                        new XElement("FirstName", person.FirstName),
                        new XElement("LastName", person.LastName),
                        new XElement("SurName", person.SurName),
                        new XElement("City", person.City),
                        new XElement("Country", person.Country)
                    );
                    root.Add(personElement);
                }
            }
            await _fileSystem.SaveXmlAsync(filePath, xdoc);
            _exportStates.StatusExport = false;
            Debug.WriteLine("### End of method SaveXMLAsync ###");
        }
    }
}
