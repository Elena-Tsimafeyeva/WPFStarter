
using WPFStarter.ProgramLogic.Interfaces;
using Microsoft.Data.SqlClient;
using System.Diagnostics;
using System.Windows;
using WPFStarter.ImportAndExport.Export.Interfaces;

namespace WPFStarter.ImportAndExport.Export
{
    public class TestConnection: ITestConnection
    {
        private readonly IMessageBox _messageBox;
        private readonly ISqlConnection _connectionFactory;
        public TestConnection(IMessageBox messageBox, ISqlConnection connectionFactory)
        {
            _messageBox = messageBox;
            _connectionFactory = connectionFactory;   
        }
        ///<summary>
        /// E.A.T. 9-June-2025
        /// Checking connection to the database.
        ///</summary>
        public async Task<bool> TestConnectionAsync(string connectionString)
        {
            try
            {
                ISqlConnectionWrapper connection = _connectionFactory.Create(connectionString);
                await connection.OpenAsync();
                return true;
            }
            catch (SqlException ex)
            {
                Debug.WriteLine($"Ошибка подключения к SQL Server: {ex.Message}");
                _messageBox.Show($"Ошибка подключения: {ex.Message}");
                return false;
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Неизвестная ошибка: {ex.Message}");
                _messageBox.Show($"Неизвестная ошибка: {ex.Message}");
                return false;
            }
        }
    }
}
