

using WPFStarter.ImportAndExport.Import.Interfaces;

namespace WPFStarter.ImportAndExport.Import
{
    internal class ImportState : IImportStates
    {
        /// <summary>
        /// Added "isImportCsvRunning" to track import activity.
        /// E.A.T. 23-April-2025
        /// Added "statusImport" to enable and disable progressbar.
        /// E.A.T. 21-April-2025
        /// Added "windowDB" to enable and disable the window for entering "server" and "database" data.
        /// </summary>
        public bool ImportRunning { get; set; }
        public bool StatusImport { get; set; }
        public bool WindowDB { get; set; }
    }
}
