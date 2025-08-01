using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace WPFStarter.ProgramLogic
{
    internal class SortDataService
    {
        public bool StatusExport { get; private set; } = true;

        private readonly InputValidator validator = new();
        private readonly DataExporter exporter = new();

        public async Task SortDataAsync(string? date, string? fromDate, string? toDate, string? firstName, string? lastName, string? surName, string? city, string? country, string? fileType, string? fileName)
        {
            var result = validator.Validate(date, fromDate, toDate, firstName, lastName, surName, city, country);

            if (!result.isValid)
            {
                StatusExport = false;
                MessageBox.Show("Исправьте данные!");
                return;
            }

            var confirm = MessageBox.Show($"Вы хотите перенести данные?\nВаши данные:\nДата за {date}\nДата с {fromDate} по {toDate}\nГород {city}\nСтрана {country}\nФамилия {lastName}\nИмя {firstName}\nОтчество {surName}\nТип файла: {fileType}","Перенос данных", MessageBoxButton.YesNo, MessageBoxImage.Question);

            if (confirm != MessageBoxResult.Yes)
            {
                StatusExport = false;
                MessageBox.Show("Операция отменена.");
                return;
            }

            await exporter.ExportAsync(fileName, fileType, date, fromDate, toDate, firstName, lastName, surName, city, country, result.outDate, result.outFromDate, result.outToDate, result.outFirstName, result.outLastName, result.outSurName, result.outCity, result.outCountry);
        }
    }
}
