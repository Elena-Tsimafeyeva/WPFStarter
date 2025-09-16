

namespace WPFStarter.ImportAndExport.Export.Interfaces
{
    public interface ISqlConnection
    {
        ISqlConnectionWrapper Create(string connectionString);
    }
}
