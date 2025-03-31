using Microsoft.Win32;
using System.ComponentModel;
using System.Windows;
using System.Windows.Input;
using WPFStarter.ProgramLogic;
using WPFStarter.ViewModel;

namespace WPFStarter
{
    class PersonViewModel : INotifyPropertyChanged
    {
        /// <summary>
        /// E.A.T. 26-March-2025
        /// Private fields for storing properties.
        /// </summary>
        private string firstName;
        private string lastName;
        private string surName;
        private string city;
        private string country;
        private DateTime? date;
        private DateTime? fromDate;
        private DateTime? toDate;
        private bool isEnabledDate = true;
        private bool isEnabledFromDate = true;
        private bool isEnabledToDate = true;
        private string fileExport = "Здесь будет путь файла";
        private string fileImport = "Здесь будет путь файла";
        private bool typeXML;
        private bool typeCSV = true;


        /// <summary>
        /// E.A.T. 26-March-2025
        /// Public property for accessing and changing the value of firstName.
        /// </summary>
        public string FirstName
        {
            get => firstName;
            set
            {
                firstName = value;
                OnPropertyChanged(nameof(FirstName));
            }
        }
        /// <summary>
        /// E.A.T. 26-March-2025
        /// Public property for accessing and changing the value of lastName.
        /// </summary>
        public string LastName
        {
            get => lastName;
            set
            {
                lastName = value;
                OnPropertyChanged(nameof(LastName));
            }
        }
        /// <summary>
        /// E.A.T. 26-March-2025
        /// Public property for accessing and changing the value of surName.
        /// </summary>
        public string SurName
        {
            get => surName;
            set
            {
                surName = value;
                OnPropertyChanged(nameof(SurName));
            }
        }
        /// <summary>
        /// E.A.T. 26-March-2025
        /// Public property for accessing and changing the value of city.
        /// </summary>
        public string City
        {
            get => city;
            set
            {
                city = value;
                OnPropertyChanged(nameof(City));
            }
        }
        /// <summary>
        /// E.A.T. 26-March-2025
        /// Public property for accessing and changing the value of country.
        /// </summary>
        public string Country
        {
            get => country;
            set
            {
                country = value;
                OnPropertyChanged(nameof(Country));
            }
        }
        /// <summary>
        /// E.A.T. 26-March-2025
        /// Public property for accessing and changing the value of date.
        /// </summary>
        public DateTime? Date
        {
            get => date;
            set
            {
                date = value;
                OnPropertyChanged(nameof(Date));
                IsEnabledDates();
            }
        }
        /// <summary>
        /// E.A.T. 26-March-2025
        /// Public property for accessing and changing the value of fromDate.
        /// </summary>
        public DateTime? FromDate
        {
            get => fromDate;
            set
            {
                fromDate = value;
                OnPropertyChanged(nameof(FromDate));
                IsEnabledDates();
            }
        }
        /// <summary>
        /// E.A.T. 26-March-2025
        /// Public property for accessing and changing the value of toDate.
        /// </summary>
        public DateTime? ToDate
        {
            get => toDate;
            set
            {
                toDate = value;
                OnPropertyChanged(nameof(ToDate));
                IsEnabledDates();
            }
        }
        /// <summary>
        /// E.A.T. 28-March-2025
        /// Public property for accessing and changing the value of isEnabledDate.
        /// </summary>
        public bool IsEnabledDate
        {
            get => isEnabledDate;
            set
            {
                isEnabledDate = value;
                OnPropertyChanged(nameof(IsEnabledDate));
            }

        }
        /// <summary>
        /// E.A.T. 28-March-2025
        /// Public property for accessing and changing the value of isEnabledFromDate.
        /// </summary>
        public bool IsEnabledFromDate
        {
            get => isEnabledFromDate;
            set
            {
                isEnabledFromDate = value;
                OnPropertyChanged(nameof(IsEnabledFromDate));
            }

        }
        /// <summary>
        /// E.A.T. 28-March-2025
        /// Public property for accessing and changing the value of isEnabledToDate.
        /// </summary>
        public bool IsEnabledToDate
        {
            get => isEnabledToDate;
            set
            {
                isEnabledToDate = value;
                OnPropertyChanged(nameof(IsEnabledToDate));
            }

        }
        /// <summary>
        /// E.A.T. 27-March-2025
        /// Public property for accessing and changing the value of fileExport.
        /// </summary>
        public string FileExport
        {
            get => fileExport;
            set
            {
                fileExport = value;
                OnPropertyChanged(nameof(FileExport));
            }
        }
        /// <summary>
        /// E.A.T. 27-March-2025
        /// Public property for accessing and changing the value of fileImport.
        /// </summary>
        public string FileImport
        {
            get => fileImport;
            set
            {
                fileImport = value;
                OnPropertyChanged(nameof(FileImport));
            }
        }
        /// <summary>
        /// E.A.T. 28-March-2025
        /// Public property for accessing and changing the value of typeXML.
        /// </summary>
        public bool TypeXML
        {
            get => typeXML;
            set
            {
                typeXML = value;
                OnPropertyChanged(nameof(typeXML));
            }
        }
        /// <summary>
        /// E.A.T. 28-March-2025
        /// Public property for accessing and changing the value of typeCSV.
        /// </summary>
        public bool TypeCSV
        {
            get => typeCSV;
            set
            {
                typeCSV = value;
                OnPropertyChanged(nameof(typeCSV));
            }
        }
        /// <summary>
        /// E.A.T. 27-March-2025
        /// Public property ExportCommand representing the command to perform the data export operation.
        /// </summary>
        public ICommand ExportCommand { get; }
        /// <summary>
        /// E.A.T. 27-March-2025
        /// The ImportCommand public property represents the command to perform the data import operation.
        /// </summary>
        public ICommand ImportCommand { get; }
        /// <summary>
        /// E.A.T. 27-March-2025
        /// The ViewModel constructor where commands are initialized using RelayCommand.
        /// </summary>

