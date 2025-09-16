using System.Diagnostics;
using WPFStarter.ImportAndExport.Export.Interfaces;
using WPFStarter.ImportAndExport.Export.Interfaces.IFileExporterService;

namespace WPFStarter.ImportAndExport.Export
{
    public class SaveFileCSV:ISaveFileCSV
    {
        private readonly IExportStates _exportStates;
        private readonly IFileSystem _fileSystem;
        private readonly IGetDataCSV _getDataCSV;

        public SaveFileCSV(IExportStates exportStates, IFileSystem fileSystem, IGetDataCSV getDataCSV) {
            _exportStates = exportStates;
            _fileSystem = fileSystem;
            _getDataCSV = getDataCSV;
        }
        ///<summary>
        /// E.A.T. 12-February-2025
        /// Data export to .csv.
        ///</summary>
        public async Task SaveCSVAsync(string filePath, string? date, string? fromDate, string? toDate, string? firstName, string? lastName, string? surName, string? city, string? country, bool outDate, bool outFromDate, bool outToDate, bool outFirstName, bool outLastName, bool outSurName, bool outCity, bool outCountry)
        {
            Debug.WriteLine("### Start of method SaveCSVAsync ###");
            var content = _getDataCSV.GetDataCSVAsync(date, fromDate, toDate, firstName, lastName, surName, city, country, outDate, outFromDate, outToDate, outFirstName, outLastName, outSurName, outCity, outCountry);
            await _fileSystem.WriteTextAsync(filePath, content);
            _exportStates.StatusExport = false;
            Debug.WriteLine("### End of method SaveCSVAsync ###");
        }
    }
}
