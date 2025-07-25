using DAL.Entities;
using FUMiniHotelManagementWPF.ViewModels;
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

namespace FUMiniHotelManagementWPF.Views
{
    /// <summary>
    /// Interaction logic for CustomerWindow.xaml
    /// </summary>
    public partial class CustomerWindow : Window
    {
        private Customer _customer;
        public CustomerWindow(Customer customer)
        {
            InitializeComponent();
            _customer = customer;
            WelcomeText.Text = $"Welcome, {customer.CustomerFullName}!";
        }

        private void Account_Click(object sender, RoutedEventArgs e)
        {
            AccountWindow aw = new AccountWindow(_customer);
            aw.ShowDialog();
        }

        private void History_Click(object sender, RoutedEventArgs e)
        {
            HistoryWindow hw = new HistoryWindow(_customer);
            hw.ShowDialog();
        }
        private void Logout_Click(object sender, RoutedEventArgs e)
        {
            var loginWindow = new LoginWindow();
            loginWindow.DataContext = new LoginViewModel(App._authServiceSingleton);
            loginWindow.Show();
            this.Close();
        }
    }
}
