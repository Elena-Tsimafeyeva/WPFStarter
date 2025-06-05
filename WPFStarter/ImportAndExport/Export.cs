using Microsoft.Data.SqlClient;
using System.Diagnostics;
using System.IO;
using System.Text;
using System.Windows;
using System.Xml.Linq;
using WPFStarter.ProgramLogic;
using WPFStarter.Model;

namespace WPFStarter.ImportAndExport
{
    class ExportState
    {
        ///<summary>
        /// Added "isExportCsvRunning" to track export activity.
        /// E.A.T. 25-April-2025
        /// Added "statusExport" to enable and disable progressbar.
        /// E.A.T. 21-April-2025
        /// Added "windowDB" to enable and disable the window for entering "server" and "database" data.
        ///</summary>
        public static bool exportRunning { get; set; } = false;
        public static bool statusExport { get; set; }  = true;
        public static bool windowDB { get; set; }  = false;
    }
    class DatabaseReader
    {
        ///<summary>
        /// E.A.T. 3-February-2025
        /// Outputting data from the DB to the list of objects.
        /// E.A.T. 18-April-2025
        /// Adding error handling if the database connection string is incorrect.
        ///</summary>
        public static async Task<List<Person>> ReadDataAsync()
        {
            Debug.WriteLine("### Start of method ReadDataAsync ###");
            ExportState.exportRunning = true;
            var records = new List<Person>();
            string connectionString = "Server=localhost;Database=Peopl;Trusted_Connection=True;TrustServerCertificate=True;";
            string query = "SELECT Id, Date, FirstName, LastName, SurName, City, Country FROM Table_People_Data";
            try
            {
                records = await ListPersonAsync(connectionString, query);
                ExportState.exportRunning = false;
            }
            catch (Exception ex)
            {
                try
                {
                    string[] elements = File.ReadAllText("db.txt").Split(' ');
                    string server = elements[0];
                    string database = elements[1];
                    connectionString = $"Server={server};Database={database};Trusted_Connection=True;TrustServerCertificate=True;";
                    records = await ListPersonAsync(connectionString, query);
                    ExportState.exportRunning = false;
                }
                catch
                {
                    ExportState.statusExport = false;
                    ExportState.exportRunning = false;
                    ExportState.windowDB = true;
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
    }
    class SortingData
    {
        ///<summary>
        /// E.A.T. 11-February-2025
        /// Data export.
        ///</summary>
        public static async void ExportDataAsync(string? fileName, string? typeFile, string? date, string? fromDate, string? toDate, string? firstName, string? lastName, string? surName, string? city, string? country, bool outDate, bool outFromDate, bool outToDate, bool outFirstName, bool outLastName, bool outSurName, bool outCity, bool outCountry)
        {
            Debug.WriteLine("### Start of method ExportDataAsync ###");
            ExportState.windowDB = false;
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
                FileExporter.CreateFile(fileName, typeFile, out string? fullFileName, records);
            Debug.WriteLine("### End of method ExportDataAsync ###");
        }
    }
    class FileExporter
    {
        ///<summary>
        /// E.A.T. 10-February-2025
        /// Checking the entered word.
        ///</summary>
        public static void CreateFile(string? fileName, string? typeFile, out string? fullFileName, List<Person> records)
        {
            Debug.WriteLine("### Start of method CreateFile ###");
            fullFileName = null;
            if (!string.IsNullOrEmpty(fileName) || !string.IsNullOrEmpty(typeFile))
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
            ExportState.statusExport = false;
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
            ExportState.statusExport = false;
            Debug.WriteLine("### End of method SaveCSVAsync ###");
        }
    }
    class ScreenOutputExport
    {
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
    }
}
