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
    /// Interaction logic for AdminBookingManagement.xaml
    /// </summary>
    public partial class AdminBookingManagement : Window
    {
        public AdminBookingManagement()
        {
            InitializeComponent();
            DataContext = new AdminBookingManagementViewModel(App._manageBookingServiceSingleton, App._manageBookingDetailServiceSingleton, App._manageCustomerServiceSingleton);
        }
    }
}
