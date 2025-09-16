using System.Diagnostics;
using WPFStarter.ImportAndExport.Export.Interfaces;
using WPFStarter.ImportAndExport.Export.Interfaces.IFileExporterService;
using WPFStarter.ProgramLogic.Interfaces;

namespace WPFStarter.ImportAndExport.Export
{
    public class FileAvailability: IFileAvailability
    {
        private readonly IExportStates _exportStates;
        private readonly IMessageBox _messageBox;
        private readonly IFileSystem _fileSystem;
        private readonly ISaveFileCSV _saveFileCSV;
        private readonly ISaveFileXML _saveFileXML;
        public FileAvailability(IExportStates exportStates, IMessageBox messageBox, IFileSystem fileSystem, ISaveFileCSV saveFileCSV, ISaveFileXML saveFileXML)
        {
            _exportStates = exportStates;
            _messageBox = messageBox;
            _fileSystem = fileSystem;
            _saveFileCSV = saveFileCSV;
            _saveFileXML = saveFileXML;
        }
        ///<summary>
        /// E.A.T. 10-February-2025
        /// Checking that such a file does not exist yet.
        ///</summary>
        public async Task FileAvailabilityAsync(string fileName, string? typeFile, string? date, string? fromDate, string? toDate, string? firstName, string? lastName, string? surName, string? city, string? country, bool outDate, bool outFromDate, bool outToDate, bool outFirstName, bool outLastName, bool outSurName, bool outCity, bool outCountry)
        {
            Debug.WriteLine("### Start of method FileAvailability ###");
            if (_fileSystem.FileExists(fileName))
            {
                _messageBox.Show($"Файл {fileName} уже есть");
                _exportStates.StatusExport = false;
            }
            else
            {
                if (typeFile == ".csv")
                {
                    await _saveFileCSV.SaveCSVAsync(fileName, date, fromDate, toDate, firstName, lastName, surName, city, country, outDate, outFromDate, outToDate, outFirstName, outLastName, outSurName, outCity, outCountry);
                    _messageBox.Show("Data saved .CSV");
                    _exportStates.StatusExport = false;
                }
                else if (typeFile == ".xml")
                {
                    await _saveFileXML.SaveXMLAsync(fileName, date, fromDate, toDate, firstName, lastName, surName, city, country, outDate, outFromDate, outToDate, outFirstName, outLastName, outSurName, outCity, outCountry);
                    _messageBox.Show("Data saved .XML");
                    _exportStates.StatusExport = false;
                }
            }
            Debug.WriteLine("### End of method FileAvailability ###");
        }
    }
}
