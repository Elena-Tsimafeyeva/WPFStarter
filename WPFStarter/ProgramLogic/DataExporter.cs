using WPFStarter.ImportAndExport.Export;
using WPFStarter.ProgramLogic.Interfaces;
using WPFStarter.ProgramLogic.Services;

namespace WPFStarter.ProgramLogic
{
    ///<summary>
    /// E.A.T. 25-August-2025
    /// Implementation of the IDataExporter interface for asynchronously exporting data to a file.
    ///</summary>
    internal class DataExporter : IDataExporter
    {
        public async Task ExportAsync(string? fileName, string? fileType, string? date, string? fromDate, string? toDate, string? firstName, string? lastName, string? surName, string? city, string? country, bool outDate, bool outFromDate, bool outToDate, bool outFirstName, bool outLastName, bool outSurName, bool outCity, bool outCountry)
        {
            var exportStates = new ExportState();
            var messageBox = new MessageBoxService();
            var personRepository = new PersonRepository();
            var connectingFactory = new SqlConnectionFactory();
            var connecting = new TestConnection(messageBox, connectingFactory);
            var streamPersonChunks = new StreamPersonChunks(connectingFactory);
            var fileReader = new FileReader();
            var connectingString = new ConnectionString();
            var dbReader = new DatabaseReader(exportStates, messageBox, connecting, streamPersonChunks, fileReader, connectingString);
            var fileSystem = new FileSystem();
            var getDataCSV = new GetDataCSV(personRepository, dbReader);
            var saveFileCSV = new SaveFileCSV(exportStates, fileSystem, getDataCSV);
            var saveFileXML = new SaveFileXML(exportStates, personRepository, dbReader, fileSystem);
            var fileAvailability = new FileAvailability(exportStates, messageBox, fileSystem, saveFileCSV, saveFileXML);
            var fileExporter = new FileExporter(fileAvailability);
            await fileExporter.CreateFile(fileName, fileType, date, fromDate, toDate, firstName, lastName, surName, city, country, outDate, outFromDate, outToDate, outFirstName, outLastName, outSurName, outCity, outCountry);
        }
    }
}
