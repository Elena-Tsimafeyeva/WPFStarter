using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace WPFStarter
{
    ///<summary>
    /// E.A.T. 28-January-2025
    /// Connecting the database.
    ///</summary>
    internal class ApplicationContext : DbContext
    {
        public DbSet<Person> Table_People_second { get; set; }
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer("Server=localhost;Database=People;Trusted_Connection=True;TrustServerCertificate=True;");
        }
    }
}
