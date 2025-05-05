using System.ComponentModel;

namespace WPFStarter.Model
{
    internal class ConnectingToDB : INotifyPropertyChanged
    {
        private string server;
        private string database;
        public string Server
        {
            get { return server; }
            set
            {
                server = value;
                OnPropertyChanged(nameof(Server));
            }

        }
        public string Database
        {
            get { return database; }
            set
            {
                database = value;
                OnPropertyChanged(nameof(Database));
            }

        }
        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
