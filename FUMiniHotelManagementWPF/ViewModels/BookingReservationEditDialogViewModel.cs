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
    public class BookingReservationEditDialogViewModel
    {
        public int CustomerId { get; set; }
        public DateTime? BookingDate { get; set; }
        public decimal? TotalPrice { get; set; }
        public byte? BookingStatus { get; set; }
        public string ErrorMessage { get; set; }


        public ICommand SaveCommand { get; }
        public event Action<bool> RequestClose;

        public BookingReservationEditDialogViewModel(BookingReservation reservation = null, BookingDetail detail = null)
        {
            if (reservation != null)
            {
                CustomerId = reservation.CustomerId;
                BookingDate = reservation.BookingDate.HasValue ? reservation.BookingDate.Value.ToDateTime(TimeOnly.MinValue) : (DateTime?)null;
                TotalPrice = reservation.TotalPrice;
                BookingStatus = reservation.BookingStatus;
            }

            SaveCommand = new RelayCommand(_ => Save());
        }

        private void Save()
        {
            ErrorMessage = string.Empty;
            if (CustomerId <= 0)
                ErrorMessage = "Khách hàng không hợp lệ.";
            else if (BookingDate == null)
                ErrorMessage = "Ngày đặt không hợp lệ.";
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
