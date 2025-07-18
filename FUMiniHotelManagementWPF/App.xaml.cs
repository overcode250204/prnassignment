using BLL.Services;
using DAL.Entities;
using DAL.Repositories;
using FUMiniHotelManagementWPF.ViewModels;
using FUMiniHotelManagementWPF.Views;
using Microsoft.Extensions.Configuration;
using System.Configuration;
using System.Data;
using System.IO;
using System.Windows;

namespace FUMiniHotelManagementWPF
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        public static FuminiHotelManagementContext _dbContextSingleton { get; private set; }
        public static CustomerRepository _customerRepositorySingleton { get; private set; }
        public static AuthService _authServiceSingleton { get; private set; }
        public static CustomerService _manageCustomerServiceSingleton { get; private set; }
        public static RoomInformationRepository _roomInformationRepositorySingleton { get; private set; }
        public static BookingDetailRepository _bookingDetailRepositorySingleton { get; private set; }
        public static ManageRoomService _manageRoomServiceSingleton { get; private set; }
        public static RoomTypeRepository _roomTypeRepositorySingleton { get; private set; }
        public static BookingReservationRepository _bookingReservationRepositorySingleton { get; private set; }
        public static BookingService _manageBookingServiceSingleton { get; private set; }

        public static BookingDetailService _manageBookingDetailServiceSingleton { get; private set; }


        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);

            var configuration = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
                .Build();




            _dbContextSingleton = new FuminiHotelManagementContext();

            string adminEmail = configuration["AppSettings:AdminAccount:Email"];
            string adminPassword = configuration["AppSettings:AdminAccount:Password"];


            _customerRepositorySingleton = new CustomerRepository(_dbContextSingleton);
            _authServiceSingleton = new AuthService(_customerRepositorySingleton, adminEmail, adminPassword);
            _manageCustomerServiceSingleton = new CustomerService(_customerRepositorySingleton);
            _roomInformationRepositorySingleton = new RoomInformationRepository(_dbContextSingleton);
            _bookingDetailRepositorySingleton = new BookingDetailRepository(_dbContextSingleton);
            _manageRoomServiceSingleton = new ManageRoomService(_roomInformationRepositorySingleton, _bookingDetailRepositorySingleton);
            _roomTypeRepositorySingleton = new RoomTypeRepository(_dbContextSingleton);
            _bookingReservationRepositorySingleton = new BookingReservationRepository(_dbContextSingleton);
            _manageBookingDetailServiceSingleton = new BookingDetailService(_bookingDetailRepositorySingleton);
            _manageBookingServiceSingleton = new BookingService(_bookingReservationRepositorySingleton, _bookingDetailRepositorySingleton);

            var loginWindow = new LoginWindow();
            loginWindow.DataContext = new LoginViewModel(_authServiceSingleton);
            loginWindow.Show();
        }
    }
}
