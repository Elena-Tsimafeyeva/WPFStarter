

namespace WPFStarter.ImportAndExport.Export.Interfaces
{
    public interface ISqlDataReaderWrapper
    {
        Task<bool> ReadAsync();
        object this[string columnName] { get; }
    }
}
