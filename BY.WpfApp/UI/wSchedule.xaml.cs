using BY.Business;
using BY.Data.Models;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.ComponentModel;
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

namespace BY.WpfApp.UI
{
    /// <summary>
    /// Interaction logic for wSchedule.xaml
    /// </summary>
    public partial class wSchedule : Window
    {
        private readonly IScheduleBusiness _scheduleBusiness;
        private readonly ICourtBusiness _courtBusiness;
        public wSchedule()
        {
            _scheduleBusiness = new ScheduleBusiness();
            _courtBusiness = new CourtBusiness();
            InitializeComponent();
            LoadGrdSchedule();
            LoadComboBoxCourt();
        }
        private async void LoadGrdSchedule()
        {
            var result = await _scheduleBusiness.GetAllSchedule();

            if (result.Status > 0 && result.Data != null)
            {
                grdSchedule.ItemsSource = result.Data as List<Schedule>;
            }
            else
            {
                grdSchedule.ItemsSource = new List<Schedule>();
            }

        }
        private async void LoadComboBoxCourt()
        {
            var result = await _courtBusiness.GetAllCourt();

            if (result.Status > 0 && result.Data != null)
            {
                txtCourt.ItemsSource = result.Data as List<Court>;
                txtCourt.DisplayMemberPath = "Name";
                txtCourt.SelectedValuePath = "CourtId";
            }
            else
            {
                txtCourt.ItemsSource = new List<Court>();
            }

        }
        private async void ButtonSave_Click(object sender, RoutedEventArgs e)
        {
            var selectedCourt = txtCourt.SelectedItem as Court;
            if (selectedCourt == null)
            {
                MessageBox.Show("Please chose any court", "Warning", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }
            if (!TimeOnly.TryParse(txtFrom.Text, out TimeOnly startTime))
            {
                MessageBox.Show("Please chose Start Time", "Warning", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }
            if (!TimeOnly.TryParse(txtTo.Text, out TimeOnly endTime))
            {
                MessageBox.Show("Please chose End Time ", "Warning", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }
            if (endTime <= startTime)
            {
                MessageBox.Show("Please chose end time great than start time ", "Warning", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }
            if (!decimal.TryParse(txtPrice.Text, out decimal money))
            {
                MessageBox.Show("Please enter money ", "Warning", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }
            if (money <= 0)
            {
                MessageBox.Show("Please money great than zero ", "Warning", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }
            if (!DateTime.TryParse(txtDate.Text, out DateTime startDate))
            {
                MessageBox.Show("Please chose Start Date ", "Warning", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }
            if (startDate.Date < DateTime.Now.Date)
            {
                MessageBox.Show("Please chose current Date or future Date ", "Warning", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }
            if (!int.TryParse(txtScheduleId.Text, out int scheduleId))
            {
                Schedule schedule = new Schedule()
                {
                    CourtId = selectedCourt.CourtId,
                    From = startTime,
                    To = endTime,
                    Price = money,
                    Date = startDate.Date,
                    IsBooked = txtIsBooked.IsChecked
                };
                var result = await _scheduleBusiness.CreateSchedule(schedule);
                if (result != null && result.Status > 0)
                {
                    LoadGrdSchedule();
                    SetValueDefault();
                    MessageBox.Show(result?.Message, "Success", MessageBoxButton.OK, MessageBoxImage.Information);
                }
                else
                {
                    MessageBox.Show("Create fail", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
            else
            {
                var scheduleResult = await _scheduleBusiness.GetScheduleById(scheduleId);
                if (scheduleResult != null && scheduleResult.Status > 0 && scheduleResult.Data != null)
                {
                    var schedule = scheduleResult.Data as Schedule;
                    if (schedule != null)
                    {
                        schedule.ScheduleId = scheduleId;
                        schedule.CourtId = schedule.CourtId;
                        schedule.From = startTime;
                        schedule.To = endTime;
                        schedule.Price = money;
                        schedule.Date = startDate.Date;
                        schedule.IsBooked = txtIsBooked.IsChecked;

                        var result = await _scheduleBusiness.UpdateSchedule(schedule);
                        if (result != null && result.Status > 0)
                        {
                            SetValueDefault();
                            LoadGrdSchedule();
                            MessageBox.Show(result?.Message, "Success", MessageBoxButton.OK, MessageBoxImage.Information);
                        }
                        else
                        {
                            MessageBox.Show("Update fail", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                        }
                    }
                }
            }
        }
        private void ButtonCancel_Click(object sender, RoutedEventArgs e)
        {
            SetValueDefault();
        }
        private void SetValueDefault()
        {
            txtScheduleId.Text = "";
            txtCourt.SelectedItem = null;
            txtFrom.Text = "";
            txtTo.Text = "";
            txtPrice.Text = "";
            txtDate.Text = "";
            txtIsBooked.IsChecked = false;
        }
        private void grdSchedule_MouseDouble_Click(object sender, RoutedEventArgs e)
        {
            var schedule = grdSchedule.SelectedItem as Schedule;
            if (schedule != null)
            {
                txtScheduleId.Text = schedule.ScheduleId.ToString();
                txtCourt.SelectedValue = schedule.CourtId;
                txtFrom.Text = schedule.From.ToString();
                txtTo.Text = schedule.To.ToString();
                txtPrice.Text = schedule.Price.ToString();
                txtDate.Text = schedule.Date.ToString();
                txtIsBooked.IsChecked = schedule.IsBooked;

            }
            else
            {
                MessageBox.Show("Not Found Schedule", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
        private void grdSchedule_SelectionChanged(object sender, RoutedEventArgs e)
        {

        }
        private async void grdSchedule_ButtonDelete_Click(object sender, RoutedEventArgs e)
        {
            var schedule = grdSchedule.SelectedItem as Schedule;
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
            }
        }
    }
}
