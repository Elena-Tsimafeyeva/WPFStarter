using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WPFStarter.ProgramLogic.Interfaces
{
    public interface IFileWriter
    {
        Task WriteAllTextAsync(string path, string content);
    }
}
