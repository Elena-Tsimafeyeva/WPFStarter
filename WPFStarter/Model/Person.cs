using System.ComponentModel;
///<summary>
/// E.A.T. 25-December-2024
/// A data entry class for each person.
/// E.A.T. 26-March-2025
/// Updating the class to use the MVVM pattern.
///</summary>
namespace WPFStarter.Model
{
    internal class Person : INotifyPropertyChanged
    {
        private int id;
        private DateTime date;
        private string firstname;
        private string lastname;
        private string surname;
        private string city;
        private string country;
        public int Id {
            get { return id; }
            set
            {
                id = value;
                OnPropertyChanged(nameof(Id));
            }
        }
        public DateTime Date {
            get { return date; }
            set
            {date = value;
                OnPropertyChanged(nameof(Date));

            } 
        }
        public string FirstName
        {
            get { return firstname; }
            set
            {
                firstname = value;
                OnPropertyChanged(nameof(FirstName));

            }
        }
        public string LastName
        {
            get { return lastname; }
            set
            {
                lastname = value;
                OnPropertyChanged(nameof(LastName));

            }
        }
        public string SurName
        {
            get { return surname; }
            set
            {
                surname = value;
                OnPropertyChanged(nameof(SurName));

            }
        }
        public string City
        {
            get { return city; }
            set
            {
                city = value;
                OnPropertyChanged(nameof(City));

            }
        }
        public string Country
        {
            get { return country; }
            set
            {
                country = value;
                OnPropertyChanged(nameof(Country));

            }
        }

        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }

}
