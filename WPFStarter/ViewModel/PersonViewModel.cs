using ControlzEx.Standard;
using Microsoft.Win32;
using System.ComponentModel;
using System.Diagnostics;
using System.Windows;
using System.Windows.Input;
using WPFStarter.ProgramLogic;
using WPFStarter.ViewModel;

namespace WPFStarter
{
    public class PersonViewModel : INotifyPropertyChanged
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
        private Visibility _elementVisibilityImport = Visibility.Hidden;
        private bool _isWorkingImport;
        private Visibility _elementVisibilityExport = Visibility.Hidden;
        private bool _isWorkingExport;


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
        /// E.A.T. 23-April-2025
        /// Public property for accessing and changing the value of ElementVisibilityImport.
        /// </summary>
        public Visibility ElementVisibilityImport
        {
            get => _elementVisibilityImport;
            set
            {
                if (_elementVisibilityImport != value)
                {
                    _elementVisibilityImport = value;
                    OnPropertyChanged(nameof(ElementVisibilityImport));
                }
            }
        }
        /// <summary>
        /// E.A.T. 23-April-2025
        /// Public property for accessing and changing the value of IsWorkingImport.
        /// </summary>
        public bool IsWorkingImport
        {
            get => _isWorkingImport;
            set
            {
                if (_isWorkingImport != value)
                {
                    _isWorkingImport = value;
                    OnPropertyChanged(nameof(IsWorkingImport));
                }
            }
        }
        /// <summary>
        /// E.A.T. 25-April-2025
        /// Public property for accessing and changing the value of ElementVisibilityExport.
        /// </summary>
        public Visibility ElementVisibilityExport
        {
            get => _elementVisibilityExport;
            set
            {
                if (_elementVisibilityExport != value)
                {
                    _elementVisibilityExport = value;
                    OnPropertyChanged(nameof(ElementVisibilityExport));
                }
            }
        }
        /// <summary>
        /// E.A.T. 25-April-2025
        /// Public property for accessing and changing the value of IsWorkingExport.
        /// </summary>
        public bool IsWorkingExport
        {
            get => _isWorkingExport;
            set
            {
                if (_isWorkingExport != value)
                {
                    _isWorkingExport = value;
                    OnPropertyChanged(nameof(IsWorkingExport));
                }
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
        /// E.A.T. 4-February-2025
        /// Checking the entered data.
        /// E.A.T. 28-March-2025
        /// Moved the method to the PersonViewModels class.
        /// Fixed the method to work correctly with the MVVM pattern.
        /// Added check for file type selection.
        /// E.A.T. 21-April-2025
        /// Open a window to enter the "server" and "database" data if necessary.
        /// E.A.T. 25-April-2025
        /// Enable and disable progressbar during export.
        /// </summary>
        private async void ExportData()
        {
            Debug.WriteLine("### Start of method ExportData ###");
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
                        if (ImportExport.isImportCsvRunning == false && ImportExport.isExportDataRunning == false){

                            StatusWorkExport(true);
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
                            Program.SortDataAsync(date, fromDate, toDate, firstName, lastName, surName, city, country, selectedFileType, fileExport);
                            }
                            else if (ImportExport.isImportCsvRunning == true)
                            {
                                MessageBox.Show("Ожидайте.\nДанные ещё импортируются в БД.");
                            }
                            else
                            {
                                MessageBox.Show("Ожидайте.\nДанные из БД ещё экспортируются.");
                            }
                            while (ImportExport.statusExport&&Program.statusExport)
                            {
                                await Task.Delay(100);
                            }
                            StatusWorkExport(false);
                            if (ImportExport.windowDB == true)
                            {
                                OpenWindowDatabase();
                            }
                        }              
                    }
                }
            catch (Exception ex) {
                MessageBox.Show("error");
            }
            Debug.WriteLine("### End of method ExportData ###");
        }
        /// <summary>
        /// E.A.T. 21-April-2025
        /// Open a window to enter the "server" and "database" data.
        /// </summary>
        private static void OpenWindowDatabase()
        {
            Debug.WriteLine("### Start of method OpenWindowDatabase ###");
            var window = new WindowDataBase();
                window.ShowDialog();
            Debug.WriteLine("### Start of method OpenWindowDatabase ###");
        }
        /// <summary>
        /// E.A.T. 28-March-2025
        /// Blocking unnecessary elements.
        /// </summary>
        private void IsEnabledDates()
        {
            Debug.WriteLine("### Start of method IsEnabledDates ###");
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
            Debug.WriteLine("### End of method IsEnabledDates ###");
        }
        /// <summary>
        /// E.A.T. 20-January-2025
        /// Button event handler for import.
        /// E.A.T. 28-March-2025
        /// Moved the method to the PersonViewModels class.
        /// Fixed the method to work correctly with the MVVM pattern.
        /// E.A.T. 23-April-2025
        /// Enable progressbar during export.
        /// </summary>
        private void ImportCSV()
        {
            Debug.WriteLine("### Start of method ImportCSV ###");
            if (ImportExport.isImportCsvRunning == false && ImportExport.isExportDataRunning == false)
            {
                OpenFileDialog openFileDialog = new OpenFileDialog();
                openFileDialog.Filter = "Text files (*.csv)|*.csv|All files (*.*)|*.*";
            if (openFileDialog.ShowDialog() == true)
            {
                StatusWorkImport(true);
                string filePath = openFileDialog.FileName;
                FileImport = filePath;
                FileAvailability(filePath);
            }
            }
            else if (ImportExport.isImportCsvRunning == true)
            {
                MessageBox.Show("Ожидайте.\nДанные ещё импортируются в БД.");
            }
            else
            {
                MessageBox.Show("Ожидайте.\nДанные из БД ещё экспортируются.");
            }
            Debug.WriteLine("### End of method ImportCSV ###");
        }
        /// <summary>
        /// E.A.T. 22-January-2025
        /// Checking for file availability (for import).
        /// E.A.T. 27-March-2025
        /// Moved the method to the PersonViewModels class.
        /// Fixed the method to work correctly with the MVVM pattern.
        /// E.A.T. 23-April-2025
        /// Disable progressbar after export.
        /// </summary>
        public async void FileAvailability(string filePath)
        {
            Debug.WriteLine("### Start of method FileAvailability ###");
            if (System.IO.File.Exists(filePath))
            {
                ImportExport.ImportCsvAsync(filePath);
                while (ImportExport.statusImport)
                {
                    await Task.Delay(100);
                }
                StatusWorkImport(false);
                if (ImportExport.windowDB == true)
                {
                    OpenWindowDatabase();
                }
            }
            else
            {
                MessageBox.Show("Файл не найден.");
            }
            Debug.WriteLine("### End of method FileAvailability ###");
        }
        /// <summary>
        /// E.A.T. 23-April-2025
        /// Enable and disable progressbar during import.
        /// </summary>
        public void StatusWorkImport(bool progress)
        {
            Debug.WriteLine("### Start of method StatusWorkImport ###");
            if (progress == true)
            {
                ElementVisibilityImport = Visibility.Visible;
                IsWorkingImport = true;
            }
            else
            {
                ElementVisibilityImport = Visibility.Hidden;
                IsWorkingImport = false;
            }
            Debug.WriteLine("### End of method StatusWorkImport ###");
        }
        /// <summary>
        /// E.A.T. 25-April-2025
        /// Enable and disable progressbar during export.
        /// </summary>
        public void StatusWorkExport(bool progress)
        {
            Debug.WriteLine("### Start of method StatusWorkExport ###");
            if (progress == true)
            {
                ElementVisibilityExport = Visibility.Visible;
                IsWorkingExport = true;
            }
            else
            {
                ElementVisibilityExport = Visibility.Hidden;
                IsWorkingExport = false;
            }
            Debug.WriteLine("### End of method StatusWorkExport ###");
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
