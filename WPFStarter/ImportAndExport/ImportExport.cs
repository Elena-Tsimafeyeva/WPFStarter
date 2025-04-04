using Microsoft.Data.SqlClient;
using Microsoft.VisualBasic.FileIO;
using System.IO;
using System.Text;
using System.Windows;
using System.Xml.Linq;
using WPFStarter.ProgramLogic;

namespace WPFStarter
{
    /// <summary>
    /// E.A.T. 24-March-2025
    /// Create the ImportExport class and transfer the necessary methods from the Program class to it.
    /// </summary>
    class ImportExport
    {
        ///<summary>
        /// E.A.T. 25-December-2024
        /// Reading data from a .csv file and transferring it to a list of objects.
        /// Writing data to the database.
        /// E.A.T. 25-March-2025
        /// Adding asynchrony to data import.
        /// E.A.T. 03-April-2025
        /// Updated the use of asynchrony.
        ///</summary>
        public static async Task ImportCsv(string filePath)
        {
            try
            {
                    var records = await ReadingData(filePath);
                    await RecordDatabase(records);
                    MessageBox.Show("Успешно");
                    //OutputDataScreen(records);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка импорта {ex}");
            }
        }
        ///<summary>
        /// E.A.T. 29-January-2025
        /// Reading data from a .csv file and transferring it to a list of objects.
        /// E.A.T. 03-April-2025
        /// Added asynchrony for reading from a .csv file.
        ///</summary>
        public static async Task<List<Person>> ReadingData(string filePath)
        {
            return await Task.Run(() =>
            {
                var records = new List<Person>();
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
                return records;
            });
        }
        ///<summary>
        /// E.A.T. 30-January-2025
        /// Record data to the database.
        /// E.A.T. 03-April-2025
        /// Added asynchrony for writing data to the database.
        ///</summary>
        public static async Task RecordDatabase(List<Person> records)
        {
            using var context = new ApplicationContext();
            {
               await context.Table_People.AddRangeAsync(records);
               await context.SaveChangesAsync();
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
        ///<summary>
        /// E.A.T. 3-February-2025
        /// Outputting data from the DB to the list of objects.
        ///</summary>
        public static void ReadData(out List<Person> records)
        {
            records = new List<Person>();
            string connectionString = "Server=localhost;Database=People;Trusted_Connection=True;TrustServerCertificate=True;";
            string query = "SELECT Id, Date, FirstName, LastName, SurName, City, Country FROM Table_People";
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
               //MessageBox.Show("Данные из БД Записанны!");
            }

        }
        ///<summary>
        /// E.A.T. 10-February-2025
        /// Checking the entered word.
        ///</summary>
        public static void CreateFile(string? fileName, string? typeFile, out string? fullFileName, List<Person> records)
        {
            fullFileName = null;
            if (fileName != "" || typeFile != "")
            {
                fullFileName = $"{fileName}{typeFile}";
                MessageBox.Show($"{fullFileName}");
                FileAvailability(fullFileName, typeFile, records);
            }
        }
        ///<summary>
        /// E.A.T. 10-February-2025
        /// Checking that such a file does not exist yet.
        ///</summary>
        public static void FileAvailability(string fileName, string? typeFile, List<Person> records)
        {
            if (File.Exists(fileName))
            {
                MessageBox.Show($"Файл {fileName} уже есть");
            }
            else
            {
                if (typeFile == ".csv")
                {
                    SaveCSV(fileName, records);
                }
                else if (typeFile == ".xml")
                {
                    SaveXML(fileName, records);
                }
            }
        }
        ///<summary>
        /// E.A.T. 11-February-2025
        /// Data export.
        ///</summary>
        public static void ExportData(string? fileName, string? typeFile, string? date, string? fromDate, string? toDate, string? firstName, string? lastName, string? surName, string? city, string? country, bool outDate, bool outFromDate, bool outToDate, bool outFirstName, bool outLastName, bool outSurName, bool outCity, bool outCountry)
        {
            List<Person> records;
            Program.SortingDataForRecording(out records,
                                    date,
                                    fromDate,
                                    toDate,
                                    firstName,
                                    lastName,
                                    surName,
                                    city,
                                    country,
                                    outDate,
                                    outFromDate,
                                    outToDate,
                                    outFirstName,
                                    outLastName,
                                    outSurName,
                                    outCity,
                                    outCountry);
            CreateFile(fileName, typeFile, out string? fullFileName, records);
        }
        ///<summary>
        /// E.A.T. 11-February-2025
        /// Data export to .xml.
        ///</summary>
        public static void SaveXML(string fileName, List<Person> records)
        {
            XElement testProgramElement = new XElement("TestProgram");

            foreach (var person in records)
            {
                XElement personElement = new XElement("Record",
                    new XAttribute("id", person.Id),
                    new XElement("Date", person.Date.ToString("yyyy-MM-dd")),
                    new XElement("FirstName", person.FirstName),
                    new XElement("LastName", person.LastName),
                    new XElement("SurName", person.SurName),
                    new XElement("City", person.City),
                    new XElement("Country", person.Country)
                );

                testProgramElement.Add(personElement);
            }

            XDocument xdoc = new XDocument(testProgramElement);
            xdoc.Save($"{fileName}");

            MessageBox.Show("Data saved .XML");

        }
        ///<summary>
        /// E.A.T. 12-February-2025
        /// Data export to .csv.
        ///</summary>
        public static void SaveCSV(string filePath, List<Person> records)
        {
            using (StreamWriter writer = new StreamWriter(filePath, false, Encoding.UTF8))
            {
                foreach (var person in records)
                {
                    writer.WriteLine($"{person.Id};{person.Date:yyyy-MM-dd};{person.FirstName};{person.LastName};{person.SurName};{person.City};{person.Country}");
                }
            }
            MessageBox.Show("Data saved .CSV");

        }
    }
}
