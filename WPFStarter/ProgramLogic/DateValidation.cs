using System.Diagnostics;
using System.Globalization;

namespace WPFStarter.ProgramLogic
{
    public class DateValidation
    {
        private readonly Interfaces.IMessageBox _messageBox;
        
        public DateValidation(Interfaces.IMessageBox messageBox)
        {
            _messageBox = messageBox;
        }
        ///<summary>
        /// E.A.T. 4-February-2025
        /// Checking date format.
        ///</summary>
        public void SortDate(string? date, out bool outDate)
        {
            Debug.WriteLine("### Start of method SortDate ###");
            outDate = true;
            if (!string.IsNullOrEmpty(date))
            {
                try
                {
                    DateTime parsedDate = DateTime.ParseExact(date, "dd.MM.yyyy", CultureInfo.InvariantCulture);
                    string stringParsedDate = parsedDate.ToString("yyyy-MM-dd");
                    DateTime dateFormat = DateTime.ParseExact(stringParsedDate, "yyyy-MM-dd", CultureInfo.InvariantCulture);
                    _messageBox.Show($"{dateFormat}");
                }
                catch (FormatException)
                {
                    _messageBox.Show($"Неправильный формат даты.\nДата должна иметь вид: Год-Месяц-День\nПример: 2024-02-01\nВы ввели: {date}");
                    outDate = false;
                }
            }
            Debug.WriteLine("### End of method SortDate ###");
        }

        ///<summary>
        /// E.A.T. 5-February-2025
        /// Checking date.
        ///</summary>
        public void CheckingDate(string? date, string? fromDate, string? toDate, out bool outDate, out bool outFromDate, out bool outToDate)
        {
            Debug.WriteLine("### Start of method CheckingDate ###");
            outDate = false;
            outFromDate = false;
            outToDate = false;

            if (!string.IsNullOrEmpty(date) && !string.IsNullOrEmpty(fromDate) || !string.IsNullOrEmpty(date) && !string.IsNullOrEmpty(toDate))
            {
                _messageBox.Show("Вы можете использовать даты для сортировки или 'ЗА Год-Месяц-День' или 'С Год-Месяц-День ПО Год-Месяц-День'.");
            }
            else if (string.IsNullOrEmpty(date) && !string.IsNullOrEmpty(fromDate) && string.IsNullOrEmpty(toDate) || string.IsNullOrEmpty(date) && string.IsNullOrEmpty(fromDate) && !string.IsNullOrEmpty(toDate))
            {
                _messageBox.Show("Чтобы использовать даты для сортировки 'С Год-Месяц-День ПО Год-Месяц-День',\n Вы должны заполнить оба поля.");
            }
            else if (!string.IsNullOrEmpty(date) && string.IsNullOrEmpty(fromDate) && string.IsNullOrEmpty(toDate))
            {
                SortDate(date, out outDate);
            }
            else if (string.IsNullOrEmpty(date) && !string.IsNullOrEmpty(fromDate) && !string.IsNullOrEmpty(toDate))
            {
                SortDate(fromDate, out outFromDate);
                SortDate(toDate, out outToDate);
                if (outFromDate && outToDate)
                {
                    DateTime parsedFromDate = DateTime.ParseExact(fromDate, "dd.MM.yyyy", CultureInfo.InvariantCulture);
                    string stringParsedFromDate = parsedFromDate.ToString("yyyy-MM-dd");
                    DateTime fromDateFormat = DateTime.ParseExact(stringParsedFromDate, "yyyy-MM-dd", CultureInfo.InvariantCulture);
                    DateTime parsedToDate = DateTime.ParseExact(toDate, "dd.MM.yyyy", CultureInfo.InvariantCulture);
                    string stringParsedToDate = parsedToDate.ToString("yyyy-MM-dd");
                    DateTime toDateFormat = DateTime.ParseExact(stringParsedToDate, "yyyy-MM-dd", CultureInfo.InvariantCulture);
                    if (fromDateFormat < toDateFormat)
                    {
                        outFromDate = true;
                        outToDate = true;
                    }
                    else
                    {
                        _messageBox.Show("Дата 'С' должна быть раньше даты 'По'");
                        outFromDate = false;
                        outToDate = false;
                    }
                }
            }
            Debug.WriteLine("### End of method CheckingDate ###");
        }
    }
}
