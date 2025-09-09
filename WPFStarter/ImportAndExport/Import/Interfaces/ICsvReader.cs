using WPFStarter.Model;

namespace WPFStarter.ImportAndExport.Import.Interfaces
{
    public interface ICsvReader
    {
        IAsyncEnumerable<List<Person>> ReadingDataAsync(string filePath, int batchSize);
    }
}
