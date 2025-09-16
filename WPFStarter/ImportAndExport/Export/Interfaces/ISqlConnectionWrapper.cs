
namespace WPFStarter.ImportAndExport.Export.Interfaces
{
    public interface ISqlConnectionWrapper
    {
        Task OpenAsync();
        ISqlCommandWrapper CreateCommand(string query);
    }
}
