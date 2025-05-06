using Microsoft.Data.SqlClient;
using Microsoft.VisualBasic.FileIO;
using System.Diagnostics;
using System.IO;
using System.Text;
using System.Windows;
using System.Xml.Linq;
using WPFStarter.ProgramLogic;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;

namespace WPFStarter
{
    /// <summary>
    /// E.A.T. 24-March-2025
    /// Create the ImportExport class and transfer the necessary methods from the Program class to it.
    /// </summary>
    class ImportExport
    {
        /// <summary>
        /// Added "isImportCsvRunning" to track import activity.
        /// Added "isExportCsvRunning" to track export activity.
        /// E.A.T. 23-April-2025
        /// Added "statusImport" to enable and disable progressbar.
        /// E.A.T. 25-April-2025
        /// Added "statusExport" to enable and disable progressbar.
        /// E.A.T. 21-April-2025
        /// Added "windowDB" to enable and disable the window for entering "server" and "database" data.
        /// </summary>
        public static bool isImportCsvRunning = false;
        public static bool isExportDataRunning = false;
        public static bool statusImport = true;
        public static bool statusExport = true;
        public static bool windowDB = false;
        ///<summary>
        /// E.A.T. 25-December-2024
        /// Reading data from a .csv file and transferring it to a list of objects.
        /// Writing data to the database.
        /// E.A.T. 25-March-2025
        /// Adding asynchrony to data import.
        /// E.A.T. 03-April-2025
        /// Updated the use of asynchrony.
        ///</summary>
        public static async Task ImportCsvAsync(string filePath)
        {
            Debug.WriteLine("### Start of method ImportCsvAsync ###");
            statusImport = true;
            windowDB = false;
            try
            {
                var records = await ReadingDataAsync(filePath);
                await RecordDatabaseAsync(records);
                statusImport = false;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка импорта {ex}");
                statusImport = false;
            }
            Debug.WriteLine("### End of method ImportCsvAsync ###");
        }
        ///<summary>
        /// E.A.T. 29-January-2025
        /// Reading data from a .csv file and transferring it to a list of objects.
        /// E.A.T. 03-April-2025
        /// Added asynchrony for reading from a .csv file.
        ///</summary>
        public static async Task<List<Person>> ReadingDataAsync(string filePath)
        {
            Debug.WriteLine("### Start of method ReadingDataAsync ###");
            return await Task.Run(() =>
            {
                var records = new List<Person>();
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
                Debug.WriteLine("### End of method ReadingDataAsync ###");
                return records;
            });
        }
        ///<summary>
        /// E.A.T. 30-January-2025
        /// Record data to the database.
        /// E.A.T. 03-April-2025
        /// Added asynchrony for writing data to the database.
        /// E.A.T. 07-April-2025
        /// Splitting data package into parts.
        /// E.A.T. 17-April-2025
        /// Checking connection to the database.
        /// E.A.T. 02-May-2025
        /// Writing data to a DB table using asynchronous streams.
        ///</summary>
        public static async Task RecordDatabaseAsync(List<Person> records)
        {
            Debug.WriteLine("### Start of method RecordDatabaseAsync ###");
            await Task.Run(async () => {
                using (var context = new ApplicationContext())
                {
                    try
                    {
                        if (!context.Database.CanConnect())
                        {
                            throw new Exception("Подключение к базе данных невозможно. Проверьте строку подключения.");
                        }
                        else
                        {
                            isImportCsvRunning = true;
                            await foreach (var batch in GetDataAsync(records, 1000))
                            {
                                context.Table_People_Data.AddRange(batch);
                                context.SaveChanges();
                            }
                            isImportCsvRunning = false;
                            MessageBox.Show("Успешно!");
                        }
                    }
                    catch (Exception ex)
                    {
                        statusImport = false;
                        isImportCsvRunning = false;
                        windowDB = true;
                        Debug.WriteLine($"Ошибка подключения: {ex.Message}");
                        MessageBox.Show($"Ошибка подключения: {ex.Message}");
                        return;
                    }
                }
            });
            Debug.WriteLine("### End of method RecordDatabaseAsync ###");
        }
        ///<summary>
        /// E.A.T. 02-May-2025
        /// Using asynchronous streams to read data in 1000-record increments..
        ///</summary>
        public static async IAsyncEnumerable<List<Person>> GetDataAsync(List<Person> records, int packageSize)
        {
            Debug.WriteLine("### Start of method GetDataAsync ###");
            int totalRecords = records.Count;
            List<Person> batch = new List<Person>();
            foreach (var record in records)
            {
                batch.Add(record);
                if (batch.Count == packageSize)
                {
                    yield return batch;
                    batch = new List<Person>();
                    await Task.Delay(500);
                }
            }
            if (batch.Count > 0)
            {
                yield return batch;
            }
            Debug.WriteLine("### End of method GetDataAsync ###");
        }
        ///<summary>
        /// E.A.T. 30-January-2025
        /// Outputting data from a .csv file to the screen.
        ///</summary>
        public static void OutputDataScreen(List<Person> records)
        {
            Debug.WriteLine("### Start of method OutputDataScreen ###");
            foreach (var record in records)
            {
                MessageBox.Show($"{record.Date}, {record.FirstName}, {record.LastName}, {record.SurName}, {record.City}, {record.Country}");
            }
            MessageBox.Show("Данные записанны!");
            Debug.WriteLine("### End of method OutputDataScreen ###");
        }
        ///<summary>
        /// E.A.T. 3-February-2025
        /// Outputting data from the DB to the screen.
        ///</summary>
        public static void OutputDataScreenId(List<Person> records)
        {
            Debug.WriteLine("### Start of method OutputDataScreenId ###");
            foreach (var record in records)
            {
                MessageBox.Show($"{record.Id}, {record.Date}, {record.FirstName}, {record.LastName}, {record.SurName}, {record.City}, {record.Country}");
            }
            Debug.WriteLine("### End of method OutputDataScreenId ###");
        }
        ///<summary>
        /// E.A.T. 3-February-2025
        /// Outputting data from the DB to the list of objects.
        /// E.A.T. 18-April-2025
        /// Adding error handling if the database connection string is incorrect.
        ///</summary>
        public static async Task<List<Person>> ReadDataAsync()
        {
            Debug.WriteLine("### Start of method ReadDataAsync ###");
            isExportDataRunning = true;
            var records = new List<Person>();
            string connectionString = "Server=localhost;Database=Peopl;Trusted_Connection=True;TrustServerCertificate=True;";
            string query = "SELECT Id, Date, FirstName, LastName, SurName, City, Country FROM Table_People_Data";
            try
            {
                records = await ListPersonAsync(connectionString, query);
                isExportDataRunning = false;
            }
            catch (Exception ex) {
                try
                {
                    string[] elements = File.ReadAllText("db.txt").Split(' ');
                    string server = elements[0];
                    string database = elements[1];
                    connectionString = $"Server={server};Database={database};Trusted_Connection=True;TrustServerCertificate=True;";
                    records = await ListPersonAsync(connectionString, query);
                    isExportDataRunning = false;
                }
                catch{
                    statusExport = false;
                    isExportDataRunning = false;
                    windowDB = true;
                    Debug.WriteLine($"Ошибка подключения: {ex.Message}");
                    MessageBox.Show($"Ошибка подключения: {ex.Message}");
                }
            }
            Debug.WriteLine("### End of method ReadDataAsync ###");
            return records;
        }
        ///<summary>
        /// E.A.T. 18-April-2025
        /// Recording data about people in "records" of type "List<Person>".
        ///</summary>
        public static async Task<List<Person>> ListPersonAsync(string connectionString, string query)
        {
            Debug.WriteLine("### Start of method ListPersonAsync ###");
            var records = new List<Person>();
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                SqlCommand command = new SqlCommand(query, connection);
                await connection.OpenAsync();
                SqlDataReader reader = await command.ExecuteReaderAsync();
                while (await reader.ReadAsync())
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
            }
            Debug.WriteLine("### End of method ListPersonAsync ###");
            return records;
        }
        ///<summary>
        /// E.A.T. 10-February-2025
        /// Checking the entered word.
        ///</summary>
        public static void CreateFile(string? fileName, string? typeFile, out string? fullFileName, List<Person> records)
        {
            Debug.WriteLine("### Start of method CreateFile ###");
            fullFileName = null;
            if (fileName != "" || typeFile != "")
            {
                fullFileName = $"{fileName}{typeFile}";
                FileAvailability(fullFileName, typeFile, records);
            }
            Debug.WriteLine("### End of method CreateFile ###");
        }
        ///<summary>
        /// E.A.T. 10-February-2025
        /// Checking that such a file does not exist yet.
        ///</summary>
        public static void FileAvailability(string fileName, string? typeFile, List<Person> records)
        {
            Debug.WriteLine("### Start of method FileAvailability ###");
            if (File.Exists(fileName))
            {
                MessageBox.Show($"Файл {fileName} уже есть");
            }
            else
            {
                if (typeFile == ".csv")
                {
                    SaveCSVAsync(fileName, records);
                }
                else if (typeFile == ".xml")
                {
                    SaveXMLAsync(fileName, records);
                }
            }
            Debug.WriteLine("### End of method FileAvailability ###");
        }
        ///<summary>
        /// E.A.T. 11-February-2025
        /// Data export.
        ///</summary>
        public static async void ExportDataAsync(string? fileName, string? typeFile, string? date, string? fromDate, string? toDate, string? firstName, string? lastName, string? surName, string? city, string? country, bool outDate, bool outFromDate, bool outToDate, bool outFirstName, bool outLastName, bool outSurName, bool outCity, bool outCountry)
        {
            Debug.WriteLine("### Start of method ExportDataAsync ###");
            windowDB = false;
            List<Person> records = await Program.SortingDataForRecordingAsync(
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
            if (records.Count != 0)
                CreateFile(fileName, typeFile, out string? fullFileName, records);
            Debug.WriteLine("### End of method ExportDataAsync ###");
        }
        ///<summary>
        /// E.A.T. 11-February-2025
        /// Data export to .xml.
        ///</summary>
        public static async void SaveXMLAsync(string fileName, List<Person> records)
        {
            Debug.WriteLine("### Start of method SaveXMLAsync ###");
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
            await Task.Run(() => {
            XDocument xdoc = new XDocument(testProgramElement);
            xdoc.Save($"{fileName}");
            });
            
            MessageBox.Show("Data saved .XML");
            statusExport = false;
            Debug.WriteLine("### End of method SaveXMLAsync ###");
        }
        ///<summary>
        /// E.A.T. 12-February-2025
        /// Data export to .csv.
        ///</summary>
        public static async void SaveCSVAsync(string filePath, List<Person> records)
        {
            Debug.WriteLine("### Start of method SaveCSVAsync ###");
            using (StreamWriter writer = new StreamWriter(filePath, false, Encoding.UTF8))
            {
                foreach (var person in records)
                {
                    await Task.Run(() => { writer.WriteLine($"{person.Id};{person.Date:yyyy-MM-dd};{person.FirstName};{person.LastName};{person.SurName};{person.City};{person.Country}"); });
                }
            }
           
            MessageBox.Show("Data saved .CSV");
            statusExport = false;
            Debug.WriteLine("### End of method SaveCSVAsync ###");
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
