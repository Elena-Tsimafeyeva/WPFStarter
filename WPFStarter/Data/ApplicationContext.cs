using Microsoft.EntityFrameworkCore;
using WPFStarter.ProgramLogic;

namespace WPFStarter
{
    ///<summary>
    /// E.A.T. 28-January-2025
    /// Connecting the database.
    ///</summary>
    internal class ApplicationContext : DbContext
    {
        public DbSet<Person> Table_People { get; set; }
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer("Server=localhost;Database=People;Trusted_Connection=True;TrustServerCertificate=True;");
        }
    }
}
