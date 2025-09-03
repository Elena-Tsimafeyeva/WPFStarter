using WPFStarter.ProgramLogic.Interfaces;
using WPFStarter.ProgramLogic.Services;


namespace WPFStarter.ProgramLogic
{
    ///<summary>
    /// E.A.T. 25-August-2025
    /// Implementation of the IInputValidator interface to check the correctness of input data: dates, names and geographic parameters.
    ///</summary>
    public class InputValidator : IInputValidator
    {
        public ValidationResult Validate(string? date, string? fromDate, string? toDate, string? firstName, string? lastName, string? surName, string? city, string? country)
        {
            var messageBox = new MessageBoxService();
            var dateValidation = new DateValidation(messageBox);
            var textValidation = new TextValidation(messageBox);
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

            return new ValidationResult
            {
                IsValid = isValid,
                OutDate = outDate,
                OutFromDate = outFromDate,
                OutToDate = outToDate,
                OutFirstName = outFirstName,
                OutLastName = outLastName,
                OutSurName = outSurName,
                OutCity = outCity,
                OutCountry = outCountry
            };
        }
    }
}
