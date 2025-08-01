using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WPFStarter.ImportAndExport.Export;

namespace WPFStarter.ProgramLogic
{
    internal class DataExporter
    {
        public async Task ExportAsync(string? fileName, string? fileType, string? date, string? fromDate, string? toDate, string? firstName, string? lastName, string? surName, string? city, string? country, bool outDate, bool outFromDate, bool outToDate, bool outFirstName, bool outLastName, bool outSurName, bool outCity, bool outCountry)
        {
            await FileExporter.CreateFile(fileName, fileType, date, fromDate, toDate, firstName, lastName, surName, city, country, outDate, outFromDate, outToDate, outFirstName, outLastName, outSurName, outCity, outCountry);
        }
    }
}
