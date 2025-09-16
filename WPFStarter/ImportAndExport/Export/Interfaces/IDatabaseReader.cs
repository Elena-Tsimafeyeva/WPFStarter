using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WPFStarter.Model;

namespace WPFStarter.ImportAndExport.Export.Interfaces
{
    public interface IDatabaseReader
    {
        IAsyncEnumerable<List<Person>> ReadDataInChunksAsync(int chunkSize);
    }
}
