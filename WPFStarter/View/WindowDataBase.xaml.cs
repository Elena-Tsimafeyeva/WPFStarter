using MahApps.Metro.Controls;
using WPFStarter.ViewModel;

namespace WPFStarter
{
    /// <summary>
    /// Interaction logic for WindowDataBase.xaml
    /// </summary>
    public partial class WindowDataBase : MetroWindow
    {
        public WindowDataBase()
        {
            InitializeComponent();
            DataContext = new ConnectingToDBViewModel();
        }
    }
}
