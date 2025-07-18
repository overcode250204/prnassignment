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
    public class RoomEditDialogViewModel
    {
        public string RoomNumber { get; set; }
        public int RoomTypeId { get; set; }
        public string RoomDetailDescription { get; set; }
        public int RoomMaxCapacity { get; set; }
        public decimal RoomPricePerDay { get; set; }
        public byte RoomStatus { get; set; }
        public string ErrorMessage { get; set; }

        public ICommand SaveCommand { get; }
        public event Action<bool> RequestClose;

        public List<RoomType> RoomTypes { get; }
        public RoomType SelectedRoomType { get; set; }

        public RoomEditDialogViewModel(RoomInformation room = null)
        {
            // Lấy danh sách RoomType từ singleton repository
            RoomTypes = App._roomTypeRepositorySingleton.GetAll().ToList();
            if (room != null)
            {
                RoomNumber = room.RoomNumber;
                RoomTypeId = room.RoomTypeId;
                RoomDetailDescription = room.RoomDetailDescription;
                RoomMaxCapacity = room.RoomMaxCapacity ?? 0;
                RoomPricePerDay = room.RoomPricePerDay ?? 0;
                RoomStatus = room.RoomStatus ?? 1;
                SelectedRoomType = RoomTypes.FirstOrDefault(rt => rt.RoomTypeId == room.RoomTypeId);
            }
            else
            {
                RoomStatus = 1;
                SelectedRoomType = RoomTypes.FirstOrDefault();
            }
            SaveCommand = new RelayCommand(_ => Save());
        }

        private void Save()
        {
            ErrorMessage = string.Empty;
            if (string.IsNullOrWhiteSpace(RoomNumber))
            {
                ErrorMessage = "Số phòng không được để trống.";
            }
            else if (SelectedRoomType == null)
            {
                ErrorMessage = "Phải chọn loại phòng.";
            }
            else if (RoomMaxCapacity <= 0)
            {
                ErrorMessage = "Sức chứa tối đa phải lớn hơn 0.";
            }
            else if (RoomPricePerDay <= 0)
            {
                ErrorMessage = "Giá/ngày phải lớn hơn 0.";
            }
            else
            {
                RoomTypeId = SelectedRoomType.RoomTypeId;
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
