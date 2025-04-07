using System.Globalization;
using System.Windows;
namespace WPFStarter.ProgramLogic
{
    internal class Program
    {
        ///<summary>
        /// E.A.T. 4-February-2025
        /// Validation of entered data for sorting. 
        /// E.A.T. 25-March-2025
        /// Adding asynchrony to data export.
        ///</summary>
        public static async Task SortData(string? date, string? fromDate, string? toDate, string? firstName, string? lastName, string? surName, string? city, string? country, string? fileType, string? fileName)
        {
            CheckingDate(date, fromDate, toDate, out bool outDate, out bool outFromDate, out bool outToDate);
            CheckingWord(firstName, out bool outFirstName);
            CheckingWord(lastName, out bool outLastName);
            CheckingWord(surName, out bool outSurName);
            CheckingWord(city, out bool outCity);
            CheckingWord(country, out bool outCountry);
            if (date != "" && outDate == false || fromDate != "" && outFromDate == false || toDate != "" && outToDate == false || firstName != "" && outFirstName == false || lastName != "" && outLastName == false || surName != "" && outSurName == false || city != "" && outCity == false || country != "" && outCountry == false)
            {
                MessageBox.Show("Исправьте данные!");
            }
            else
            {
                if (MessageBox.Show($"Вы хотите перенести данные?\nВаши данные:\nДата за {date}\nДата с {fromDate} по {toDate}\nГород {city}\nСтрана {country}\nФамилия {lastName}\nИмя {firstName}\nОтчество{surName}\nТип файла: {fileType}", "Перенос данных", MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.Yes)
                {
                    Task.Run(() => { 
                        ImportExport.ExportData(fileName, fileType, date, fromDate, toDate, firstName, lastName, surName, city, country, outDate, outFromDate, outToDate, outFirstName, outLastName, outSurName, outCity, outCountry); 
                    });
                }
                else
                {
                    MessageBox.Show("Исправьте данные!");
                }
            }
        }
        ///<summary>
        /// E.A.T. 4-February-2025
        /// Checking date format.
        ///</summary>
        public static void SortDate(string? date, out bool outDate)
        {
            outDate = true;
            if (date != null)
            {
                try
                {

                    DateTime parsedDate = DateTime.ParseExact(date, "dd.MM.yyyy", CultureInfo.InvariantCulture);
                    string stringParsedDate = parsedDate.ToString("yyyy-MM-dd");
                    DateTime dateFormat = DateTime.ParseExact(stringParsedDate, "yyyy-MM-dd", CultureInfo.InvariantCulture);
                    MessageBox.Show($"{dateFormat}");
                }
                catch (FormatException)
                {
                    MessageBox.Show($"Неправильный формат даты.\nДата должна иметь вид: Год-Месяц-День\nПример: 2024-02-01\nВы ввели: {date}");
                    outDate = false;
                }
            }
        }
        ///<summary>
        /// E.A.T. 5-February-2025
        /// Checking date.
        ///</summary>
        public static void CheckingDate(string? date, string? fromDate, string? toDate, out bool outDate, out bool outFromDate, out bool outToDate)
        {
            outDate = false;
            outFromDate = false;
            outToDate = false;
            if (date != "" && fromDate != "" || date != "" && toDate != "")
            {
                MessageBox.Show("Вы можете использовать даты для сортировки или 'ЗА Год-Месяц-День' или 'С Год-Месяц-День ПО Год-Месяц-День'.");
            }
            else if (date == "" && fromDate != "" && toDate == "" || date == "" && fromDate == "" && toDate != "")
            {
                MessageBox.Show("Чтобы использовать даты для сортировки 'С Год-Месяц-День ПО Год-Месяц-День',\n Вы должны заполнить оба поля.");
            }
            else if (date != "" && fromDate == "" && toDate == "")
            {
                SortDate(date, out outDate);
            }
            else if (date == "" && fromDate != "" && toDate != "")
            {
                SortDate(fromDate, out outFromDate);
                SortDate(toDate, out outToDate);
                if (outFromDate == true && outToDate == true)
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
                        MessageBox.Show("Дата 'С' должна быть раньше даты 'По'");
                        outFromDate = false;
                        outToDate = false;
                    }
                }
            }
        }
        ///<summary>
        /// E.A.T. 7-February-2025
        /// Checking the entered word.
        ///</summary>
        public static void CheckingWord(string? word, out bool outWord)
        {
            outWord = false;
            if (word != "")
            {
                int lengthWord = word.Length;
                string capitalLetters = "АБВГДЕЁЖЗИЙКЛМНОПРСТУФХЦЧШЩЪЫЬЭЮЯ";
                string lowercaseLetters = "абвгдеёжзийклмнопрстуфхцчшщъыьэюя-";
                int counter = 0;
                for (int i = 0; i < capitalLetters.Length; i++)
                {
                    if (word[0] == capitalLetters[i])
                    {
                        for (int j = 0; j < lowercaseLetters.Length; j++)
                        {
                            for (int k = 0; k < lengthWord; k++)
                            {
                                if (lowercaseLetters[j] == word[k])
                                {
                                    counter++;
                                }
                            }
                        }
                        for (int j = 0; j < capitalLetters.Length; j++)
                        {
                            for (int k = 0; k < lengthWord; k++)
                            {
                                if (capitalLetters[j] == word[k])
                                {
                                    counter++;
                                }
                            }
                        }
                    }
                }
                if (counter == 0)
                {
                    MessageBox.Show($"{word} нужно ввести с большой буквы!");
                }
                else if (counter != lengthWord)
                {
                    MessageBox.Show($"После первой заглавной буквы Вы ввели недопустимый симол или ввели пробел! Вы ввели: {word}");
                }
                else
                {
                    outWord = true;
                }

            }
        }
        
