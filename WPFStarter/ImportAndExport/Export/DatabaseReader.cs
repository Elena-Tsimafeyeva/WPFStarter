using Microsoft.Data.SqlClient;
using System.Diagnostics;
using System.Configuration;
using System.IO;
using System.Windows;
using WPFStarter.Model;

namespace WPFStarter.ImportAndExport.Export
{
    internal class DatabaseReader
    {
        ///<summary>
        /// E.A.T. 3-February-2025
        /// Outputting data from the DB to the list of objects.
        ///</summary>
        public static async IAsyncEnumerable<List<Person>> ReadDataInChunksAsync(int chunkSize)
        {
            Debug.WriteLine("### Start of method ReadDataInChunksAsync ###");
            ExportState.exportRunning = true;
            string connectionString = ConfigurationManager.ConnectionStrings["MainConnection"].ConnectionString;
            string query = "SELECT Id, Date, FirstName, LastName, SurName, City, Country FROM People";

            if (!await TestConnectionAsync(connectionString))
            {
                try
                {
                    MessageBox.Show("using db file");
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
}
