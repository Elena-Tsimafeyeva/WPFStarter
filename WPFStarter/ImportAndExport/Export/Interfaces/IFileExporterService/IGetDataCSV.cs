
namespace WPFStarter.ImportAndExport.Export.Interfaces.IFileExporterService
{
    public interface IGetDataCSV
    {
        IAsyncEnumerable<string> GetDataCSVAsync(string? date, string? fromDate, string? toDate, string? firstName, string? lastName, string? surName, string? city, string? country, bool outDate, bool outFromDate, bool outToDate, bool outFirstName, bool outLastName, bool outSurName, bool outCity, bool outCountry);
    }
}
