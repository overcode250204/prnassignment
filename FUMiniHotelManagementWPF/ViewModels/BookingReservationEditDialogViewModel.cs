using DAL.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace FUMiniHotelManagementWPF.ViewModels
{
    public class BookingReservationEditDialogViewModel : INotifyPropertyChanged
    {
        public int CustomerId { get; set; }
        public DateTime? BookingDate { get; set; }
        public decimal? TotalPrice { get; set; }
        public byte? BookingStatus { get; set; }
        public string ErrorMessage { get; set; }


        public ICommand SaveCommand { get; }
        public event Action<bool> RequestClose;

        private readonly bool _isCreate;
        public List<Customer> Customers { get; set; }
        private Customer _selectedCustomer;
        public Customer SelectedCustomer
        {
            get => _selectedCustomer;
            set
            {
                _selectedCustomer = value;
                CustomerId = value?.CustomerId ?? 0;
                OnPropertyChanged();
                OnPropertyChanged(nameof(CustomerId));
            }
        }

        public BookingReservationEditDialogViewModel(BookingReservation reservation = null, BookingDetail detail = null)
        {
            Customers = App._manageCustomerServiceSingleton.GetAll();
            if (reservation != null)
            {
                CustomerId = reservation.CustomerId;
                SelectedCustomer = Customers.FirstOrDefault(c => c.CustomerId == CustomerId);
                BookingDate = reservation.BookingDate.HasValue ? reservation.BookingDate.Value.ToDateTime(TimeOnly.MinValue) : (DateTime?)null;
                TotalPrice = reservation.TotalPrice;
                BookingStatus = reservation.BookingStatus;
                _isCreate = false;
            }
            else
            {
                BookingDate = DateTime.Today;
                _isCreate = true;
            }
            SaveCommand = new RelayCommand(_ => Save());
        }

        private void Save()
        {
            ErrorMessage = string.Empty;
            if (SelectedCustomer == null)
                ErrorMessage = "Bạn phải chọn khách hàng.";

            else if (TotalPrice == null || TotalPrice < 0)
                ErrorMessage = "Tổng tiền không hợp lệ.";
            else if (BookingStatus == null || BookingStatus < 0)
                ErrorMessage = "Trạng thái không hợp lệ.";

            else
            {
                RequestClose?.Invoke(true);
                return;
            }
            OnPropertyChanged(nameof(ErrorMessage));
            if (!string.IsNullOrEmpty(ErrorMessage))
            {
                MessageBox.Show(ErrorMessage, "Lỗi nhập liệu", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}