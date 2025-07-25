using BLL.Services;
using DAL.Entities;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace FUMiniHotelManagementWPF.ViewModels
{
    public class HistoryViewModel : INotifyPropertyChanged
    {
        private readonly BookingService _bookingSerivce;
        public ObservableCollection<BookingReservation> Reservations { get; set; }

        private BookingReservation _selectedReservation;
        public BookingReservation SelectedReservation
        {
            get => _selectedReservation;
            set
            {
                _selectedReservation = value;
                OnPropertyChanged();
                OnPropertyChanged(nameof(SelectedReservation));
            }
        }

        private BookingDetail _selectedDetail;
        public BookingDetail SelectedDetail
        {
            get => _selectedDetail;
            set { _selectedDetail = value; OnPropertyChanged(); }
        }

        public ICommand CloseCommand { get; }

        public HistoryViewModel(Customer customer)
        {
            _bookingSerivce = App._manageBookingServiceSingleton;
            var reservations = _bookingSerivce.GetReservationsByCustomerId(customer.CustomerId);
            Reservations = new ObservableCollection<BookingReservation>(reservations);
            CloseCommand = new RelayCommand(_ => CloseWindow());
        }

        private void CloseWindow()
        {
            foreach (Window window in Application.Current.Windows)
            {
                if (window.DataContext == this)
                {
                    window.Close();
                    break;
                }
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
