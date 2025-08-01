using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WPFStarter.ProgramLogic
{
    internal class DbConnectionSettings
    {
        ///<summary>
        /// E.A.T. 16-April-2025
        /// Asynchronous writing of "server" and "database" data.
        ///</summary>
        public async Task SaveServerDatabase(string server, string database)
        {
            Debug.WriteLine("### Start of method SaveServerDatabase ###");
            await File.WriteAllTextAsync("db.txt", $"{server} {database}");
            Debug.WriteLine("### End of method SaveServerDatabase ###");
        }
    }
}
