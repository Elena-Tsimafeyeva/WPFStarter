using System.Windows;
using System.IO;
using WPFStarter.ProgramLogic;
using Microsoft.Win32;

/// <summary>
/// E.A.T. 20-January-2025
/// Interaction logic for MainWindow.xaml
/// </summary>
namespace WPFStarter
{
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();

        }
        /// <summary>
        /// E.A.T. 20-January-2025
        /// Button event handler for import.
        /// </summary>
        private void Button_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Filter = "Text files (*.csv)|*.csv|All files (*.*)|*.*";
            if (openFileDialog.ShowDialog() == true)
            {
                string filePath = openFileDialog.FileName;
                FileAvailability(filePath);
            }
            //string? filePath = PathImport.Text;
            //FileAvailability(filePath);
        }
        /// <summary>
        /// E.A.T. 20-January-2025
        /// Button event handler for export.
        /// E.A.T. 4-February--2025
        /// Checking the entered data.
        /// </summary>
        private void Button_Click_1(object sender, RoutedEventArgs e)
        {

            SaveFileDialog saveFileDialog = new SaveFileDialog();
            if (saveFileDialog.ShowDialog() == true)
            {
                string fileExport = saveFileDialog.FileName;
                // Здесь вы можете добавить код для сохранения данных в выбранный файл
                string? selectedFileType = null;
                FileType(out selectedFileType);
                MessageBox.Show($"Проверка в Button_Click_1 {selectedFileType}");
                //Sorting the required data
                string? date = Date.Text;
                string? fromDate = FromDate.Text;
                string? toDate = ToDate.Text;
                string? firstName = FirstName.Text;
                string? lastName = LastName.Text;
                string? surName = SurName.Text;
                string? city = City.Text;
                string? country = Country.Text;
                //string? fileExport = FileExport.Text;
                Program.SortData(date, fromDate, toDate, firstName, lastName, surName, city, country, selectedFileType, fileExport);
            }
        }
        /// <summary>
        /// E.A.T. 22-January-2025
        /// Checking for file availability (for import).
        /// </summary>
        /// <param name="filePath"></param>
        public static void FileAvailability(string filePath)
        {
            if (File.Exists(filePath))
            {
                MessageBox.Show("Файл существует.");
                Program.ImportCsv(filePath);
            }
            else
            {
                MessageBox.Show("Файл не найден.");
            }
        }
        /// <summary>
        /// E.A.T. 22-January-2025
        /// Checking the file type selection.
        /// </summary>
        public void FileType(out string? selectedFileType) {
            selectedFileType = null;
            if (radioButton1.IsChecked == true)
            {
                selectedFileType = radioButton1.Content.ToString();
                //MessageBox.Show($"Тип файла: {selectedFileType}");
            }
            else if (radioButton2.IsChecked == true) {
                selectedFileType = radioButton2.Content.ToString();
                //MessageBox.Show($"Тип файла: {selectedFileType}");
            }
        }
    }
}