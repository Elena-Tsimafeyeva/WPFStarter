using System.Diagnostics;
using System.Windows;
using WPFStarter.Data;
using WPFStarter.Model;


namespace WPFStarter.ImportAndExport.Import
{
    internal class DBWriter
    {
        ///<summary>
        /// E.A.T. 30-January-2025
        /// Record data to the database.
        ///</summary>
        public static async Task RecordDBAsync(List<Person> records)
        {
            Debug.WriteLine("### Start of method RecordDatabaseAsync ###");
            await Task.Run(async () => {
                using (var context = new ApplicationContext())
                {
                    try
                    {
                        if (!context.Database.CanConnect())
                        {
                            throw new Exception("Подключение к базе данных невозможно. Проверьте строку подключения.");
                        }
                        else
                        {
                            ImportState.importRunning = true;
                            await foreach (var batch in GetDataAsync(records, 1000))
                            {
                                context.People.AddRange(batch);
                                context.SaveChanges();
                            }
                            ImportState.importRunning = false;
                        }
                    }
                    catch (Exception ex)
                    {
                        ImportState.statusImport = false;
                        ImportState.importRunning = false;
                        ImportState.windowDB = true;
                        Debug.WriteLine($"Ошибка подключения: {ex.Message}");
                        MessageBox.Show($"Ошибка подключения: {ex.Message}");
                        return;
                    }
                }
            });
            Debug.WriteLine("### End of method RecordDatabaseAsync ###");
        }
        ///<summary>
        /// E.A.T. 02-May-2025
        /// Using asynchronous streams to read data in 1000-record increments..
        ///</summary>
        public static async IAsyncEnumerable<List<Person>> GetDataAsync(List<Person> records, int packageSize)
        {
            Debug.WriteLine("### Start of method GetDataAsync ###");
            int totalRecords = records.Count;
            List<Person> batch = new List<Person>();
            foreach (var record in records)
            {
                batch.Add(record);
                if (batch.Count == packageSize)
                {
                    yield return batch;
                    batch = new List<Person>();
                    await Task.Delay(500);
                }
            }
            if (batch.Count > 0)
            {
                yield return batch;
            }
            Debug.WriteLine("### End of method GetDataAsync ###");
        }
    }
}
