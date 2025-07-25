using BLL.Services;
using DAL.Entities;
using FUMiniHotelManagementWPF.Views;
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
    public class AdminRoomViewModel : INotifyPropertyChanged
    {
        private readonly ManageRoomService _manageRoomService;
        public ObservableCollection<RoomInformation> Rooms { get; set; }
        private RoomInformation _selectedRoom;
        public RoomInformation SelectedRoom
        {
            get => _selectedRoom;
            set { _selectedRoom = value; OnPropertyChanged(); }
        }

        public ICommand AddCommand { get; }
        public ICommand EditCommand { get; }
        public ICommand DeleteCommand { get; }
        public ICommand SearchCommand { get; }
        public string SearchKeyword { get; set; }

        public AdminRoomViewModel(ManageRoomService manageRoomService)
        {
            _manageRoomService = manageRoomService;
            Rooms = new ObservableCollection<RoomInformation>(_manageRoomService.GetAll());
            AddCommand = new RelayCommand(_ => AddRoom());
            EditCommand = new RelayCommand(_ => EditRoom(), _ => SelectedRoom != null);
            DeleteCommand = new RelayCommand(_ => DeleteRoom(), _ => SelectedRoom != null);
            SearchCommand = new RelayCommand(_ => Search());
        }

        private void AddRoom()
        {
            var dialog = new RoomEditDialog();
            var vm = new RoomEditDialogViewModel();
            dialog.DataContext = vm;
            vm.RequestClose += result => { if (result) dialog.DialogResult = true; else dialog.DialogResult = false; };
            if (dialog.ShowDialog() == true)
            {
                var newRoom = new DAL.Entities.RoomInformation
                {
                    RoomNumber = vm.RoomNumber,
                    RoomTypeId = vm.RoomTypeId,
                    RoomDetailDescription = vm.RoomDetailDescription,
                    RoomMaxCapacity = vm.RoomMaxCapacity,
                    RoomPricePerDay = vm.RoomPricePerDay,
                    RoomStatus = vm.RoomStatus
                };
                _manageRoomService.Add(newRoom);
                ReloadRooms();
                MessageBox.Show("Thêm phòng thành công!", "Thông báo", MessageBoxButton.OK, MessageBoxImage.Information);
            }
        }

        private void EditRoom()
        {
            if (SelectedRoom == null) return;
            var dialog = new RoomEditDialog();
            var vm = new RoomEditDialogViewModel(SelectedRoom);
            dialog.DataContext = vm;
            vm.RequestClose += result => { if (result) dialog.DialogResult = true; else dialog.DialogResult = false; };
            if (dialog.ShowDialog() == true)
            {
                SelectedRoom.RoomNumber = vm.RoomNumber;
                SelectedRoom.RoomTypeId = vm.RoomTypeId;
                SelectedRoom.RoomDetailDescription = vm.RoomDetailDescription;
                SelectedRoom.RoomMaxCapacity = vm.RoomMaxCapacity;
                SelectedRoom.RoomPricePerDay = vm.RoomPricePerDay;
                SelectedRoom.RoomStatus = vm.RoomStatus;
                _manageRoomService.Update(SelectedRoom);
                ReloadRooms();
                MessageBox.Show("Cập nhật phòng thành công!", "Thông báo", MessageBoxButton.OK, MessageBoxImage.Information);
            }
        }

        private void DeleteRoom()
        {
            if (SelectedRoom == null) return;
            var result = MessageBox.Show($"Bạn có chắc chắn muốn xóa phòng {SelectedRoom.RoomNumber}?", "Xác nhận xóa", MessageBoxButton.YesNo, MessageBoxImage.Warning);
            if (result == MessageBoxResult.Yes)
            {
                bool deleted = _manageRoomService.Remove(SelectedRoom);
                if (deleted)
                {
                    MessageBox.Show("Đã xóa phòng thành công.", "Thông báo", MessageBoxButton.OK, MessageBoxImage.Information);
                }
                else
                {
                    MessageBox.Show("Phòng đã có lịch sử đặt, chỉ đổi trạng thái sang 0.", "Thông báo", MessageBoxButton.OK, MessageBoxImage.Information);
                }
                ReloadRooms();
            }
        }

        private void Search()
        {
            Rooms.Clear();
            foreach (var r in _manageRoomService.Search(SearchKeyword))
                Rooms.Add(r);
        }

        private void ReloadRooms()
        {
            Rooms.Clear();
            foreach (var r in _manageRoomService.GetAll())
                Rooms.Add(r);
        }

        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
