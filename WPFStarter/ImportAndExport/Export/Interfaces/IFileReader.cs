using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WPFStarter.ImportAndExport.Export.Interfaces
{
    public interface IFileReader
    {
        string[] ReadAllText(string path);
    }
}
