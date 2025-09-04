using WPFStarter.ImportAndExport.Import.Interfaces;
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
            ImportState.statusImport = true;
            ImportState.windowDB = false;
            var csvParser = new CsvParser(filePath);
            var csvReader = new CsvReader(csvParser);
            try
            {
                await foreach (var batch in csvReader.ReadingDataAsync(filePath, 1000))
                {
                    await DBWriter.RecordDBAsync(batch);
                }
                MessageBox.Show("Успешно!");
                ImportState.statusImport = false;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка импорта {ex}");
                ImportState.statusImport = false;
            }
            Debug.WriteLine("### End of method ImportCsvAsync ###");
        }
    }
}
