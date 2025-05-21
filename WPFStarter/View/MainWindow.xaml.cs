using MahApps.Metro.Controls;
using WPFStarter.ViewModel;

/// <summary>
/// E.A.T. 20-January-2025
/// Interaction logic for MainWindow.xaml
/// E.A.T. 26-March-2025
/// Move logic to PersonViewMain.
/// </summary>
namespace WPFStarter
{
    public partial class MainWindow : MetroWindow
    {
        public MainWindow()
        {
            InitializeComponent();
            DataContext = new PersonViewModel();
        }
    }
}