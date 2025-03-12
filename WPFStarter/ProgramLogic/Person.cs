///<summary>
/// E.A.T. 25-December-2024
/// A data entry class for each person.
///</summary>
namespace WPFStarter.ProgramLogic
{
    internal class Person
    {
        public int Id { get; set; }
        public DateTime Date { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string SurName { get; set; }
        public string City { get; set; }
        public string Country { get; set; }
    }

}