        ///<summary>
        /// E.A.T. 11-February-2025
        /// Sorting data for recording.
        ///</summary>
        public static async Task<List<Person>> SortingDataForRecording(string? date, string? fromDate, string? toDate, string? firstName, string? lastName, string? surName, string? city, string? country, bool outDate, bool outFromDate, bool outToDate, bool outFirstName, bool outLastName, bool outSurName, bool outCity, bool outCountry)
        {
            //ImportExport.ReadData(out List<Person> records);
            List<Person> records = await ImportExport.ReadData();
            var newRecords = new List<Person>();
            newRecords = records;
            if (date != "" && outDate == true)
            {
                DateTime parsedDate = DateTime.ParseExact(date, "dd.MM.yyyy", CultureInfo.InvariantCulture);
                string stringParsedDate = parsedDate.ToString("yyyy-MM-dd");
                DateTime dateFormat = DateTime.ParseExact(stringParsedDate, "yyyy-MM-dd", CultureInfo.InvariantCulture);
                newRecords = records
                    .Where(person => person.Date == dateFormat)
                    .ToList();
            }
            else if (fromDate != "" && outFromDate == true && toDate != "" && outToDate == true)
            {
                DateTime parsedFromDate = DateTime.ParseExact(fromDate, "dd.MM.yyyy", CultureInfo.InvariantCulture);
                string stringParsedFromDate = parsedFromDate.ToString("yyyy-MM-dd");
                DateTime fromDateFormat = DateTime.ParseExact(stringParsedFromDate, "yyyy-MM-dd", CultureInfo.InvariantCulture);
                DateTime parsedToDate = DateTime.ParseExact(toDate, "dd.MM.yyyy", CultureInfo.InvariantCulture);
                string stringParsedToDate = parsedToDate.ToString("yyyy-MM-dd");
                DateTime toDateFormat = DateTime.ParseExact(stringParsedToDate, "yyyy-MM-dd", CultureInfo.InvariantCulture);
                newRecords = records
                    .Where(person => person.Date >= fromDateFormat && person.Date <= toDateFormat)
                    .ToList();
            }
            if (firstName != "" && outFirstName == true)
            {
                newRecords = records
                    .Where(person => person.FirstName == firstName)
                    .ToList();
            }
            if (lastName != "" && outLastName == true)
            {
                newRecords = records
                    .Where(person => person.LastName == lastName)
                    .ToList();
            }
            if (surName != "" && outSurName == true)
            {
                newRecords = records
                    .Where(person => person.SurName == surName)
                    .ToList();
            }
            if (city != "" && outCity == true)
            {
                newRecords = records
                    .Where(person => person.City == city)
                    .ToList();
            }
            if (country != "" && outCountry == true)
            {
                newRecords = records
                    .Where(person => person.Country == country)
                    .ToList();
            }
            return newRecords;
        }


    }
}
