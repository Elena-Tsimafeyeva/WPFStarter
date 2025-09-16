using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WPFStarter.ImportAndExport.Export.Interfaces.IFileExporterService
{
    public interface ISaveFileCSV
    {
        Task SaveCSVAsync(string filePath, string? date, string? fromDate, string? toDate, string? firstName, string? lastName, string? surName, string? city, string? country, bool outDate, bool outFromDate, bool outToDate, bool outFirstName, bool outLastName, bool outSurName, bool outCity, bool outCountry);
    }
}
