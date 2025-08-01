using System.Diagnostics;
using System.Windows;
using WPFStarter.Model;

namespace WPFStarter.ImportAndExport.Import
{
    internal class ScreenOutputImport
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
