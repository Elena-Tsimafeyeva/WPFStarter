using System.Diagnostics;
using WPFStarter.ProgramLogic.Interfaces;


namespace WPFStarter.ProgramLogic
{
    public class DbConnectionSettings
    {
        private readonly IFileWriter _fileWriter;

        public DbConnectionSettings(IFileWriter fileWriter)
        {
            _fileWriter = fileWriter;
        }
        ///<summary>
        /// E.A.T. 16-April-2025
        /// Asynchronous writing of "server" and "database" data.
        ///</summary>
        public async Task SaveServerDatabase(string server, string database)
        {
            Debug.WriteLine("### Start of method SaveServerDatabase ###");
            await _fileWriter.WriteAllTextAsync("db.txt", $"{server} {database}");
            Debug.WriteLine("### End of method SaveServerDatabase ###");
        }
    }
}
