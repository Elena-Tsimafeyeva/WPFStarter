
namespace WPFStarter.ProgramLogic.Interfaces
{
    ///<summary>
    /// E.A.T. 25-August-2025
    /// Interface for exporting data to a file.
    ///</summary>
    public interface IDataExporter
    {
        Task ExportAsync(string? fileName, string? fileType, string? date, string? fromDate, string? toDate, string? firstName, string? lastName, string? surName, string? city, string? country, bool outDate, bool outFromDate, bool outToDate, bool outFirstName, bool outLastName, bool outSurName, bool outCity, bool outCountry);
    }
}
