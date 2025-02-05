using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.IO;

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
            string? filePath = PathImport.Text;
            FileAvailability(filePath);
        }
        /// <summary>
        /// E.A.T. 20-January-2025
        /// Button event handler for export.
        /// E.A.T. 4-February--2025
        /// Checking the entered data.
        /// </summary>
        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            //Checking the file type.
            string? selectedFileType = null;
            FileType(out selectedFileType);
            MessageBox.Show($"Проверка в Button_Click_1 {selectedFileType}");
            //Sorting the required data
            string? date = Date.Text;
            string? fromDate = FromDate.Text;
            string? toDate = ToDate.Text;
            Program.SortData(date, fromDate, toDate);


        }
        /// <summary>
        /// E.A.T. 22-January-2025
        /// Checking for file availability.
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
                MessageBox.Show($"Тип файла: {selectedFileType}");
            }
            else if (radioButton2.IsChecked == true) {
                selectedFileType = radioButton2.Content.ToString();
                MessageBox.Show($"Тип файла: {selectedFileType}");
            }
        }
    }
}