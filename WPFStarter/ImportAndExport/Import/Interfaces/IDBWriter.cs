using WPFStarter.Model;

namespace WPFStarter.ImportAndExport.Import.Interfaces
{
    public interface IDBWriter
    {
        Task RecordDBAsync(List<Person> batch);
    }
}
