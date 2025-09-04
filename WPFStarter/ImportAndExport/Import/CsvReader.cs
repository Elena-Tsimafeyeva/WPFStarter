using WPFStarter.ImportAndExport.Import.Interfaces;
using WPFStarter.Model;

namespace WPFStarter.ImportAndExport.Import
{
    public class CsvReader
    {
        private readonly ICsvParser _parser;

        public CsvReader(ICsvParser parser)
        {
            _parser = parser;
        }
        ///<summary>
        /// E.A.T. 29-January-2025
        /// Reading data from a .csv file and transferring it to a list of objects.
        ///</summary>
        public async IAsyncEnumerable<List<Person>> ReadingDataAsync(string filePath, int batchSize)
        {
            List<Person> batch = new();
            while (!_parser.EndOfData)
            {
                var values = _parser.ReadFields();
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
}
