

using System.Diagnostics;
using WPFStarter.ImportAndExport.Export.Interfaces;
using WPFStarter.Model;

namespace WPFStarter.ImportAndExport.Export
{
    public class StreamPersonChunks: IStreamPersonChunks
    {
        private readonly ISqlConnection _connectionFactory;
        public StreamPersonChunks(ISqlConnection connectionFactory)
        {
            _connectionFactory = connectionFactory;
        }

        ///<summary>
        /// E.A.T. 18-April-2025
        /// Recording data about people in "records" of type "List<Person>".
        ///</summary>
        public async IAsyncEnumerable<List<Person>> StreamPersonChunksAsync(string connectionString, string query, int chunkSize = 3000)
        {
            Debug.WriteLine("### Start of method StreamPersonChunksAsync ###");

            var connection = _connectionFactory.Create(connectionString);
            await connection.OpenAsync();

            var command = connection.CreateCommand(query);
            var reader = await command.ExecuteReaderAsync();

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
    }
}
