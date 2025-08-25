using System.Diagnostics;
using System.IO;


namespace WPFStarter.ProgramLogic
{
    internal class DbConnectionSettings
    {
        ///<summary>
        /// E.A.T. 16-April-2025
        /// Asynchronous writing of "server" and "database" data.
        ///</summary>
        public static async Task SaveServerDatabase(string server, string database)
        {
            Debug.WriteLine("### Start of method SaveServerDatabase ###");
            await File.WriteAllTextAsync("db.txt", $"{server} {database}");
            Debug.WriteLine("### End of method SaveServerDatabase ###");
        }
    }
}
