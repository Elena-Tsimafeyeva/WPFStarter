using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Microsoft.Data.SqlClient;
using WPFStarter.ImportAndExport.Export.Interfaces;

namespace WPFStarter.ImportAndExport.Export
{
    public class SqlConnectionFactory : ISqlConnection
    {
        public ISqlConnectionWrapper Create(string connectionString)
        {
            return new SqlConnectionWrapper(connectionString);
        }
    }
}
