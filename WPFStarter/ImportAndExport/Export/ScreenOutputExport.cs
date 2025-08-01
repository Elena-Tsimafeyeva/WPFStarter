using System.Diagnostics;
using System.Windows;
using WPFStarter.Model;

namespace WPFStarter.ImportAndExport.Export
{
    internal class ScreenOutputExport
    {
        ///<summary>
        /// E.A.T. 3-February-2025
        /// Outputting data from the DB to the screen.
        ///</summary>
        public static void OutputDataScreenId(List<Person> records)
        {
            Debug.WriteLine("### Start of method OutputDataScreenId ###");
            foreach (var record in records)
            {
                MessageBox.Show($"{record.Id}, {record.Date}, {record.FirstName}, {record.LastName}, {record.SurName}, {record.City}, {record.Country}");
            }
            Debug.WriteLine("### End of method OutputDataScreenId ###");
        }
    }
}
