using Microsoft.Data.SqlClient;
using Microsoft.VisualBasic;
using Microsoft.VisualBasic.FileIO;
using System;
using System.Collections.Generic;
using System.Diagnostics.Metrics;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media.Animation;
using System.Windows.Media.Media3D;
using System.Windows.Media.TextFormatting;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;
using static System.Runtime.InteropServices.JavaScript.JSType;
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
        ///<summary>
        /// E.A.T. 4-February--2025
        /// Validation of entered data for sorting. 
        ///</summary>
        public static void SortData(string? date, string? fromDate, string? toDate) {
            CheckingDate(date, fromDate, toDate, out bool outDate, out bool outFromDate, out bool outToDate);
            MessageBox.Show($"SortData {outDate},{outFromDate},{outToDate}");
        }
        ///<summary>
        /// E.A.T. 4-February--2025
        /// Checking date format.
        ///</summary>
        public static void SortDate(string? date, out bool outDate)
        {
            outDate = true;
            if(date != null)
            {
                try
                {
                    DateTime dateFormat = DateTime.ParseExact(date, "yyyy-MM-dd", CultureInfo.InvariantCulture);
                    MessageBox.Show($"{dateFormat}");
                }
                catch (FormatException)
                {
                    MessageBox.Show($"Неправильный формат даты.\nДата должна иметь вид: Год-Месяц-День\nПример: 2024-02-01\nВы ввели: {date}");
                    outDate = false;
                }
            }
        }
        ///<summary>
        /// E.A.T. 5-February--2025
        /// Checking date.
        ///</summary>
        public static void CheckingDate(string? date, string? fromDate, string? toDate, out bool outDate, out bool outFromDate, out bool outToDate)
        {
            outDate = false ;
            outFromDate = false ;
            outToDate = false;
            //SortDate(date, out bool outDate);
            //SortDate(fromDate, out bool outFromDate);
            //SortDate(toDate, out bool outToDate);
            //MessageBox.Show($"{outDate}, {outFromDate}, {outToDate}");
            if (date != "" && fromDate != "" || date != "" && toDate != "")
            {
                MessageBox.Show("Вы можете использовать даты для сортировки или 'ЗА Год-Месяц-День' или 'С Год-Месяц-День ПО Год-Месяц-День'.");
            }
            else if (date == "" && fromDate != "" && toDate == "" || date == "" && fromDate == "" && toDate != "")
            {
                MessageBox.Show("Чтобы использовать даты для сортировки 'С Год-Месяц-День ПО Год-Месяц-День',\n Вы должны заполнить оба поля.");
            }
            else if (date != "" && fromDate == "" && toDate == "")
            {
                MessageBox.Show("Сортировка по дате 'ЗА Год-Месяц-День'");
                SortDate(date, out outDate);
            }
            else if (date == "" && fromDate != "" && toDate != "")
            {
                MessageBox.Show("Сортировка по датам 'С Год-Месяц-День ПО Год-Месяц-День'");
                SortDate(fromDate, out outFromDate);
                SortDate(toDate, out outToDate);
            }
        }
        ///<summary>
        /// E.A.T. 7-February--2025
        /// Checking the entered word.
        ///</summary>
        public static void CheckingWord(string? word, out bool outWord)
        {
            outWord = false ;
            if (word != "")
            {
                int lengthWord = word.Length;
                string capitalLetters = "АБВГДЕЁЖЗИЙКЛМНОПРСТУФХЦЧШЩЪЫЬЭЮЯ";
                string lowercaseLetters = "абвгдеёжзийклмнопрстуфхцчшщъыьэюя-";
                int counter = 0;
                for (int i = 0; i<capitalLetters.Length; i++)
                {
                    if(word[0] == capitalLetters[i])
                    {
                        for(int j = 0; j< lowercaseLetters.Length; j++)
                        {
                            for (int k = 0; k< lengthWord; k++)
                            {
                                if (lowercaseLetters[j] == word[k])
                                {
                                    counter++;
                                }
                            }
                        }
                        for (int j = 0; j < capitalLetters.Length; j++)
                        {
                            for (int k = 0; k < lengthWord; k++)
                            {
                                if (capitalLetters[j] == word[k])
                                {
                                    counter++;
                                }
                            }
                        }
                    }
                }
                if (counter == 0)
                {
                    MessageBox.Show($"{word} нужно ввести с большой буквы!");
                }
                else if (counter != lengthWord)
                {
                    MessageBox.Show($"После первой заглавной буквы Вы ввели недопустимый симол или ввели пробел! Вы ввели: {word}");
                }
                else
                {
                    outWord = true;
                }

            }
        }
    }
}
