using BY.Business;
using BY.Data.Models;
using Microsoft.IdentityModel.Tokens;
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

namespace BY.WpfApp.UI
{
    /// <summary>
    /// Interaction logic for wCustomer.xaml
    /// </summary>
    public partial class wCustomer : Window
    {
        private readonly ICustomerBusiness _business;
        public wCustomer()
        {
            _business = new CustomerBusiness();
            InitializeComponent();
            LoadGrdCustomer();
        }
        private async void LoadGrdCustomer()
        {
            var result = await _business.GetAll();

            if (result.Status > 0 && result.Data != null)
            {
                grdCustomer.ItemsSource = result.Data as List<Customer>;
            }
            else
            {
                grdCustomer.ItemsSource = new List<Customer>();
            }

        }
        private async void ButtonSave_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (!int.TryParse(txtCustomerId.Text, out int customerId))
                {

                    var customer = new Customer()
                    {
                        Name = txtName.Text,
                        Email = txtEmail.Text,
                        Address = txtAddress.Text,
                        Phone = txtPhone.Text,
                        NumberSlot = int.Parse(txtNumberSlot.Text),
                        YearOfBirth = new DateOnly(YearOfBirth.SelectedDate.Value.Year, YearOfBirth.SelectedDate.Value.Month, YearOfBirth.SelectedDate.Value.Day),
                        RegisterDate = RegisterDate.Value,
                        Gender = Gender.IsChecked.Value,
                        IsActive = IsActive.IsChecked.Value
                    };
                    var result = await _business.Save(customer);
                    MessageBox.Show(result.Message, "Save");
                }
                else
                {
                    var item = await _business.GetById(customerId);
                    if(item != null)
                    {
                        var customer = item.Data as Customer;
                        customer.Name = txtName.Text;
                        customer.Email = txtEmail.Text;
                        customer.Address = txtAddress.Text;
                        customer.Phone = txtPhone.Text;
                        customer.NumberSlot = int.Parse(txtNumberSlot.Text);
                        customer.YearOfBirth = new DateOnly(YearOfBirth.SelectedDate.Value.Year, YearOfBirth.SelectedDate.Value.Month, YearOfBirth.SelectedDate.Value.Day);
                        customer.RegisterDate = RegisterDate.Value;
                        customer.Gender = Gender.IsChecked.Value;
                        customer.IsActive = IsActive.IsChecked.Value;

                        var result = await _business.Update(customer);
                        MessageBox.Show(result?.Message, "Success Update!", MessageBoxButton.OK, MessageBoxImage.Information);
                    }
                    else
                    {
                        MessageBox.Show("Something went wrong", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                    }
                }
                txtCustomerId.Text = string.Empty;
                txtName.Text = string.Empty;
                txtEmail.Text = string.Empty;
                txtAddress.Text = string.Empty;
                txtPhone.Text = string.Empty;
                this.LoadGrdCustomer();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString(), "Error");
            }

        }

        private async void grdCustomer_MouseDouble_Click(object sender, RoutedEventArgs e)
        {
            //MessageBox.Show("Double Click on Grid");
            DataGrid grd = sender as DataGrid;
            if (grd != null && grd.SelectedItems != null && grd.SelectedItems.Count == 1)
            {
                var row = grd.ItemContainerGenerator.ContainerFromItem(grd.SelectedItem) as DataGridRow;
                if (row != null)
                {
                    var item = row.Item as Customer;
                    if (item != null)
                    {
                        var customerResult = await _business.GetById(item.CustomerId);

                        if (customerResult.Status > 0 && customerResult.Data != null)
                        {
                            item = customerResult.Data as Customer;
                            txtCustomerId.Text = item.CustomerId.ToString();
                            txtName.Text = item.Name;
                            txtEmail.Text = item.Email;
                            txtAddress.Text = item.Address;
                            txtPhone.Text = item.Phone;
                            txtNumberSlot.Text = item.NumberSlot.ToString();
                            if (item.YearOfBirth.HasValue)
                            {
                                DateOnly dateOnly = item.YearOfBirth.Value;
                                DateTime dateTime = new DateTime(dateOnly.Year, dateOnly.Month, dateOnly.Day);
                                YearOfBirth.SelectedDate = dateTime;
                            }
                            RegisterDate.Value = item.RegisterDate;
                            Gender.IsChecked = item.Gender;
                            IsActive.IsChecked = item.IsActive;
                        }
                    }
                }
            }
        }

        private async void ButtonCancel_Click(object sender, RoutedEventArgs e)
        {
            txtCustomerId.Text = string.Empty;
            txtName.Text = string.Empty;
            txtEmail.Text = string.Empty;
            txtAddress.Text = string.Empty;
            txtPhone.Text = string.Empty;
        }

        private async void grdCustomer_ButtonDelete_Click(object sender, RoutedEventArgs e)
        {
            Button btn = (Button)sender;

            int customerId = (int)btn.CommandParameter;

            //MessageBox.Show(currencyCode);

            if (customerId > 0)
            {
                if (MessageBox.Show("Do you want to delete this item?", "Delete", MessageBoxButton.YesNo) == MessageBoxResult.Yes)
                {
                    var result = await _business.Delete(customerId);
                    MessageBox.Show($"{result.Message}", "Delete");
                    this.LoadGrdCustomer();
                }
            }
        }
    }
}
