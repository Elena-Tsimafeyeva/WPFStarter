
namespace WPFStarter.ImportAndExport.Export.Interfaces
{
    public interface ISqlCommandWrapper
    {
        Task<ISqlDataReaderWrapper> ExecuteReaderAsync();
    }
}
