using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using System.IO;
using System.Configuration;
using System.Diagnostics;
using WPFStarter.Model;

namespace WPFStarter.Data
{
    ///<summary>
    /// E.A.T. 28-January-2025
    /// Connecting the database.
    ///</summary>
    internal class ApplicationContext : DbContext
    {
        public DbSet<Person> People { get; set; }
        private static string GetConnectionString()
        {
            Debug.WriteLine("### Start of method GetConnectionString ###\n# Attempting to connect to the database. #");
            string mainConnectionString = ConfigurationManager.ConnectionStrings["MainConnection"].ConnectionString;
            try
            {
                using (var connection = new SqlConnection(mainConnectionString))
                {
                    connection.Open();
                    connection.Close();
                    return mainConnectionString;
                }
            }
            catch
            {
                Debug.WriteLine("# If there is an error in the connection string, then an attempt to connect to the database using the data in the db.txt file. #");
                string secondConnectionString = "Server=localhost;Database=People;Trusted_Connection=True;TrustServerCertificate=True;";
                try
                { 
                    string[] elements = File.ReadAllText("db.txt").Split(' ');
                    string server = elements[0];
                    string database = elements[1];
                    secondConnectionString = $"Server={server};Database={database};Trusted_Connection=True;TrustServerCertificate=True;";
                }
                catch {
                    Debug.WriteLine("# There is an error in the connection string. There is an error in the connection file or the data is missing. #");
                }
                Debug.WriteLine("### End of method GetConnectionString ###");
                return secondConnectionString;
            }
        }
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            string connectionString = GetConnectionString();
            optionsBuilder.UseSqlServer(connectionString);
        }
    }
}