        public PersonViewModel()
        {
            ExportCommand = new RelayCommand(ExportData);
            ImportCommand = new RelayCommand(ImportCSV);
        }
        /// <summary>
        /// E.A.T. 20-January-2025
        /// Button event handler for export.
        /// E.A.T. 4-February--2025
        /// Checking the entered data.
        /// E.A.T. 28-March-2025
        /// Moved the method to the PersonViewModels class.
        /// Fixed the method to work correctly with the MVVM pattern.
        /// Added check for file type selection.
        /// </summary>
        private void ExportData()
        {
            try
            {
                string? selectedFileType = null;
                if (typeCSV == true){
                    selectedFileType = ".csv";
                }else if (typeXML == true){
                    selectedFileType = ".xml";
                }
                if (selectedFileType != null)
                {
                    
                    
                    SaveFileDialog saveFileDialog = new SaveFileDialog();
                    if (saveFileDialog.ShowDialog() == true)
                    {
                        string fileExport = saveFileDialog.FileName;
                        FileExport = fileExport + selectedFileType;

                        string? date = "";
                        string? fromDate = "";
                        string? toDate = "";
                        string? firstName = "";
                        string? lastName = "";
                        string? surName = "";
                        string? city = "";
                        string? country = "";
                        if (Date.ToString() != "")
                        {
                            date = Date.ToString().Substring(0, 10);
                        }
                        if (FromDate.ToString() != "")
                        {
                            fromDate = FromDate.ToString().Substring(0, 10);
                        }
                        if (ToDate.ToString() != "")
                        {
                            toDate = ToDate.ToString().Substring(0, 10);
                        }
                        if (FirstName != null)
                        {
                            firstName = FirstName.ToString();
                        }
                        if (LastName != null)
                        {
                            lastName = LastName.ToString();
                        }
                        if (SurName != null)
                        {
                            surName = SurName.ToString();
                        }
                        if (City != null)
                        {
                           city = City.ToString();
                        }
                        if (Country != null)
                        {
                            country = Country.ToString();
                        }
                        Program.SortData(date, fromDate, toDate, firstName, lastName, surName, city, country, selectedFileType, fileExport);
                    }              
                }
                
            }
            catch (Exception ex) {
                MessageBox.Show("error");
            }


        }
        /// <summary>
        /// E.A.T. 28-March-2025
        /// Blocking unnecessary elements.
        /// </summary>
        private void IsEnabledDates()
        {
            if (Date.ToString() != "")
            {
                IsEnabledFromDate = false;
                IsEnabledToDate = false;
            }
            else if (FromDate.ToString() != "" || ToDate.ToString() != "")
            {
                IsEnabledDate = false;
            }
            else
            {
                IsEnabledDate = true;
                IsEnabledFromDate = true;
                IsEnabledToDate = true;

            }
        }
        /// <summary>
        /// E.A.T. 20-January-2025
        /// Button event handler for import.
        /// E.A.T. 28-March-2025
        /// Moved the method to the PersonViewModels class.
        /// Fixed the method to work correctly with the MVVM pattern.
        /// </summary>
        private void ImportCSV()
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Filter = "Text files (*.csv)|*.csv|All files (*.*)|*.*";
            if (openFileDialog.ShowDialog() == true)
            {
                string filePath = openFileDialog.FileName;
                FileImport = filePath;
                FileAvailability(filePath);
            }
        }
        /// <summary>
        /// E.A.T. 22-January-2025
        /// Checking for file availability (for import).
        /// E.A.T. 27-March-2025
        /// Moved the method to the PersonViewModels class.
        /// Fixed the method to work correctly with the MVVM pattern.
        /// </summary>
        public static void FileAvailability(string filePath)
        {
            if (System.IO.File.Exists(filePath))
            {
                MessageBox.Show("Файл существует.");
                ImportExport.ImportCsv(filePath);
            }
            else
            {
                MessageBox.Show("Файл не найден.");
            }
        }
        /// <summary>
        /// E.A.T. 26-March-2025
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
