using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;
using System.Configuration;
using WPFStarter.ImportAndExport.Export.Interfaces;

namespace WPFStarter.ImportAndExport.Export
{
    public class ConnectionString: IConnectionString
    {
        public string GetConnectionString(string name) 
        {
            return ConfigurationManager.ConnectionStrings[name]?.ConnectionString
               ?? throw new InvalidOperationException($"Connection string '{name}' not found.");
        }
    }
}
