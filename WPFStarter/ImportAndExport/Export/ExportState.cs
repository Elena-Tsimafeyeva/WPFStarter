

namespace WPFStarter.ImportAndExport.Export
{
    internal class ExportState
    {
        ///<summary>
        /// Added "isExportCsvRunning" to track export activity.
        /// E.A.T. 25-April-2025
        /// Added "statusExport" to enable and disable progressbar.
        /// E.A.T. 21-April-2025
        /// Added "windowDB" to enable and disable the window for entering "server" and "database" data.
        ///</summary>
        public static bool exportRunning { get; set; } = false;
        public static bool statusExport { get; set; } = true;
        public static bool windowDB { get; set; } = false;
    }
}
