using Microsoft.Data.SqlClient;
using Microsoft.VisualBasic.FileIO;
using System;
using System.Collections.Generic;
using System.Diagnostics.Metrics;
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
            var records = new List<Person>();
            ReadingData(records, filePath);
            RecordDatabase(records);
            OutputDataScreen(records);
        }
        ///<summary>
        /// E.A.T. 29-January-2025
        /// Reading data from a .csv file and transferring it to a list of objects.
        ///</summary>
        public static void ReadingData(List<Person> records, string filePath)
        {
            using TextFieldParser tfp = new(filePath);
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
        }
        ///<summary>
        /// E.A.T. 30-January-2025
        /// Record data to the database.
        ///</summary>
        public static void RecordDatabase(List<Person> records)
        {
            using var context = new ApplicationContext();
            {
                context.Table_People_second.AddRange(records);
                context.SaveChanges();
            }
        }
        ///<summary>
        /// E.A.T. 30-January-2025
        /// Outputting data from a .csv file to the screen.
        ///</summary>
        public static void OutputDataScreen(List<Person> records)
        {
            foreach (var record in records)
            {
                MessageBox.Show($"{record.Date}, {record.FirstName}, {record.LastName}, {record.SurName}, {record.City}, {record.Country}");
            }
            MessageBox.Show("Данные записанны!");
        }
        ///<summary>
        /// E.A.T. 3-February-2025
        /// Outputting data from the DB to the screen.
        ///</summary>
        public static void OutputDataScreenId(List<Person> records)
        {
            foreach (var record in records)
            {
                MessageBox.Show($"{record.Id}, {record.Date}, {record.FirstName}, {record.LastName}, {record.SurName}, {record.City}, {record.Country}");
            }
        }
        //public void ExportData(string fileName) { }
        //public void CreateFile() { }
        ///<summary>
        /// E.A.T. 3-February--2025
        /// Outputting data from the DB to the list of objects.
        ///</summary>
        public static void ReadData(List<Person> records) {
            string connectionString = "Server=localhost;Database=People;Trusted_Connection=True;TrustServerCertificate=True;";
            string query = "SELECT Id, Date, FirstName, LastName, SurName, City, Country FROM  Table_People_second";
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                SqlCommand command = new SqlCommand(query, connection);
                connection.Open();
                SqlDataReader reader = command.ExecuteReader();
                while (reader.Read())
                {
                    Person person = new Person
                    {
                        Id = (int)reader["Id"],
                        Date = (DateTime)reader["Date"],
                        FirstName = reader["FirstName"].ToString(),
                        LastName = reader["LastName"].ToString(),
                        SurName = reader["SurName"].ToString(),
                        City = reader["City"].ToString(),
                        Country = reader["Country"].ToString()

                    };
                    records.Add(person);
                    
                }
                reader.Close();
                MessageBox.Show("Данные из БД Записанны!");
                OutputDataScreenId(records);
            }
            
        }
        //public void SortData() { }
    }
}
