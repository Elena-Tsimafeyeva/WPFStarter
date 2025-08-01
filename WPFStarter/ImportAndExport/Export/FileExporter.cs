using System.Diagnostics;
using System.IO;
using System.Text;
using System.Windows;
using System.Xml.Linq;
using WPFStarter.Model;
using WPFStarter.ProgramLogic;

namespace WPFStarter.ImportAndExport.Export
{
    internal class FileExporter
    {
        ///<summary>
        /// E.A.T. 10-February-2025
        /// Checking the entered word.
        ///</summary>
        public static async Task CreateFile(string? fileName, string? typeFile, string? date, string? fromDate, string? toDate, string? firstName, string? lastName, string? surName, string? city, string? country, bool outDate, bool outFromDate, bool outToDate, bool outFirstName, bool outLastName, bool outSurName, bool outCity, bool outCountry)
        {
            Debug.WriteLine("### Start of method CreateFile ###");
            string? fullFileName = null;
            if (!string.IsNullOrEmpty(fileName) || !string.IsNullOrEmpty(typeFile))
            {
                fullFileName = $"{fileName}{typeFile}";
                await FileAvailability(fullFileName, typeFile, date, fromDate, toDate, firstName, lastName, surName, city, country, outDate, outFromDate, outToDate, outFirstName, outLastName, outSurName, outCity, outCountry);
            }
            Debug.WriteLine("### End of method CreateFile ###");
        }
        ///<summary>
        /// E.A.T. 10-February-2025
        /// Checking that such a file does not exist yet.
        ///</summary>
        public static async Task FileAvailability(string fileName, string? typeFile, string? date, string? fromDate, string? toDate, string? firstName, string? lastName, string? surName, string? city, string? country, bool outDate, bool outFromDate, bool outToDate, bool outFirstName, bool outLastName, bool outSurName, bool outCity, bool outCountry)
        {
            Debug.WriteLine("### Start of method FileAvailability ###");
            if (File.Exists(fileName))
            {
                MessageBox.Show($"Файл {fileName} уже есть");
            }
            else
            {
                if (typeFile == ".csv")
                {
                    await SaveCSVAsync(fileName, date, fromDate, toDate, firstName, lastName, surName, city, country, outDate, outFromDate, outToDate, outFirstName, outLastName, outSurName, outCity, outCountry);
                    MessageBox.Show("Data saved .CSV");
                    ExportState.statusExport = false;
                }
                else if (typeFile == ".xml")
                {
                    await SaveXMLAsync(fileName, date, fromDate, toDate, firstName, lastName, surName, city, country, outDate, outFromDate, outToDate, outFirstName, outLastName, outSurName, outCity, outCountry);
                    MessageBox.Show("Data saved .XML");
                    ExportState.statusExport = false;
                }
            }
            Debug.WriteLine("### End of method FileAvailability ###");
        }

        ///<summary>
        /// E.A.T. 11-February-2025
        /// Data export to .xml.
        ///</summary>
        public static async Task SaveXMLAsync(string filePath, string? date, string? fromDate, string? toDate, string? firstName, string? lastName, string? surName, string? city, string? country, bool outDate, bool outFromDate, bool outToDate, bool outFirstName, bool outLastName, bool outSurName, bool outCity, bool outCountry)
        {
            Debug.WriteLine("### Start of method SaveXMLAsync ###");
            string rootElementName = "TestProgram";
            XDocument xdoc;
            if (!File.Exists(filePath))
            {
                xdoc = new XDocument(new XDeclaration("1.0", "utf-8", "yes"), new XElement(rootElementName));
            }
            else
            {
                xdoc = XDocument.Load(filePath);
            }
            XElement root = xdoc.Element(rootElementName)!;
            var repository = new PersonRepository();
            await foreach (var chunk in DatabaseReader.ReadDataInChunksAsync(3000))
            {
                var filtered = repository.FilterPeople(chunk, date, fromDate, toDate, firstName, lastName, surName, city, country, outDate, outFromDate, outToDate, outFirstName, outLastName, outSurName, outCity, outCountry );
                foreach (var person in filtered)
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
                    root.Add(personElement);
                }
            }
            await Task.Run(() => xdoc.Save(filePath));
            Debug.WriteLine("### End of method SaveXMLAsync ###");
        }
        ///<summary>
        /// E.A.T. 12-February-2025
        /// Data export to .csv.
        ///</summary>
        public static async Task SaveCSVAsync(string filePath, string? date, string? fromDate, string? toDate, string? firstName, string? lastName, string? surName, string? city, string? country, bool outDate, bool outFromDate, bool outToDate, bool outFirstName, bool outLastName, bool outSurName, bool outCity, bool outCountry)
        {
            Debug.WriteLine("### Start of method SaveCSVAsync ###");
            var repository = new PersonRepository();
            using (var writer = new StreamWriter(filePath, false, Encoding.UTF8))
            {
                await foreach (var chunk in DatabaseReader.ReadDataInChunksAsync(3000))
                {
                    var filtered = repository.FilterPeople( chunk, date, fromDate, toDate, firstName, lastName, surName, city, country, outDate, outFromDate, outToDate, outFirstName, outLastName, outSurName, outCity, outCountry);

                    foreach (var person in filtered)
                    {
                        await writer.WriteLineAsync($"{person.Id};{person.Date:yyyy-MM-dd};{person.FirstName};{person.LastName};{person.SurName};{person.City};{person.Country}");
                    }
                }
            }
            Debug.WriteLine("### End of method SaveCSVAsync ###");
        }
    }
}
