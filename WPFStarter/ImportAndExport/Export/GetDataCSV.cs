using System.Diagnostics;
using WPFStarter.ImportAndExport.Export.Interfaces;
using WPFStarter.ImportAndExport.Export.Interfaces.IFileExporterService;
using WPFStarter.ProgramLogic.Interfaces;

namespace WPFStarter.ImportAndExport.Export
{
    public class GetDataCSV:IGetDataCSV
    {
       
        private readonly IPersonRepository _personRepository;
        private readonly IDatabaseReader _databaseReader;
       
        public GetDataCSV(IPersonRepository personRepository, IDatabaseReader databaseReader) {
           
            _personRepository = personRepository;
            _databaseReader = databaseReader;
           
        }
        public async IAsyncEnumerable<string> GetDataCSVAsync(string? date, string? fromDate, string? toDate, string? firstName, string? lastName, string? surName, string? city, string? country, bool outDate, bool outFromDate, bool outToDate, bool outFirstName, bool outLastName, bool outSurName, bool outCity, bool outCountry)
        {
            Debug.WriteLine("### Start of method GetDataCSVAsync ###");
            await foreach (var chunk in _databaseReader.ReadDataInChunksAsync(3000))
            {
                var filtered = _personRepository.FilterPeople(chunk, date, fromDate, toDate, firstName, lastName, surName, city, country, outDate, outFromDate, outToDate, outFirstName, outLastName, outSurName, outCity, outCountry);
                Debug.WriteLine($"filtered size: {filtered.Count}");
                foreach (var person in filtered)
                {
                    yield return $"{person.Id};{person.Date:yyyy-MM-dd};{person.FirstName};{person.LastName};{person.SurName};{person.City};{person.Country}";
                }
            }
            Debug.WriteLine("### End of method GetDataCSVAsync ###");
        }
    }
}
