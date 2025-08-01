using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WPFStarter.ProgramLogic;


namespace WPFStarter.ProgramLogic
{
    internal class InputValidator
    {
        public (bool isValid, bool outDate, bool outFromDate, bool outToDate, bool outFirstName, bool outLastName, bool outSurName, bool outCity, bool outCountry)
        Validate(string? date, string? fromDate, string? toDate, string? firstName, string? lastName, string? surName, string? city, string? country)
        {
            var dateValidation = new DateValidation();
            var textValidation = new TextValidation();
            dateValidation.CheckingDate(date, fromDate, toDate, out var outDate, out var outFromDate, out var outToDate);
            textValidation.CheckingWord(firstName, out var outFirstName);
            textValidation.CheckingWord(lastName, out var outLastName);
            textValidation.CheckingWord(surName, out var outSurName);
            textValidation.CheckingWord(city, out var outCity);
            textValidation.CheckingWord(country, out var outCountry);

            bool isValid = (string.IsNullOrEmpty(date) || outDate) &&
                           (string.IsNullOrEmpty(fromDate) || outFromDate) &&
                           (string.IsNullOrEmpty(toDate) || outToDate) &&
                           (string.IsNullOrEmpty(firstName) || outFirstName) &&
                           (string.IsNullOrEmpty(lastName) || outLastName) &&
                           (string.IsNullOrEmpty(surName) || outSurName) &&
                           (string.IsNullOrEmpty(city) || outCity) &&
                           (string.IsNullOrEmpty(country) || outCountry);

            return (isValid, outDate, outFromDate, outToDate, outFirstName, outLastName, outSurName, outCity, outCountry);
        }
    }
}
