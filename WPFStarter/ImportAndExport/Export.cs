using Microsoft.Data.SqlClient;
using System.Diagnostics;
using System.IO;
using System.Text;
using System.Windows;
using System.Xml.Linq;
using WPFStarter.ProgramLogic;
using WPFStarter.Model;
using static System.Runtime.InteropServices.JavaScript.JSType;
using System.Diagnostics.Metrics;

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
        ///</summary>
        public static async IAsyncEnumerable<List<Person>> ReadDataInChunksAsync(int chunkSize)
        {
            Debug.WriteLine("### Start of method ReadDataInChunksAsync ###");
            ExportState.exportRunning = true;

            string connectionString = "Server=localhost;Database=Peopl;Trusted_Connection=True;TrustServerCertificate=True;";
            string query = "SELECT Id, Date, FirstName, LastName, SurName, City, Country FROM Table_People_Data";

            if (!await TestConnectionAsync(connectionString))
            {
                try
                {
                    string[] elements = File.ReadAllText("db.txt").Split(' ');
                    string server = elements[0];
                    string database = elements[1];
                    connectionString = $"Server={server};Database={database};Trusted_Connection=True;TrustServerCertificate=True;";

                    if (!await TestConnectionAsync(connectionString))
                    {
                        ExportState.statusExport = false;
                        ExportState.exportRunning = false;
                        ExportState.windowDB = true;
                        yield break;
                    }
                }
                catch (Exception ex)
                {
                    ExportState.statusExport = false;
                    ExportState.exportRunning = false;
                    ExportState.windowDB = true;
                    Debug.WriteLine($"Ошибка при чтении файла db.txt: {ex.Message}");
                    MessageBox.Show($"Ошибка при чтении файла db.txt: {ex.Message}");
                    yield break;
                }
            }

            await foreach (var chunk in StreamPersonChunksAsync(connectionString, query, chunkSize))
            {
                yield return chunk;
            }

            ExportState.exportRunning = false;
            Debug.WriteLine("### End of method ReadDataInChunksAsync ###");
        }

        ///<summary>
        /// E.A.T. 18-April-2025
        /// Recording data about people in "records" of type "List<Person>".
        ///</summary>
        public static async IAsyncEnumerable<List<Person>> StreamPersonChunksAsync(string connectionString, string query, int chunkSize = 3000)
        {
            Debug.WriteLine("### Start of method StreamPersonChunksAsync ###");

            await using var connection = new SqlConnection(connectionString);
            await connection.OpenAsync();

            using var command = new SqlCommand(query, connection);
            using var reader = await command.ExecuteReaderAsync();

            var chunk = new List<Person>(chunkSize);

            while (await reader.ReadAsync())
            {
                var person = new Person
                {
                    Id = (int)reader["Id"],
                    Date = (DateTime)reader["Date"],
                    FirstName = reader["FirstName"].ToString(),
                    LastName = reader["LastName"].ToString(),
                    SurName = reader["SurName"].ToString(),
                    City = reader["City"].ToString(),
                    Country = reader["Country"].ToString()
                };

                chunk.Add(person);

                if (chunk.Count >= chunkSize)
                {
                    yield return chunk;
                    chunk = new List<Person>(chunkSize);
                }
            }

            if (chunk.Count > 0)
            {
                yield return chunk; 
            }

            Debug.WriteLine("### End of method StreamPersonChunksAsync ###");
        }
        ///<summary>
        /// E.A.T. 9-June-2025
        /// Checking connection to the database.
        ///</summary>
        public static async Task<bool> TestConnectionAsync(string connectionString)
        {
            try
            {
                await using var connection = new SqlConnection(connectionString);
                await connection.OpenAsync();
                return true;
            }
            catch (SqlException ex)
            {
                //Debug.WriteLine($"Ошибка подключения к SQL Server: {ex.Message}");
                //MessageBox.Show($"Ошибка подключения: {ex.Message}", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                return false;
            }
            catch (Exception ex)
            {
                //Debug.WriteLine($"Неизвестная ошибка: {ex.Message}");
                //MessageBox.Show($"Неизвестная ошибка: {ex.Message}", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                return false;
            }
        }

    }
    class FileExporter
    {
        ///<summary>
        /// E.A.T. 10-February-2025
        /// Checking the entered word.
        ///</summary>
        public static async Task CreateFile(string? fileName, string? typeFile, string? date, string? fromDate, string? toDate, string? firstName, string? lastName, string? surName, string? city, string? country, bool outDate, bool outFromDate, bool outToDate, bool outFirstName, bool outLastName, bool outSurName, bool outCity, bool outCountry)
        {
            Debug.WriteLine("### Start of method CreateFile ###");
            string? fullFileName = null;
            if (!string.IsNullOrEmpty(fileName) || !string.IsNullOrEmpty(typeFile))
            {
                fullFileName = $"{fileName}{typeFile}";
                await FileAvailability(fullFileName, typeFile, date, fromDate, toDate, firstName, lastName, surName, city, country, outDate, outFromDate, outToDate, outFirstName, outLastName, outSurName, outCity, outCountry);
            }
            Debug.WriteLine("### End of method CreateFile ###");
        }
        ///<summary>
        /// E.A.T. 10-February-2025
        /// Checking that such a file does not exist yet.
        ///</summary>
        public static async Task FileAvailability(string fileName, string? typeFile, string? date, string? fromDate, string? toDate, string? firstName, string? lastName, string? surName, string? city, string? country, bool outDate, bool outFromDate, bool outToDate, bool outFirstName, bool outLastName, bool outSurName, bool outCity, bool outCountry)
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
                    await SaveCSVAsync(fileName, date, fromDate, toDate, firstName, lastName, surName, city, country, outDate, outFromDate, outToDate, outFirstName, outLastName, outSurName, outCity, outCountry);
                    MessageBox.Show("Data saved .CSV");
                    ExportState.statusExport = false;
                }
                else if (typeFile == ".xml")
                {
                    await SaveXMLAsync(fileName, date, fromDate, toDate, firstName, lastName, surName, city, country, outDate, outFromDate, outToDate, outFirstName, outLastName, outSurName, outCity, outCountry);
                    MessageBox.Show("Data saved .XML");
                    ExportState.statusExport = false;
                }
            }
            Debug.WriteLine("### End of method FileAvailability ###");
        }

        ///<summary>
        /// E.A.T. 11-February-2025
        /// Data export to .xml.
        ///</summary>
        public static async Task SaveXMLAsync(string filePath, string? date, string? fromDate, string? toDate, string? firstName, string? lastName, string? surName, string? city, string? country, bool outDate, bool outFromDate, bool outToDate, bool outFirstName, bool outLastName, bool outSurName, bool outCity, bool outCountry)
        {
            Debug.WriteLine("### Start of method SaveXMLAsync ###");
            string rootElementName = "TestProgram";
            XDocument xdoc;
            if (!File.Exists(filePath))
            {
                xdoc = new XDocument(new XDeclaration("1.0", "utf-8", "yes"), new XElement(rootElementName));
            }
            else
            {
                xdoc = XDocument.Load(filePath);
            }
            XElement root = xdoc.Element(rootElementName)!;
            await foreach (var chunk in DatabaseReader.ReadDataInChunksAsync(3000))
            {
                List<Person> filtered = await Program.SortingDataForRecordingAsync(chunk, date, fromDate, toDate, firstName, lastName, surName, city, country, outDate, outFromDate, outToDate, outFirstName, outLastName, outSurName, outCity, outCountry);

                foreach (var person in filtered)
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
                    root.Add(personElement);
                }
            }
            await Task.Run(() => xdoc.Save(filePath));
            Debug.WriteLine("### End of method SaveXMLAsync ###");
        }
        ///<summary>
        /// E.A.T. 12-February-2025
        /// Data export to .csv.
        ///</summary>
        public static async Task SaveCSVAsync(string filePath, string? date, string? fromDate, string? toDate, string? firstName, string? lastName, string? surName, string? city, string? country, bool outDate, bool outFromDate, bool outToDate, bool outFirstName, bool outLastName, bool outSurName, bool outCity, bool outCountry)
        {
            Debug.WriteLine("### Start of method SaveCSVAsync ###");
            using (var writer = new StreamWriter(filePath, false, Encoding.UTF8))
            {
                await foreach (var chunk in DatabaseReader.ReadDataInChunksAsync(3000))
                {
                    List<Person> filtered = await Program.SortingDataForRecordingAsync(chunk, date, fromDate, toDate, firstName, lastName, surName, city, country, outDate, outFromDate, outToDate, outFirstName, outLastName, outSurName, outCity, outCountry);
                    foreach (var person in filtered)
                    {
                        await writer.WriteLineAsync($"{person.Id};{person.Date:yyyy-MM-dd};{person.FirstName};{person.LastName};{person.SurName};{person.City};{person.Country}");
                    }
                }
            }
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
