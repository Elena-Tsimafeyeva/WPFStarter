using WPFStarter.Model;
using WPFStarter.ProgramLogic;
using System.Globalization;

namespace WPFStarterTests
{
    public class PersonRepositoryTests
    {
        [Fact]
        public void FilterPeople_ByFirstName_ReturnsCorrectResult()
        {
            var people = new List<Person>
            {
                new Person { Id = 1, FirstName = "Иван", LastName = "Иванов", SurName = "Иванович",
                    City = "Гомель", Country = "Беларусь", Date = new DateTime(2024, 2, 1) },
                new Person { Id = 2, FirstName = "Пётр", LastName = "Петров", SurName = "Петрович",
                    City = "Москва", Country = "Россия", Date = new DateTime(2024, 2, 2) }};


            var result = PersonRepository.FilterPeople(
                people,
                date: null, fromDate: null, toDate: null,
                firstName: "Иван", lastName: null, surName: null,
                city: null, country: null,
                outDate: false, outFromDate: false, outToDate: false,
                outFirstName: true, outLastName: false, outSurName: false,
                outCity: false, outCountry: false);

           
            Assert.Single(result);
            Assert.Equal("Иван", result[0].FirstName);
        }
        [Fact]
        public void FilterPeople_ByCity_ReturnsCorrectResult()
        {
            var people = new List<Person>
            {
                new Person { Id = 1, FirstName = "Иван", LastName = "Иванов", SurName = "Иванович",
                    City = "Гомель", Country = "Беларусь", Date = new DateTime(2024, 2, 1) },
                new Person { Id = 2, FirstName = "Пётр", LastName = "Петров", SurName = "Петрович",
                    City = "Москва", Country = "Россия", Date = new DateTime(2024, 2, 2) }};


            var result = PersonRepository.FilterPeople(
                people,
                date: null, fromDate: null, toDate: null,
                firstName: null, lastName: null, surName: null,
                city: "Москва", country: null,
                outDate: false, outFromDate: false, outToDate: false,
                outFirstName: false, outLastName: false, outSurName: false,
                outCity: true, outCountry: false);


            Assert.Single(result);
            Assert.Equal("Москва", result[0].City);
        }
        [Fact]
        public void FilterPeople_ByDate_ReturnsCorrectResult()
        {
            var people = new List<Person>
            {
                new Person { Id = 1, FirstName = "Иван", LastName = "Иванов", SurName = "Иванович",
                    City = "Гомель", Country = "Беларусь", Date = new DateTime(2024, 2, 1) },
                new Person { Id = 2, FirstName = "Пётр", LastName = "Петров", SurName = "Петрович",
                    City = "Москва", Country = "Россия", Date = new DateTime(2024, 2, 2) }};

            DateTime dateFormat = DateTime.ParseExact("2024-02-01", "yyyy-MM-dd", CultureInfo.InvariantCulture);
            string date = dateFormat.ToString("dd.MM.yyyy");

            var result = PersonRepository.FilterPeople(
                people,
                date: date, fromDate: null, toDate: null,
                firstName: null, lastName: null, surName: null,
                city: null, country: null,
                outDate: true, outFromDate: false, outToDate: false,
                outFirstName: false, outLastName: false, outSurName: false,
                outCity: false, outCountry: false);

            Assert.Single(result);
            Assert.Equal("Иван", result[0].FirstName);
        }
    }
}
