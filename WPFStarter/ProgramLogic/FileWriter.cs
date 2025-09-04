using WPFStarter.ProgramLogic.Interfaces;
using System.IO;

namespace WPFStarter.ProgramLogic
{
    public class FileWriter : IFileWriter
    {
        public Task WriteAllTextAsync(string path, string content)
        {
            return File.WriteAllTextAsync(path, content);
        }
    }
}
