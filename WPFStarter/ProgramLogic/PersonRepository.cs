using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WPFStarter.Model;

namespace WPFStarter.ProgramLogic
{
    internal class PersonRepository
    {
        public static List<Person> FilterPeople(
        List<Person> people,
        string? date, string? fromDate, string? toDate,
        string? firstName, string? lastName, string? surName,
        string? city, string? country,
        bool outDate, bool outFromDate, bool outToDate,
        bool outFirstName, bool outLastName, bool outSurName,
        bool outCity, bool outCountry)
        {
            if (people.Count == 0)
                return new List<Person>();

            IEnumerable<Person> filtered = people;

            if (!string.IsNullOrEmpty(date) && outDate)
            {
                DateTime parsedDate = DateTime.ParseExact(date, "dd.MM.yyyy", CultureInfo.InvariantCulture);
                filtered = filtered.Where(p => p.Date.Date == parsedDate.Date);
            }
            else if (!string.IsNullOrEmpty(fromDate) && !string.IsNullOrEmpty(toDate) && outFromDate && outToDate)
            {
                DateTime from = DateTime.ParseExact(fromDate, "dd.MM.yyyy", CultureInfo.InvariantCulture);
                DateTime to = DateTime.ParseExact(toDate, "dd.MM.yyyy", CultureInfo.InvariantCulture);
                filtered = filtered.Where(p => p.Date >= from && p.Date <= to);
            }

            if (!string.IsNullOrEmpty(firstName) && outFirstName)
                filtered = filtered.Where(p => p.FirstName == firstName);

            if (!string.IsNullOrEmpty(lastName) && outLastName)
                filtered = filtered.Where(p => p.LastName == lastName);

            if (!string.IsNullOrEmpty(surName) && outSurName)
                filtered = filtered.Where(p => p.SurName == surName);

            if (!string.IsNullOrEmpty(city) && outCity)
                filtered = filtered.Where(p => p.City == city);

            if (!string.IsNullOrEmpty(country) && outCountry)
                filtered = filtered.Where(p => p.Country == country);

            return filtered.ToList();
        }
    }
}
