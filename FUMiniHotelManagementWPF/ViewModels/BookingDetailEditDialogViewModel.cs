using DAL.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace FUMiniHotelManagementWPF.ViewModels
{
    public class BookingDetailEditDialogViewModel
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

        public BookingDetailEditDialogViewModel(BookingDetail detail = null)
        {
            Rooms = App._roomInformationRepositorySingleton.GetAll().ToList();

            if (detail != null)
            {
                RoomId = detail.RoomId;
                ActualPrice = detail.ActualPrice;
                StartDate = detail.StartDate.ToDateTime(TimeOnly.MinValue);
                EndDate = detail.EndDate.ToDateTime(TimeOnly.MinValue);

            }
            SaveCommand = new RelayCommand(_ => Save());
        }

        private void Save()
        {
            RequestClose?.Invoke(true);
        }

        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
