using Microsoft.VisualBasic.FileIO;
using WPFStarter.Model;

namespace WPFStarter.ImportAndExport.Import
{
    internal class CsvReader
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
}
