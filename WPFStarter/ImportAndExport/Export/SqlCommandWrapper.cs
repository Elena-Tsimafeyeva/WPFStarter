using Microsoft.Data.SqlClient;
using WPFStarter.ImportAndExport.Export.Interfaces;

namespace WPFStarter.ImportAndExport.Export
{
    public class SqlCommandWrapper : ISqlCommandWrapper
    {
        private readonly SqlCommand _command;

        public SqlCommandWrapper(SqlCommand command)
        {
            _command = command;
        }

        public async Task<ISqlDataReaderWrapper> ExecuteReaderAsync()
        {
            var reader = await _command.ExecuteReaderAsync();
            return new SqlDataReaderWrapper(reader);
        }
    }
}
