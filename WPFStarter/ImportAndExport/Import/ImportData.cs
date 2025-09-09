using WPFStarter.ImportAndExport.Import.Interfaces;
using System.Diagnostics;
using WPFStarter.ProgramLogic.Interfaces;


namespace WPFStarter.ImportAndExport.Import
{
    public class ImportData
    {
        private readonly IMessageBox _messageBox;
        private readonly IDBWriter _dbWriter;
        private readonly ICsvReader _csvReader;
        private readonly IImportStates _importStates;
        public ImportData(IMessageBox messageBox, IDBWriter dbWriter, ICsvReader csvReader, IImportStates importStates)
        {
            _messageBox = messageBox;
            _dbWriter = dbWriter;
            _csvReader = csvReader;
            _importStates = importStates;
        }

        ///<summary>
        /// E.A.T. 25-December-2024
        /// Reading data from a .csv file and transferring it to a list of objects.
        /// Writing data to the database.
        ///</summary>
        public async Task ImportCsvAsync(string filePath)
        {
            Debug.WriteLine("### Start of method ImportCsvAsync ###");
            _importStates.StatusImport = true;
            _importStates.WindowDB = false;
            try
            {
                await foreach (var batch in _csvReader.ReadingDataAsync(filePath, 1000))
                {
                    await _dbWriter.RecordDBAsync(batch);
                }
                _messageBox.Show("Успешно!");
                _importStates.StatusImport = false;
            }
            catch (Exception ex)
            {
                _messageBox.Show($"Ошибка импорта {ex}");
                _importStates.StatusImport = false;
            }
            Debug.WriteLine("### End of method ImportCsvAsync ###");
        }
    }
}
