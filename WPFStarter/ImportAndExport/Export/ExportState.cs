

using WPFStarter.ImportAndExport.Export.Interfaces;

namespace WPFStarter.ImportAndExport.Export
{
    public class ExportState: IExportStates
    {
        ///<summary>
        /// Added "isExportCsvRunning" to track export activity.
        /// E.A.T. 25-April-2025
        /// Added "statusExport" to enable and disable progressbar.
        /// E.A.T. 21-April-2025
        /// Added "windowDB" to enable and disable the window for entering "server" and "database" data.
        ///</summary>
        public bool ExportRunning { get; set; } = false;
        public bool StatusExport { get; set; } = true;
        public bool WindowDB { get; set; } = false;
    }
}
