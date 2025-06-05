using System.Diagnostics;
using System.Globalization;
using System.Windows;
using WPFStarter.Model;
using WPFStarter.ImportAndExport;
using System.IO;
namespace WPFStarter.ProgramLogic
{
    internal class Program
    {

        public static bool statusExport = true;
        ///<summary>
        /// E.A.T. 4-February-2025
        /// Validation of entered data for sorting. 
        /// E.A.T. 25-March-2025
        /// Adding asynchrony to data export.
        ///</summary>
        public static async void SortDataAsync(string? date, string? fromDate, string? toDate, string? firstName, string? lastName, string? surName, string? city, string? country, string? fileType, string? fileName)
        {
            Debug.WriteLine("### Start of method SortDataAsync ###");
            CheckingDate(date, fromDate, toDate, out bool outDate, out bool outFromDate, out bool outToDate);
            CheckingWord(firstName, out bool outFirstName);
            CheckingWord(lastName, out bool outLastName);
            CheckingWord(surName, out bool outSurName);
            CheckingWord(city, out bool outCity);
            CheckingWord(country, out bool outCountry);
            if(!string.IsNullOrEmpty(date) && outDate == false || !string.IsNullOrEmpty(fromDate) && outFromDate == false || !string.IsNullOrEmpty(toDate) && outToDate == false || !string.IsNullOrEmpty(firstName) && outFirstName == false || !string.IsNullOrEmpty(lastName) && outLastName == false || !string.IsNullOrEmpty(surName) && outSurName == false || !string.IsNullOrEmpty(city) && outCity == false || !string.IsNullOrEmpty(country) && outCountry == false)
            {
                statusExport = false;
                MessageBox.Show("Исправьте данные!");
            }
            else
            {
                if (MessageBox.Show($"Вы хотите перенести данные?\nВаши данные:\nДата за {date}\nДата с {fromDate} по {toDate}\nГород {city}\nСтрана {country}\nФамилия {lastName}\nИмя {firstName}\nОтчество{surName}\nТип файла: {fileType}", "Перенос данных", MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.Yes)
                {
                    await Task.Run(() => { 
                    SortingData.ExportDataAsync(fileName, fileType, date, fromDate, toDate, firstName, lastName, surName, city, country, outDate, outFromDate, outToDate, outFirstName, outLastName, outSurName, outCity, outCountry); 
                    });
                }
                else
                {
                    statusExport = false;
                    MessageBox.Show("Исправьте данные!");
                }
            }
            Debug.WriteLine("### End of method SortDataAsync ###");
        }
        ///<summary>
        /// E.A.T. 4-February-2025
        /// Checking date format.
        ///</summary>
        public static void SortDate(string? date, out bool outDate)
        {
            Debug.WriteLine("### Start of method SortDate ###");
            outDate = true;
            if(!string.IsNullOrEmpty(date))
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
            Debug.WriteLine("### End of method SortDate ###");
        }
        ///<summary>
        /// E.A.T. 5-February-2025
        /// Checking date.
        ///</summary>
        public static void CheckingDate(string? date, string? fromDate, string? toDate, out bool outDate, out bool outFromDate, out bool outToDate)
        {
            Debug.WriteLine("### Start of method CheckingDate ###");
            outDate = false;
            outFromDate = false;
            outToDate = false;
            if(!string.IsNullOrEmpty(date) && !string.IsNullOrEmpty(fromDate) || !string.IsNullOrEmpty(date) && !string.IsNullOrEmpty(toDate))
            {
                MessageBox.Show("Вы можете использовать даты для сортировки или 'ЗА Год-Месяц-День' или 'С Год-Месяц-День ПО Год-Месяц-День'.");
            }
            else if(string.IsNullOrEmpty(date) && !string.IsNullOrEmpty(fromDate) && string.IsNullOrEmpty(toDate) || string.IsNullOrEmpty(date) && string.IsNullOrEmpty(fromDate) && !string.IsNullOrEmpty(toDate))
            {
                MessageBox.Show("Чтобы использовать даты для сортировки 'С Год-Месяц-День ПО Год-Месяц-День',\n Вы должны заполнить оба поля.");
            }
            else if(!string.IsNullOrEmpty(date) && string.IsNullOrEmpty(fromDate) && string.IsNullOrEmpty(toDate))
            {
                SortDate(date, out outDate);
            }
            else if(string.IsNullOrEmpty(date) && !string.IsNullOrEmpty(fromDate) && !string.IsNullOrEmpty(toDate))
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
                        MessageBox.Show("Дата 'С' должна быть раньше даты 'По'");
                        outFromDate = false;
                        outToDate = false;
                    }
                }
            }
            Debug.WriteLine("### End of method CheckingDate ###");
        }
        ///<summary>
        /// E.A.T. 7-February-2025
        /// Checking the entered word.
        ///</summary>
        public static void CheckingWord(string? word, out bool outWord)
        {
            Debug.WriteLine("### Start of method CheckingWord ###");
            outWord = false;
            if(!string.IsNullOrEmpty(word))
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
            Debug.WriteLine("### End of method CheckingWord ###");
        }
        
        ///<summary>
        /// E.A.T. 11-February-2025
        /// Sorting data for recording.
        ///</summary>
        public static async Task<List<Person>> SortingDataForRecordingAsync(string? date, string? fromDate, string? toDate, string? firstName, string? lastName, string? surName, string? city, string? country, bool outDate, bool outFromDate, bool outToDate, bool outFirstName, bool outLastName, bool outSurName, bool outCity, bool outCountry)
        {
            Debug.WriteLine("### Start of method SortingDataForRecordingAsync ###");
            List<Person> records = await DatabaseReader.ReadDataAsync();
            var newRecords = new List<Person>();
            newRecords = records;

            if (records.Count != 0 )
            {
                if(!string.IsNullOrEmpty(date) && outDate)
            {
                DateTime parsedDate = DateTime.ParseExact(date, "dd.MM.yyyy", CultureInfo.InvariantCulture);
                string stringParsedDate = parsedDate.ToString("yyyy-MM-dd");
                DateTime dateFormat = DateTime.ParseExact(stringParsedDate, "yyyy-MM-dd", CultureInfo.InvariantCulture);
                newRecords = records
                    .Where(person => person.Date == dateFormat)
                    .ToList();
            }
            else if (!string.IsNullOrEmpty(fromDate) && outFromDate && !string.IsNullOrEmpty(toDate) && outToDate)
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
            if(!string.IsNullOrEmpty(firstName) && outFirstName)
            {
                newRecords = records
                    .Where(person => person.FirstName == firstName)
                    .ToList();
            }
            if(!string.IsNullOrEmpty (lastName) && outLastName)
            {
                newRecords = records
                    .Where(person => person.LastName == lastName)
                    .ToList();
            }
            if(!string.IsNullOrEmpty(surName) && outSurName)
            {
                newRecords = records
                    .Where(person => person.SurName == surName)
                    .ToList();
            }
            if(!string.IsNullOrEmpty(city) && outCity)
            {
                newRecords = records
                    .Where(person => person.City == city)
                    .ToList();
            }
            if (!string.IsNullOrEmpty(country) && outCountry)
            {
                newRecords = records
                    .Where(person => person.Country == country)
                    .ToList();
            }
            }
            Debug.WriteLine("### End of method SortingDataForRecordingAsync ###");
            return newRecords;
            
        }
        ///<summary>
        /// E.A.T. 16-April-2025
        /// Asynchronous writing of "server" and "database" data.
        ///</summary>
        public static async Task SaveServerDatabase(string server, string database)
        {
            Debug.WriteLine("### Start of method SaveServerDatabase ###");
            await File.WriteAllTextAsync("db.txt", $"{server} {database}");
            Debug.WriteLine("### End of method SaveServerDatabase ###");
        }
    }
}
