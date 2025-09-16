using System.Windows;


namespace WPFStarter.ProgramLogic
{
    public class SortDataService
    {
        public bool StatusExport { get; private set; } = true;

        private readonly Interfaces.IMessageBox _messageBox;
        private readonly Interfaces.IInputValidator _validator;
        private readonly Interfaces.IDataExporter _exporter;
        public SortDataService(Interfaces.IMessageBox messageBox, Interfaces.IInputValidator validator, Interfaces.IDataExporter exporter)
        {
            _messageBox = messageBox;
            _validator = validator;
            _exporter = exporter;
        }
        public async Task SortDataAsync(string? date, string? fromDate, string? toDate, string? firstName, string? lastName, string? surName, string? city, string? country, string? fileType, string? fileName)
        {
            var result = _validator.Validate(date, fromDate, toDate, firstName, lastName, surName, city, country);

            if (!result.IsValid)
            {
                StatusExport = false;
                _messageBox.Show("Исправьте данные!");
                return;
            }


            
            var confirm = _messageBox.ShowConfirmation("Вы хотите перенести данные?", "Перенос данных");
            if (confirm != MessageBoxResult.Yes)
            {
                StatusExport = false;
                _messageBox.Show("Операция отменена.");
                return;
            }

            await _exporter.ExportAsync(fileName, fileType, date, fromDate, toDate, firstName, lastName, surName, city, country, result.OutDate, result.OutFromDate, result.OutToDate, result.OutFirstName, result.OutLastName, result.OutSurName, result.OutCity, result.OutCountry);
            StatusExport = false;
        }
        

    }


}
