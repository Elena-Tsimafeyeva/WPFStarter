using WPFStarter.Model;

namespace WPFStarter.ImportAndExport.Import.Interfaces
{
    public interface IApplicationContext
    {
        bool CanConnect();
        void AddPeople(IEnumerable<Person> people);
        Task SaveChangesAsync();
    }
}
