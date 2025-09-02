

namespace WPFStarter.ProgramLogic.Interfaces
{
    ///<summary>
    /// E.A.T. 25-August-2025
    /// Interface for checking input data according to specified parameters.
    ///</summary>
    public interface IInputValidator
    {
        ValidationResult Validate(string? date, string? fromDate, string? toDate, string? firstName, string? lastName, string? surName, string? city, string? country);
    }
}
