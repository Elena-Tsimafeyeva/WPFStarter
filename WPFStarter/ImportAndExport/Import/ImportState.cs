

namespace WPFStarter.ImportAndExport.Import
{
    internal class ImportState
    {
        /// <summary>
        /// Added "isImportCsvRunning" to track import activity.
        /// E.A.T. 23-April-2025
        /// Added "statusImport" to enable and disable progressbar.
        /// E.A.T. 21-April-2025
        /// Added "windowDB" to enable and disable the window for entering "server" and "database" data.
        /// </summary>
        public static bool importRunning { get; set; } = false;
        public static bool statusImport { get; set; } = true;
        public static bool windowDB { get; set; } = false;
    }
}
