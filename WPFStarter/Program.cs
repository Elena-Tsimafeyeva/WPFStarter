using Microsoft.VisualBasic.FileIO;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media.Media3D;
///<summary>
/// E.A.T. 25-December-2024
/// Reading data from a .csv file and transferring it to a list of objects.
/// Writing data to the database.
///</summary>
namespace WPFStarter
{
    internal class Program
    {
        public static void ImportCsv(string filePath)
        {
            ///<summary>
            /// E.A.T. 29-January-2025
            /// Reading data from a .csv file and transferring it to a list of objects.
            ///</summary>

            var records = new List<Person>();

            using (TextFieldParser tfp = new TextFieldParser(filePath))
            {
                tfp.TextFieldType = FieldType.Delimited;
                tfp.SetDelimiters(";");

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

                    records.Add(record);
                }
            }
            ///<summary>
            /// E.A.T. 30-January-2025
            /// Writing data to the database.
            ///</summary>
            using (var context = new ApplicationContext())
            {
                context.Table_People_second.AddRange(records);
                context.SaveChanges();
            }
            ///<summary>
            /// E.A.T. 30-January-2025
            /// Outputting data from a .csv file to the screen.
            ///</summary>
            foreach (var record in records)
            {
                MessageBox.Show($"{record.Date}, {record.FirstName}, {record.LastName}, {record.SurName}, {record.City}, {record.Country}");
            }
            MessageBox.Show("Данные записанны!");

        }
    }

    
}
