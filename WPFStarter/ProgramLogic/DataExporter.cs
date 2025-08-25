using WPFStarter.ImportAndExport.Export;
using WPFStarter.ProgramLogic.Interfaces;

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
            await FileExporter.CreateFile(fileName, fileType, date, fromDate, toDate, firstName, lastName, surName, city, country, outDate, outFromDate, outToDate, outFirstName, outLastName, outSurName, outCity, outCountry);
        }
    }
}
