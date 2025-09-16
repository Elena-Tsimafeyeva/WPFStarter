

using WPFStarter.Model;

namespace WPFStarter.ImportAndExport.Export.Interfaces
{
    public interface IStreamPersonChunks
    {
        IAsyncEnumerable<List<Person>> StreamPersonChunksAsync(string connectionString, string query, int chunkSize);
    }
}
