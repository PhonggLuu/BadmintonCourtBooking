using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using BY.Business;
using BY.Data.Models;


namespace BY.WpfApp.UI
{
    /// <summary>
    /// Interaction logic for wCourt.xaml
    /// </summary>
    public partial class wCourt : Window
    {
        private readonly ICourtBusiness _courtBusiness = new CourtBusiness();
     //  private readonly CourtBusiness courtBusiness;
        public wCourt()
        {
            InitializeComponent();
            DataContext = new Net1704_221_2_BYContext();

        }

        private async void ButtonCancel_Click(object sender, RoutedEventArgs e)
        {
        }
        private async void ButtonSave_Click(object sender, RoutedEventArgs e)
        {
        }
        private async void grdCurrency_ButtonDelete_Click(object sender, RoutedEventArgs e)
        {
        }
        private async void grdCurrency_MouseDouble_Click(object sender, RoutedEventArgs e)
        {
        }
        

        private async void LoadGrdCurrencies()
        {
            var result = await _courtBusiness.GetAllCourt();

            if (result.Status > 0 && result.Data != null)
            {
                grdCurrency.ItemsSource = result.Data as List<Court>;
            }
            else
            {
                grdCurrency.ItemsSource = new List<Court>();
            }
        }
    }
}
