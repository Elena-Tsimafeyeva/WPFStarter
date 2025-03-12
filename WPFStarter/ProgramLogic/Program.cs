using Microsoft.Data.SqlClient;
using Microsoft.VisualBasic.FileIO;
using System.Globalization;
using System.IO;
using System.Text;
using System.Windows;
using System.Xml.Linq;
///<summary>
/// E.A.T. 25-December-2024
/// Reading data from a .csv file and transferring it to a list of objects.
/// Writing data to the database.
///</summary>
namespace WPFStarter.ProgramLogic
{
    internal class Program
    {
        public static void ImportCsv(string filePath)
        {
            var records = new List<Person>();
            ReadingData(records, filePath);
            RecordDatabase(records);
            OutputDataScreen(records);
        }
        ///<summary>
        /// E.A.T. 29-January-2025
        /// Reading data from a .csv file and transferring it to a list of objects.
        ///</summary>
        public static void ReadingData(List<Person> records, string filePath)
        {
            using TextFieldParser tfp = new(filePath);
            {
                tfp.TextFieldType = FieldType.Delimited;
                tfp.SetDelimiters(";");

                while (!tfp.EndOfData)
                {
                    var values = tfp.ReadFields();
                    var record = new Person
                    {
                        Date = DateTime.Parse(values[0]),
                        FirstName = values[1],
                        LastName = values[2],
                        SurName = values[3],
                        City = values[4],
                        Country = values[5]
                    };

                    records.Add(record);
                }
            }
        }
        ///<summary>
        /// E.A.T. 30-January-2025
        /// Record data to the database.
        ///</summary>
        public static void RecordDatabase(List<Person> records)
        {
            using var context = new ApplicationContext();
            {
                context.Table_People.AddRange(records);
                context.SaveChanges();
            }
        }
        ///<summary>
        /// E.A.T. 30-January-2025
        /// Outputting data from a .csv file to the screen.
        ///</summary>
        public static void OutputDataScreen(List<Person> records)
        {
            foreach (var record in records)
            {
                MessageBox.Show($"{record.Date}, {record.FirstName}, {record.LastName}, {record.SurName}, {record.City}, {record.Country}");
            }
            MessageBox.Show("Данные записанны!");
        }
        ///<summary>
        /// E.A.T. 3-February-2025
        /// Outputting data from the DB to the screen.
        ///</summary>
        public static void OutputDataScreenId(List<Person> records)
        {
            foreach (var record in records)
            {
                MessageBox.Show($"{record.Id}, {record.Date}, {record.FirstName}, {record.LastName}, {record.SurName}, {record.City}, {record.Country}");
            }
        }
        ///<summary>
        /// E.A.T. 3-February-2025
        /// Outputting data from the DB to the list of objects.
        ///</summary>
        public static void ReadData(out List<Person> records)
        {
            records = new List<Person>();
            string connectionString = "Server=localhost;Database=People;Trusted_Connection=True;TrustServerCertificate=True;";
            string query = "SELECT Id, Date, FirstName, LastName, SurName, City, Country FROM Table_People";
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                SqlCommand command = new SqlCommand(query, connection);
                connection.Open();
                SqlDataReader reader = command.ExecuteReader();
                while (reader.Read())
                {
                    Person person = new Person
                    {
                        Id = (int)reader["Id"],
                        Date = (DateTime)reader["Date"],
                        FirstName = reader["FirstName"].ToString(),
                        LastName = reader["LastName"].ToString(),
                        SurName = reader["SurName"].ToString(),
                        City = reader["City"].ToString(),
                        Country = reader["Country"].ToString()

                    };
                    records.Add(person);

                }
                reader.Close();
                MessageBox.Show("Данные из БД Записанны!");
            }

        }
        ///<summary>
        /// E.A.T. 4-February-2025
        /// Validation of entered data for sorting. 
        ///</summary>
        public static void SortData(string? date, string? fromDate, string? toDate, string? firstName, string? lastName, string? surName, string? city, string? country, string? fileType, string? fileName)
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
                    ExportData(fileName, fileType, date, fromDate, toDate, firstName, lastName, surName, city, country, outDate, outFromDate, outToDate, outFirstName, outLastName, outSurName, outCity, outCountry);
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
                    DateTime dateFormat = DateTime.ParseExact(date, "yyyy-MM-dd", CultureInfo.InvariantCulture);
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
                    DateTime fromDatFormat = DateTime.ParseExact(fromDate, "yyyy-MM-dd", CultureInfo.InvariantCulture);
                    DateTime toDateFormat = DateTime.ParseExact(toDate, "yyyy-MM-dd", CultureInfo.InvariantCulture);
                    if (fromDatFormat < toDateFormat)
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
        /// E.A.T. 10-February-2025
        /// Checking the entered word.
        ///</summary>
        public static void CreateFile(string? fileName, string? typeFile, out string? fullFileName, List<Person> records)
        {
            fullFileName = null;
            if (fileName != "" || typeFile != "")
            {
                fullFileName = $"{fileName}{typeFile}";
                MessageBox.Show($"{fullFileName}");
                FileAvailability(fullFileName, typeFile, records);
            }
        }
        ///<summary>
        /// E.A.T. 10-February-2025
        /// Checking that such a file does not exist yet.
        ///</summary>
        public static void FileAvailability(string fileName, string? typeFile, List<Person> records)
        {
            if (File.Exists(fileName))
            {
                MessageBox.Show($"Файл {fileName} уже есть");
            }
            else
            {
                if (typeFile == ".csv")
                {
                    SaveCSV(fileName, records);
                }
                else if (typeFile == ".xml")
                {
                    SaveXML(fileName, records);
                }
            }
        }
        ///<summary>
        /// E.A.T. 11-February-2025
        /// Data export.
        ///</summary>
        public static void ExportData(string? fileName, string? typeFile, string? date, string? fromDate, string? toDate, string? firstName, string? lastName, string? surName, string? city, string? country, bool outDate, bool outFromDate, bool outToDate, bool outFirstName, bool outLastName, bool outSurName, bool outCity, bool outCountry)
        {
            List<Person> records;
            SortingDataForRecording(out records,
                                    date,
                                    fromDate,
                                    toDate,
                                    firstName,
                                    lastName,
                                    surName,
                                    city,
                                    country,
                                    outDate,
                                    outFromDate,
                                    outToDate,
                                    outFirstName,
                                    outLastName,
                                    outSurName,
                                    outCity,
                                    outCountry);
            CreateFile(fileName, typeFile, out string? fullFileName, records);
        }
        ///<summary>
        /// E.A.T. 11-February-2025
        /// Data export to .xml.
        ///</summary>
        public static void SaveXML(string fileName, List<Person> records)
        {
            XElement testProgramElement = new XElement("TestProgram");

            foreach (var person in records)
            {
                XElement personElement = new XElement("Record",
                    new XAttribute("id", person.Id),
                    new XElement("Date", person.Date.ToString("yyyy-MM-dd")),
                    new XElement("FirstName", person.FirstName),
                    new XElement("LastName", person.LastName),
                    new XElement("SurName", person.SurName),
                    new XElement("City", person.City),
                    new XElement("Country", person.Country)
                );

                testProgramElement.Add(personElement);
            }

            XDocument xdoc = new XDocument(testProgramElement);
            xdoc.Save($"{fileName}");

            MessageBox.Show("Data saved");

        }
        ///<summary>
        /// E.A.T. 12-February-2025
        /// Data export to .csv.
        ///</summary>
        public static void SaveCSV(string filePath, List<Person> records)
        {
            using (StreamWriter writer = new StreamWriter(filePath, false, Encoding.UTF8))
            {
                foreach (var person in records)
                {
                    writer.WriteLine($"{person.Id};{person.Date:yyyy-MM-dd};{person.FirstName};{person.LastName};{person.SurName};{person.City};{person.Country}");
                }
            }

        }
        ///<summary>
        /// E.A.T. 11-February-2025
        /// Sorting data for recording.
        ///</summary>
        public static void SortingDataForRecording(out List<Person> newRecords, string? date, string? fromDate, string? toDate, string? firstName, string? lastName, string? surName, string? city, string? country, bool outDate, bool outFromDate, bool outToDate, bool outFirstName, bool outLastName, bool outSurName, bool outCity, bool outCountry)
        {
            ReadData(out List<Person> records);
            newRecords = new List<Person>();
            newRecords = records;
            if (date != "" && outDate == true)
            {
                DateTime dateFormat = DateTime.ParseExact(date, "yyyy-MM-dd", CultureInfo.InvariantCulture);
                newRecords = records
                    .Where(person => person.Date == dateFormat)
                    .ToList();
            }
            else if (fromDate != "" && outFromDate == true && toDate != "" && outToDate == true)
            {
                DateTime fromDateFormat = DateTime.ParseExact(fromDate, "yyyy-MM-dd", CultureInfo.InvariantCulture);
                DateTime toDateFormat = DateTime.ParseExact(toDate, "yyyy-MM-dd", CultureInfo.InvariantCulture);
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

        }


    }
}
