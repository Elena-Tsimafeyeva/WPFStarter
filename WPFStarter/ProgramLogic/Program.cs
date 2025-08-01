using System.Diagnostics;
using System.Globalization;
using System.Windows;
using WPFStarter.Model;
using WPFStarter.ImportAndExport.Import;
using WPFStarter.ImportAndExport.Export;
using System.IO;
namespace WPFStarter.ProgramLogic
{
    internal class Program
    {

        //public static bool statusExport = true;
        ///<summary>
        /// E.A.T. 4-February-2025
        /// Validation of entered data for sorting. 
        ///</summary>
        //public static async void SortDataAsync(string? date, string? fromDate, string? toDate, string? firstName, string? lastName, string? surName, string? city, string? country, string? fileType, string? fileName)
        //{
        //    Debug.WriteLine("### Start of method SortDataAsync ###");
        //    CheckingDate(date, fromDate, toDate, out bool outDate, out bool outFromDate, out bool outToDate);
        //    CheckingWord(firstName, out bool outFirstName);
        //    CheckingWord(lastName, out bool outLastName);
        //    CheckingWord(surName, out bool outSurName);
        //    CheckingWord(city, out bool outCity);
        //    CheckingWord(country, out bool outCountry);
        //    if(!string.IsNullOrEmpty(date) && outDate == false || !string.IsNullOrEmpty(fromDate) && outFromDate == false || !string.IsNullOrEmpty(toDate) && outToDate == false || !string.IsNullOrEmpty(firstName) && outFirstName == false || !string.IsNullOrEmpty(lastName) && outLastName == false || !string.IsNullOrEmpty(surName) && outSurName == false || !string.IsNullOrEmpty(city) && outCity == false || !string.IsNullOrEmpty(country) && outCountry == false)
        //    {
        //        statusExport = false;
        //        MessageBox.Show("Исправьте данные!");
        //    }
        //    else
        //    {
        //        if (MessageBox.Show($"Вы хотите перенести данные?\nВаши данные:\nДата за {date}\nДата с {fromDate} по {toDate}\nГород {city}\nСтрана {country}\nФамилия {lastName}\nИмя {firstName}\nОтчество{surName}\nТип файла: {fileType}", "Перенос данных", MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.Yes)
        //        {
        //                await FileExporter.CreateFile(fileName, fileType, date, fromDate, toDate, firstName, lastName, surName, city, country, outDate, outFromDate, outToDate, outFirstName, outLastName, outSurName, outCity, outCountry);
        //        }
        //        else
        //        {
        //            statusExport = false;
        //            MessageBox.Show("Исправьте данные!");
        //        }
        //    }
        //    Debug.WriteLine("### End of method SortDataAsync ###");
        //}
        
        
        
        ///<summary>
        /// E.A.T. 11-February-2025
        /// Sorting data for recording.
        ///</summary>
        //public static async Task<List<Person>> SortingDataForRecordingAsync(List<Person> records, string? date, string? fromDate, string? toDate, string? firstName, string? lastName, string? surName, string? city, string? country, bool outDate, bool outFromDate, bool outToDate, bool outFirstName, bool outLastName, bool outSurName, bool outCity, bool outCountry)
        //{
        //    Debug.WriteLine("### Start of method SortingDataForRecordingAsync ###");
        //    var newRecords = new List<Person>();
        //    newRecords = records;

        //    if (records.Count != 0 )
        //    {
        //        if(!string.IsNullOrEmpty(date) && outDate)
        //    {
        //        DateTime parsedDate = DateTime.ParseExact(date, "dd.MM.yyyy", CultureInfo.InvariantCulture);
        //        string stringParsedDate = parsedDate.ToString("yyyy-MM-dd");
        //        DateTime dateFormat = DateTime.ParseExact(stringParsedDate, "yyyy-MM-dd", CultureInfo.InvariantCulture);
        //        newRecords = records
        //            .Where(person => person.Date == dateFormat)
        //            .ToList();
        //    }
        //    else if (!string.IsNullOrEmpty(fromDate) && outFromDate && !string.IsNullOrEmpty(toDate) && outToDate)
        //    {
        //        DateTime parsedFromDate = DateTime.ParseExact(fromDate, "dd.MM.yyyy", CultureInfo.InvariantCulture);
        //        string stringParsedFromDate = parsedFromDate.ToString("yyyy-MM-dd");
        //        DateTime fromDateFormat = DateTime.ParseExact(stringParsedFromDate, "yyyy-MM-dd", CultureInfo.InvariantCulture);
        //        DateTime parsedToDate = DateTime.ParseExact(toDate, "dd.MM.yyyy", CultureInfo.InvariantCulture);
        //        string stringParsedToDate = parsedToDate.ToString("yyyy-MM-dd");
        //        DateTime toDateFormat = DateTime.ParseExact(stringParsedToDate, "yyyy-MM-dd", CultureInfo.InvariantCulture);
        //        newRecords = records
        //            .Where(person => person.Date >= fromDateFormat && person.Date <= toDateFormat)
        //            .ToList();
        //    }
        //    if(!string.IsNullOrEmpty(firstName) && outFirstName)
        //    {
        //        newRecords = records
        //            .Where(person => person.FirstName == firstName)
        //            .ToList();
        //    }
        //    if(!string.IsNullOrEmpty (lastName) && outLastName)
        //    {
        //        newRecords = records
        //            .Where(person => person.LastName == lastName)
        //            .ToList();
        //    }
        //    if(!string.IsNullOrEmpty(surName) && outSurName)
        //    {
        //        newRecords = records
        //            .Where(person => person.SurName == surName)
        //            .ToList();
        //    }
        //    if(!string.IsNullOrEmpty(city) && outCity)
        //    {
        //        newRecords = records
        //            .Where(person => person.City == city)
        //            .ToList();
        //    }
        //    if (!string.IsNullOrEmpty(country) && outCountry)
        //    {
        //        newRecords = records
        //            .Where(person => person.Country == country)
        //            .ToList();
        //    }
        //    }
        //    Debug.WriteLine("### End of method SortingDataForRecordingAsync ###");
        //    return newRecords;
            
        //}
        
    }
}
