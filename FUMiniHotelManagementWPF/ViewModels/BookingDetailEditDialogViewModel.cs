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
    public class BookingDetailEditDialogViewModel : INotifyPropertyChanged
    {
        public int RoomId { get; set; }
        public decimal? ActualPrice { get; set; }
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public string ErrorMessage { get; set; }

        public List<RoomInformation> Rooms { get; }

        private RoomInformation _selectedRoom;

        public RoomInformation SelectedRoom
        {
            get => _selectedRoom;
            set
            {
                _selectedRoom = value;
                RoomId = value?.RoomId ?? 0;
                OnPropertyChanged();
                OnPropertyChanged(nameof(RoomId));

            }
        }



        public ICommand SaveCommand { get; }
        public event Action<bool> RequestClose;

        private DateTime? _bookingDate;
        public BookingDetailEditDialogViewModel(BookingDetail detail = null, DateTime? bookingDate = null)
        {
            Rooms = App._roomInformationRepositorySingleton.GetAll().ToList();
            _bookingDate = bookingDate;
            if (detail != null)
            {
                RoomId = detail.RoomId;
                ActualPrice = detail.ActualPrice;
                StartDate = detail.StartDate.ToDateTime(TimeOnly.MinValue);
                EndDate = detail.EndDate.ToDateTime(TimeOnly.MinValue);
                SelectedRoom = Rooms.FirstOrDefault(r => r.RoomId == detail.RoomId);
            }
            SaveCommand = new RelayCommand(_ => Save());
        }

        private void Save()
        {
            ErrorMessage = string.Empty;
            if (StartDate == null)
                ErrorMessage = "Ngày bắt đầu không hợp lệ.";
            else if (EndDate == null)
                ErrorMessage = "Ngày kết thúc không hợp lệ.";
            else if (StartDate >= EndDate)
                ErrorMessage = "Ngày bắt đầu phải nhỏ hơn ngày kết thúc.";
            else if (_bookingDate != null && StartDate <= _bookingDate)
                ErrorMessage = "Ngày bắt đầu phải lớn hơn ngày đặt.";
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
