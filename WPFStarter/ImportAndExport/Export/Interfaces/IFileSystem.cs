using System.Xml.Linq;

namespace WPFStarter.ImportAndExport.Export.Interfaces
{
    public interface IFileSystem
    {
        bool FileExists(string filePath);
        Task WriteTextAsync(string path, IAsyncEnumerable<string> content);
        Task<XDocument> LoadOrCreateXmlAsync(string filePath, string rootElement);
        Task SaveXmlAsync(string filePath, XDocument doc);
    }
}
