using WPFStarter.Data;
using WPFStarter.ImportAndExport.Import.Interfaces;
using WPFStarter.Model;

namespace WPFStarter.ImportAndExport.Import
{
    public class DBWriterAppContext : IApplicationContext
    {
        private readonly ApplicationContext _context = new();

        public bool CanConnect() => _context.Database.CanConnect();
        public void AddPeople(IEnumerable<Person> people) => _context.People.AddRange(people);
        public Task SaveChangesAsync() => _context.SaveChangesAsync();
    }
}
