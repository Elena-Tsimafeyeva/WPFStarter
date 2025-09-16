

namespace WPFStarter.ImportAndExport.Export.Interfaces
{
    public interface ITestConnection
    {
        Task<bool> TestConnectionAsync(string connectionString);
    }
}
