using System.ComponentModel;
using System.Diagnostics;
using System.Windows;
using System.Windows.Input;
namespace WPFStarter.ViewModel
{
    internal class ConnectingToDBViewModel : INotifyPropertyChanged
    {
        /// <summary>
        /// E.A.T. 14-April-2025
        /// Private fields for storing properties.
        /// </summary>
        private string server;
        private string database;
        private string explanation = " Таблица в БД должна называаться Table_People_Data \n Данные в таблице: \n Id (int, null, Identity(Yes))\n Date (date, null)\n FirstName (nvarchar(50), null)\n LastName (nvarchar(50), null)\n SureName (nvarchar(50), null)\n City (nvarchar(50), null)\n Country (nvarchar(50), null)";
        /// <summary>
        /// E.A.T. 14-April-2025
        /// Public property for accessing and changing the value of Server.
        /// </summary>
        public string Server
        {
            get => server;
            set
            {
                server = value;
                OnPropertyChanged(nameof(Server));
            }
        }
        /// <summary>
        /// E.A.T. 14-April-2025
        /// Public property for accessing and changing the value of Database.
        /// </summary>
        public string Database
        {
            get => database; 
            set
            {
                database = value;
                OnPropertyChanged(nameof(Database));
            }

        }
        /// <summary>
        /// E.A.T. 14-April-2025
        /// Public property for accessing and changing the value of Explanation.
        /// </summary>
        public string Explanation
        {
            get => explanation;
            set
            {
                explanation = value;
                OnPropertyChanged(nameof(Explanation));
            }

        }
        /// <summary>
        /// E.A.T. 16-April-2025
        /// Public property ConnectingCommand representing the command to perform the connecting operation.
        /// </summary>
        public ICommand ConnectingCommand { get; }
        /// <summary>
        /// E.A.T. 16-April-2025
        /// The ViewModel constructor where command id initialized using RelayCommand.
        /// </summary>
        public ConnectingToDBViewModel()
        {
            ConnectingCommand = new RelayCommand(ConnectingToDB);
        }
        /// <summary>
        /// E.A.T. 16-April-2025
        /// Saving the entered data "server" and "database".
        /// </summary>
        public async void ConnectingToDB()
        {
            Debug.WriteLine("### Start of method ConnectingToDB ###");
            string? server = "";
            string? database = "";
            if (Server != null && Database != null)
            {
                server = Server.ToString();
                database = Database.ToString();
                ImportExport.SaveServerDatabase(server, database);
                CloseSpecificWindow();
            }
            Debug.WriteLine("### End of method ConnectingToDB ###");
        }
        /// <summary>
        /// E.A.T. 16-April-2025
        /// Close window.
        /// </summary>
        public static void CloseSpecificWindow()
        {
            Debug.WriteLine("### Start of method CloseSpecificWindow ###");
            var windowToClose = Application.Current.Windows
                .OfType<Window>()
                .FirstOrDefault(w => w.Title == "WindowDataBase");
            windowToClose?.Close();
            Debug.WriteLine("### End of method CloseSpecificWindow ###");
        }
        /// <summary>
        /// E.A.T. 14-April-2025
        /// Method for raising the PropertyChanged event.
        /// Informs(e.g.the UI) that the value of a property has changed.
        /// </summary>
        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
