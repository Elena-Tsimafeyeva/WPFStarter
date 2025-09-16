using Microsoft.Data.SqlClient;
using WPFStarter.ImportAndExport.Export.Interfaces;

namespace WPFStarter.ImportAndExport.Export
{
    public class SqlConnectionWrapper : ISqlConnectionWrapper
    {
        private readonly SqlConnection _connection;

        public SqlConnectionWrapper(string connectionString)
        {
            _connection = new SqlConnection(connectionString);
        }

        public async Task OpenAsync()
        {
            await _connection.OpenAsync();
        }
        public ISqlCommandWrapper CreateCommand(string query)
        {
            var command = new SqlCommand(query, _connection);
            return new SqlCommandWrapper(command);
        }
    }
}
