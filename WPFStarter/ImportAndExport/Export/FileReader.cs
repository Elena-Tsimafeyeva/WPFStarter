using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WPFStarter.ImportAndExport.Export.Interfaces;

namespace WPFStarter.ImportAndExport.Export
{
    public class FileReader: IFileReader
    {
        public string[] ReadAllText(string path) 
        {
            return File.ReadAllText(path).Split(' ');
        }
    }
}
