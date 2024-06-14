using BY.WpfApp.UI;
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

namespace BY.WpfApp
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
        private async void Open_wCourt_Click(object sender, RoutedEventArgs e)
        {
            var p = new wCourt();
            p.Owner = this;
            p.Show();
        }
        private async void Open_wSchedule_Click(object sender, RoutedEventArgs e)
        {
            var p = new wSchedule();
            p.Owner = this;
            p.Show();
        }
    }
}