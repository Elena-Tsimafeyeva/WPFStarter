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

namespace WPFStarter
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
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
        /// </summary>
        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            FileType();
        }
        /// <summary>
        /// E.A.T. 22-January-2025
        /// Checking for file availability.
        /// </summary>
        /// <param name="filePath"></param>
        private static void FileAvailability(string filePath)
        {
            if (File.Exists(filePath))
            {
                MessageBox.Show("Файл существует.");
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
        private void FileType() { 
        if (radioButton1.IsChecked == true)
            {
                string selectedFileType = radioButton1.Content.ToString();
                MessageBox.Show($"Тип файла: {selectedFileType}");
            }
            else if (radioButton2.IsChecked == true) {
                string selectedFileType = radioButton2.Content.ToString();
                MessageBox.Show($"Тип файла: {selectedFileType}");
            }
        }
    }
}