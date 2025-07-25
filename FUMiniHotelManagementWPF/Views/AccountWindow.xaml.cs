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
    /// Interaction logic for AccountWindow.xaml
    /// </summary>
    public partial class AccountWindow : Window
    {
        public AccountWindow(Customer customer)
        {
            InitializeComponent();
            var vm = new CustomerProfileEditDialogViewModel(customer);
            DataContext = vm;
            vm.RequestClose += result => { if (result) this.DialogResult = true; };
        }
    }
}
