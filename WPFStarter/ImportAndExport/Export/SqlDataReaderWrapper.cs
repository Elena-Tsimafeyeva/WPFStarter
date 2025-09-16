using Microsoft.Data.SqlClient;
using WPFStarter.ImportAndExport.Export.Interfaces;

namespace WPFStarter.ImportAndExport.Export
{
    public class SqlDataReaderWrapper : ISqlDataReaderWrapper
    {
        private readonly SqlDataReader _reader;

        public SqlDataReaderWrapper(SqlDataReader reader)
        {
            _reader = reader;
        }

        public async Task<bool> ReadAsync()
        {
            return await _reader.ReadAsync();
        }

        public object this[string columnName] => _reader[columnName];
    }
}
