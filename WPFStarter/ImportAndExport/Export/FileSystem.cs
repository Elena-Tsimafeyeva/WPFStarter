using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WPFStarter.ImportAndExport.Export.Interfaces;
using WPFStarter.Model;
using System.Windows.Shapes;
using System.Xml.Linq;
using System.Diagnostics;

namespace WPFStarter.ImportAndExport.Export
{
    public class FileSystem:IFileSystem
    {
        public bool FileExists(string filePath)
        {
            return File.Exists(filePath);
                
        }
        public async Task WriteTextAsync(string filePath, IAsyncEnumerable<string> lines)
        {
            using (var writer = new StreamWriter(filePath, false, Encoding.UTF8))
            {
                await foreach (var line in lines)
                {
                    Debug.WriteLine($"Writing line: {line}");
                    await writer.WriteLineAsync(line);
                }
            };
        }
        public async Task<XDocument> LoadOrCreateXmlAsync(string filePath, string rootElementName)
        {
            XDocument xdoc;
            if (!File.Exists(filePath))
            {
                xdoc = new XDocument(new XDeclaration("1.0", "utf-8", "yes"), new XElement(rootElementName));
            }
            else
            {
                xdoc = XDocument.Load(filePath);
            }
            return xdoc;
        }
        public async Task SaveXmlAsync(string filePath, XDocument xdoc)
        {
            await Task.Run(() => xdoc.Save(filePath));
        }
    }
}
