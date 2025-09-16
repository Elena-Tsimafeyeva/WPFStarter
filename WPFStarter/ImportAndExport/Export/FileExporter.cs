using System.Diagnostics;
using WPFStarter.ImportAndExport.Export.Interfaces.IFileExporterService;

namespace WPFStarter.ImportAndExport.Export
{
    public class FileExporter
    {
        private readonly IFileAvailability _fileAvailability;


        public FileExporter(IFileAvailability fileAvailability)
        {
            _fileAvailability = fileAvailability;
        }
        ///<summary>
        /// E.A.T. 10-February-2025
        /// Checking the entered word.
        ///</summary>
        public async Task CreateFile(string? fileName, string? typeFile, string? date, string? fromDate, string? toDate, string? firstName, string? lastName, string? surName, string? city, string? country, bool outDate, bool outFromDate, bool outToDate, bool outFirstName, bool outLastName, bool outSurName, bool outCity, bool outCountry)
        {
            Debug.WriteLine("### Start of method CreateFile ###");
            string? fullFileName = null;
            if (!string.IsNullOrEmpty(fileName) || !string.IsNullOrEmpty(typeFile))
            {
                fullFileName = $"{fileName}{typeFile}";
                await _fileAvailability.FileAvailabilityAsync(fullFileName, typeFile, date, fromDate, toDate, firstName, lastName, surName, city, country, outDate, outFromDate, outToDate, outFirstName, outLastName, outSurName, outCity, outCountry);
            }
            Debug.WriteLine("### End of method CreateFile ###");
        }
    }
}
