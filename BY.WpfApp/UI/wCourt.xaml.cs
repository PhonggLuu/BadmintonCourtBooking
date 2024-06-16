using BY.Business;
using BY.Data.Models;
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
using System.Xml.Linq;

namespace BY.WpfApp.UI
{
 
    public partial class wCourt : Window
    {
        private readonly ICourtBusiness _courtBusiness;
        
        public wCourt()
        {
            _courtBusiness = new CourtBusiness();
            InitializeComponent();
            LoadGrdCourt();
         
        }
        

        private async void LoadGrdCourt()
        {
            var result = await _courtBusiness.GetAllCourt();

            if (result.Status > 0 && result.Data != null)
            {
                grdCourt.ItemsSource = result.Data as List<Court>;
            }
            else
            {
                grdCourt.ItemsSource = new List<Court>();
            }

        }

        private async void ButtonSave_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                var item = await _courtBusiness.GetById(int.Parse(txtCourtId.Text));

                if (item.Data == null)
                {
                    var court = new Court()
                    {
                        CourtId = int.Parse(txtCourtId.Text),
                        Name = txtName.Text,
                        Image = txtImage.Text,
                        Description = txtDescription.Text,
                        IsInUse = false 
                    };

                    var result = await _courtBusiness.Save(court);
                    MessageBox.Show(result.Message, "Save");
                }
                else
                {
                    var court = item.Data as Court;
                    court.CourtId = int.Parse(txtCourtId.Text);
                    court.Name = txtName.Text;
                    court.Image = txtImage.Text;
                    court.Description = txtDescription.Text;

                    var result = await _courtBusiness.Update(court);
                    MessageBox.Show(result.Message, "Update");
                }

                txtCourtId.Text = string.Empty;
                txtName.Text = string.Empty;
                txtImage.Text = string.Empty;
                txtDescription.Text = string.Empty;
                this.LoadGrdCourt();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString(), "Error");
            }
        }

        private void ButtonCancel_Click(object sender, RoutedEventArgs e)
        {
            SetValueDefault();
        }
        private void SetValueDefault()
        {
            txtCourtId.Text = "";
            txtImage.Text = "";
            txtDescription.Text = "";
            txtName.Text = "";

          //  txtInIsUse.IsChecked = false;
        }
        private void grdCourt_MouseDouble_Click(object sender, RoutedEventArgs e)
        {
            var court = grdCourt.SelectedItem as Court;
            if (court != null)
            {
                txtCourtId.Text = court.CourtId.ToString();
                txtName.Text=court.Name.ToString();
                txtImage.Text = court.Image.ToString();
                txtDescription.Text = court.Description.ToString();
                txtIsInUse.IsChecked = court.IsInUse;

            }
            else
            {
                MessageBox.Show("Not Found Court", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
  /*      private void IsInUse_Checked(object sender, RoutedEventArgs e)
        {
            CheckBox checkBox = sender as CheckBox;
            if (checkBox != null)
            {
                bool isChecked = checkBox.IsChecked ?? false; // Get the checked state

                // Update a ViewModel property 
                // (Assuming you have a ViewModel with an 'IsInUse' property)
                if (DataContext is YourViewModel viewModel)
                {
                    viewModel.IsInUse = isChecked;
                }
            }
        }*/
        private void grdCourt_SelectionChanged(object sender, RoutedEventArgs e)
        {

        }
        private async void grdCourt_ButtonDelete_Click(object sender, RoutedEventArgs e)
        {
          /*  var schedule = grdSchedule.SelectedItem as Schedule;
            if (schedule != null)
            {
                var result = await _scheduleBusiness.DeleteSchedule(schedule);
                if (result != null && result.Status > 0)
                {
                    LoadGrdSchedule();
                    MessageBox.Show("Delete Schedule successfully", "Information", MessageBoxButton.OK, MessageBoxImage.Information);
                }
                else
                {
                    MessageBox.Show("Delete Schedule Fail", "Error", MessageBoxButton.OK, MessageBoxImage.Error);

                }
            }
            else
            {
                MessageBox.Show("Not Found Schedule", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }*/
        }
    }
}

