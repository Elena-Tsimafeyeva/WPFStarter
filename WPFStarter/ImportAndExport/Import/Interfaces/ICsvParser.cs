
namespace WPFStarter.ImportAndExport.Import.Interfaces
{
    public interface ICsvParser
    {
        bool EndOfData { get; }
        string[]? ReadFields();
    }
}
