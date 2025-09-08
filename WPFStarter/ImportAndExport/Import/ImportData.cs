using WPFStarter.ImportAndExport.Import.Interfaces;
using WPFStarter.ProgramLogic.Services;
using System.Diagnostics;
using System.Windows;


namespace WPFStarter.ImportAndExport.Import
{
    public class ImportData
    {
        ///<summary>
        /// E.A.T. 25-December-2024
        /// Reading data from a .csv file and transferring it to a list of objects.
        /// Writing data to the database.
        ///</summary>
        public static async Task ImportCsvAsync(string filePath)
        {
            Debug.WriteLine("### Start of method ImportCsvAsync ###");
            var importState = new ImportState();
            importState.StatusImport = true;
            importState.WindowDB = false;
            var csvParser = new CsvParser(filePath);
            var csvReader = new CsvReader(csvParser);
            var messageBox = new MessageBoxService();
            var appContext = new DBWriterAppContext();
            var dbWriter = new DBWriter(messageBox, appContext, importState);
            try
            {
                await foreach (var batch in csvReader.ReadingDataAsync(filePath, 1000))
                {
                    await dbWriter.RecordDBAsync(batch);
                }
                MessageBox.Show("Успешно!");
                importState.StatusImport = false;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка импорта {ex}");
                importState.StatusImport = false;
            }
            Debug.WriteLine("### End of method ImportCsvAsync ###");
        }
    }
}
