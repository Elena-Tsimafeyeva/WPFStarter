using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WPFStarter.ImportAndExport.Export.Interfaces
{
    public interface IExportStates
    {
        bool ExportRunning { get; set; }
        bool StatusExport { get; set; }
        bool WindowDB {  get; set; }
    }
}
