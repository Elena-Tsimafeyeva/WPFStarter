using Microsoft.VisualBasic.FileIO;
using System.Diagnostics;
using System.Windows;
using WPFStarter.Model;
using WPFStarter.Data;

namespace WPFStarter.ImportAndExport
{
    class ImportState
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
    class CsvReader
    {
        ///<summary>
        /// E.A.T. 29-January-2025
        /// Reading data from a .csv file and transferring it to a list of objects.
        ///</summary>
        public static async IAsyncEnumerable<List<Person>> ReadingDataAsync(string filePath, int batchSize)
        {
            using TextFieldParser tfp = new(filePath)
            {
                TextFieldType = FieldType.Delimited,
            };
            tfp.SetDelimiters(";");

            List<Person> batch = new();
            while (!tfp.EndOfData)
            {
                var values = tfp.ReadFields();
                var record = new Person
                {
                    Date = DateTime.Parse(values[0]),
                    FirstName = values[1],
                    LastName = values[2],
                    SurName = values[3],
                    City = values[4],
                    Country = values[5]
                };
                batch.Add(record);

                if (batch.Count == batchSize)
                {
                    yield return batch;
                    batch = new List<Person>();
                    await Task.Yield();
                }
            }
            if (batch.Count > 0)
                yield return batch;
        }
    }
    class DBWriter
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
                                context.Table_People_Data.AddRange(batch);
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
    class ImportData
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
            try
            {
                await foreach (var batch in CsvReader.ReadingDataAsync(filePath, 1000))
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
    class ScreenOutputImport
    {
        ///<summary>
        /// E.A.T. 30-January-2025
        /// Outputting data from a .csv file to the screen.
        ///</summary>
        public static void OutputDataScreen(List<Person> records)
        {
            Debug.WriteLine("### Start of method OutputDataScreen ###");
            foreach (var record in records)
            {
                MessageBox.Show($"{record.Date}, {record.FirstName}, {record.LastName}, {record.SurName}, {record.City}, {record.Country}");
            }
            MessageBox.Show("Данные записанны!");
            Debug.WriteLine("### End of method OutputDataScreen ###");
        }
    }
}
