using System.Diagnostics;
using WPFStarter.ImportAndExport.Import.Interfaces;
using WPFStarter.Model;
using WPFStarter.ProgramLogic.Interfaces;

namespace WPFStarter.ImportAndExport.Import
{
    public class DBWriter: IDBWriter
    {
        private readonly IMessageBox _messageBox;
        private readonly IApplicationContext _applicationContext;
        private readonly IImportStates _importStates;
        public DBWriter(IMessageBox messageBox, IApplicationContext applicationContext, IImportStates importStates)
        {
            _messageBox = messageBox;
            _applicationContext = applicationContext;
            _importStates = importStates;
        }

        ///<summary>
        /// E.A.T. 30-January-2025
        /// Record data to the database.
        ///</summary>
        public async Task RecordDBAsync(List<Person> records)
        {
            Debug.WriteLine("### Start of method RecordDatabaseAsync ###");
                    try
                    {
                        if (!_applicationContext.CanConnect())
                        {
                            throw new Exception("Подключение к базе данных невозможно. Проверьте строку подключения.");
                        }
                        else
                        {
                            _importStates.ImportRunning = true;
                            await foreach (var batch in GetDataAsync(records, 1000))
                            {
                            _applicationContext.AddPeople(batch);
                            await _applicationContext.SaveChangesAsync();
                            }
                            _importStates.ImportRunning = false;
                        }
                    }
                    catch (Exception ex)
                    {
                        _importStates.StatusImport = false;
                        _importStates.ImportRunning =false;
                        _importStates.WindowDB = true;
                        _messageBox.Show($"Ошибка подключения: {ex.Message}");
                        return;
                    }
            Debug.WriteLine("### End of method RecordDatabaseAsync ###");
        }
        ///<summary>
        /// E.A.T. 02-May-2025
        /// Using asynchronous streams to read data in 1000-record increments..
        ///</summary>
        public async IAsyncEnumerable<List<Person>> GetDataAsync(List<Person> records, int packageSize)
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
