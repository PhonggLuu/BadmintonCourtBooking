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
            if (!DateOnly.TryParse(txtDate.Text, out DateOnly startDate))
            {
                MessageBox.Show("Please chose Start Date ", "Warning", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }
            if (startDate < DateOnly.FromDateTime(DateTime.Now))
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
                    Date = startDate,
                    IsBooked = txtIsBooked.IsChecked
                };
                var result = await _scheduleBusiness.CreateSchedule(schedule);
                if (result != null && result.Status > 0)
                {
                    LoadGrdSchedule();
                    SetValueDefault();
                    MessageBox.Show("Create new schedule susscessfully", "Success", MessageBoxButton.OK, MessageBoxImage.Information);
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
                        schedule.Date = startDate;
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
        private async void grdSchedule_MouseDouble_Click(object sender, RoutedEventArgs e)
        {
            DataGrid? grd = sender as DataGrid;
            if (grd != null && grd.SelectedItems != null && grd.SelectedItems.Count == 1)
            {
                var row = grd.ItemContainerGenerator.ContainerFromItem(grd.SelectedItem) as DataGridRow;
                if (row != null)
                {
                    var item = row.Item as Schedule;
                    if (item != null)
                    {
                        var scheduleResult = await _scheduleBusiness.GetScheduleById(item.ScheduleId);
                        if (scheduleResult.Status > 0 && scheduleResult.Data != null)
                        {
                            item = scheduleResult.Data as Schedule;
                            if(item != null)
                            {
                                txtScheduleId.Text = item.ScheduleId.ToString();
                                txtCourt.SelectedValue = item.CourtId;
                                txtFrom.Text = item.From.ToString();
                                txtTo.Text = item.To.ToString();
                                txtPrice.Text = item.Price.ToString();
                                txtDate.Text = item.Date.ToString();
                                txtIsBooked.IsChecked = item.IsBooked;
                            }
                        }
                    }

                }

            }
        }
        private void grdSchedule_SelectionChanged(object sender, RoutedEventArgs e)
        {

        }
        private async void grdSchedule_ButtonDelete_Click(object sender, RoutedEventArgs e)
        {
            Button btn = (Button)sender;
            if (int.TryParse(btn.CommandParameter.ToString(), out int scheduleId))
            {
                var scheduleResult = await _scheduleBusiness.GetScheduleById(scheduleId);
                if (scheduleResult.Status > 0 && scheduleResult.Data != null)
                {   var schedule = scheduleResult.Data as Schedule;
                    if(schedule != null)
                    {
                        if (MessageBox.Show("Are you sure delete", "Confirm", MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.Yes)
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
                    }
                }
            }
            else
            {
                MessageBox.Show("Not Found Schedule", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
    }
}
