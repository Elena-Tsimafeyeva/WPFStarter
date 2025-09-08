
namespace WPFStarter.ImportAndExport.Import.Interfaces
{
    public interface IImportStates
    {
        bool ImportRunning { get; set; }
        bool StatusImport { get; set; }
        bool WindowDB { get; set; }
    }
}
