using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WPFStarter.Model;

namespace WPFStarter.ProgramLogic.Interfaces
{
    public interface IPersonRepository
    {
        public List<Person> FilterPeople(List<Person> people, string? date, string? fromDate, string? toDate,
        string? firstName, string? lastName, string? surName,
        string? city, string? country,
        bool outDate, bool outFromDate, bool outToDate,
        bool outFirstName, bool outLastName, bool outSurName,
        bool outCity, bool outCountry);
    }
}
