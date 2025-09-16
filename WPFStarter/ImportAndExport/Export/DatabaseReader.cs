using System.Diagnostics;
using WPFStarter.Model;
using WPFStarter.ImportAndExport.Export.Interfaces;
using WPFStarter.ProgramLogic.Interfaces;

namespace WPFStarter.ImportAndExport.Export
{
    public class DatabaseReader:IDatabaseReader
    {
        private readonly IExportStates _exportStates;
        private readonly IMessageBox _messageBox;
        private readonly ITestConnection _connection;
        private readonly IStreamPersonChunks _streamPersonChunks;
        private readonly IFileReader _reader;
        private readonly IConnectionString _connectionString;
        public DatabaseReader(IExportStates exportStates, IMessageBox messageBox, ITestConnection connection, IStreamPersonChunks streamPersonChunks, IFileReader reader, IConnectionString connectionString)
        {
            _exportStates = exportStates;
            _messageBox = messageBox;
            _connection = connection;
            _streamPersonChunks = streamPersonChunks;
            _reader = reader;
            _connectionString = connectionString;
        }
        ///<summary>
        /// E.A.T. 3-February-2025
        /// Outputting data from the DB to the list of objects.
        ///</summary>
        public async IAsyncEnumerable<List<Person>> ReadDataInChunksAsync(int chunkSize)
        {
            Debug.WriteLine("### Start of method ReadDataInChunksAsync ###");
            _exportStates.ExportRunning = true;
            string connectionString = _connectionString.GetConnectionString("MainConnection");
            string query = "SELECT Id, Date, FirstName, LastName, SurName, City, Country FROM People";

            if (!await _connection.TestConnectionAsync(connectionString))
            {
                try
                {
                    _messageBox.Show("using db file");
                    string[] elements = _reader.ReadAllText("db.txt");
                    string server = elements[0];
                    string database = elements[1];
                    connectionString = $"Server={server};Database={database};Trusted_Connection=True;TrustServerCertificate=True;";

                    if (!await _connection.TestConnectionAsync(connectionString))
                    {
                        _exportStates.StatusExport = false;
                        _exportStates.ExportRunning = false;
                        _exportStates.WindowDB = true;
                        yield break;
                    }
                }
                catch (Exception ex)
                {
                    _exportStates.StatusExport = false;
                    _exportStates.ExportRunning = false;
                    _exportStates.WindowDB = true;
                    Debug.WriteLine($"Ошибка при чтении файла db.txt: {ex.Message}");
                    _messageBox.Show($"Ошибка при чтении файла db.txt: {ex.Message}");
                    yield break;
                }
            }

            await foreach (var chunk in _streamPersonChunks.StreamPersonChunksAsync(connectionString, query, chunkSize))
            {
                Debug.WriteLine($"Chunk size: {chunk.Count}");
                yield return chunk;
            }

            _exportStates.ExportRunning = false;
            Debug.WriteLine("### End of method ReadDataInChunksAsync ###");
        }
    }
}
