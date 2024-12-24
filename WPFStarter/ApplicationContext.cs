using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace WPFStarter
{
    internal class ApplicationContext : DbContext
    {
        public DbSet<Person> Persons { get; set; } = null!;
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer(@"Server=LAPTOP-2CDU469O;Database=People;Trusted_Connection=True;");
        }
    }
}
