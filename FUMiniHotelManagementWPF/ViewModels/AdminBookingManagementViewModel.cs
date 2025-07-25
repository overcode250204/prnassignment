using BLL.Services;
using DAL.Entities;
using FUMiniHotelManagementWPF.Views;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
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
    public class AdminBookingViewModel : INotifyPropertyChanged
    {
        private readonly BookingService _bookingService;

        private readonly BookingDetailService _bookingDetailService;

        private readonly CustomerService _customerService;
        private readonly ManageRoomService _manageRoomService;
        public ObservableCollection<BookingReservation> Reservations { get; set; }
        public ObservableCollection<BookingDetail> Details { get; set; }

        private BookingReservation _selectedReservation;
        public BookingReservation SelectedReservation
        {
            get => _selectedReservation;
            set
            {
                _selectedReservation = value;
                OnPropertyChanged();
                Details = _selectedReservation != null
                    ? new ObservableCollection<BookingDetail>(_selectedReservation.BookingDetails)
                    : new ObservableCollection<BookingDetail>();
                OnPropertyChanged(nameof(Details));
            }
        }

        private BookingDetail _selectedDetail;
        public BookingDetail SelectedDetail
        {
            get => _selectedDetail;
            set { _selectedDetail = value; OnPropertyChanged(); }
        }

        public ICommand AddReservationCommand { get; }
        public ICommand EditReservationCommand { get; }
        public ICommand DeleteReservationCommand { get; }
        public ICommand AddDetailCommand { get; }
        public ICommand EditDetailCommand { get; }
        public ICommand DeleteDetailCommand { get; }

        public AdminBookingViewModel(BookingService BookingService, BookingDetailService BookingDetailService, CustomerService CustomerService, ManageRoomService manageRoomService)
        {
            _customerService = CustomerService;
            _bookingService = BookingService;
            _bookingDetailService = BookingDetailService;
            _manageRoomService = manageRoomService;
            Reservations = new ObservableCollection<BookingReservation>(_bookingService.GetAllReservations());
            AddReservationCommand = new RelayCommand(_ => AddReservation());
            EditReservationCommand = new RelayCommand(_ => EditReservation(), _ => SelectedReservation != null);
            DeleteReservationCommand = new RelayCommand(_ => DeleteReservation(), _ => SelectedReservation != null);
            AddDetailCommand = new RelayCommand(_ => AddDetail(), _ => SelectedReservation != null);
            EditDetailCommand = new RelayCommand(_ => EditDetail(), _ => SelectedDetail != null);
            DeleteDetailCommand = new RelayCommand(_ => DeleteDetail(), _ => SelectedDetail != null);
        }

        private void AddReservation()
        {
            var dialog = new BookingReservationEditDialog();
            var vm = new BookingReservationEditDialogViewModel();
            dialog.DataContext = vm;
            vm.RequestClose += result => dialog.DialogResult = result;
            if (dialog.ShowDialog() == true)
            {
                if (!ValidateReservation(vm, out string error))
                {
                    MessageBox.Show(error, "Lỗi nhập liệu", MessageBoxButton.OK, MessageBoxImage.Error);
                    return;
                }
                int newId = _bookingService.GenerateNewBookingReservationId();
                var newReservation = new BookingReservation
                {
                    BookingReservationId = newId,
                    CustomerId = vm.CustomerId,
                    BookingDate = vm.BookingDate.HasValue ? DateOnly.FromDateTime(vm.BookingDate.Value) : (DateOnly?)null,
                    TotalPrice = vm.TotalPrice,
                    BookingStatus = vm.BookingStatus,
                    BookingDetails = new List<BookingDetail>()
                };
                _bookingService.AddReservation(newReservation);
                SelectedReservation = newReservation;
                Reload();
                MessageBox.Show("Thêm đơn đặt phòng thành công!", "Thông báo", MessageBoxButton.OK, MessageBoxImage.Information);
            }
        }

        private void EditReservation()
        {
            if (SelectedReservation == null) return;
            var dialog = new BookingReservationEditDialog();
            var vm = new BookingReservationEditDialogViewModel(SelectedReservation);
            dialog.DataContext = vm;
            vm.RequestClose += result => dialog.DialogResult = result;
            if (dialog.ShowDialog() == true)
            {
                if (!ValidateReservation(vm, out string error))
                {
                    MessageBox.Show(error, "Lỗi nhập liệu", MessageBoxButton.OK, MessageBoxImage.Error);
                    return;
                }
                SelectedReservation.CustomerId = vm.CustomerId;
                SelectedReservation.BookingDate = vm.BookingDate.HasValue ? DateOnly.FromDateTime(vm.BookingDate.Value) : (DateOnly?)null;
                SelectedReservation.TotalPrice = vm.TotalPrice;
                SelectedReservation.BookingStatus = vm.BookingStatus;
                _bookingService.UpdateReservation(SelectedReservation);
                OnPropertyChanged(nameof(Reservations));
                Reload();
                MessageBox.Show("Cập nhật đơn đặt phòng thành công!", "Thông báo", MessageBoxButton.OK, MessageBoxImage.Information);
            }
        }

        private void DeleteReservation()
        {
            if (SelectedReservation == null) return;
            var result = MessageBox.Show("Bạn có chắc muốn xóa đặt phòng này?", "Xác nhận xóa", MessageBoxButton.YesNo, MessageBoxImage.Warning);
            if (result == MessageBoxResult.Yes)
            {
                _bookingService.RemoveReservation(SelectedReservation);
                Reservations.Remove(SelectedReservation);
                SelectedReservation = null;
                Reload();
                MessageBox.Show("Xóa đơn đặt phòng thành công!", "Thông báo", MessageBoxButton.OK, MessageBoxImage.Information);
            }
        }

        private void AddDetail()
        {
            if (SelectedReservation == null) return;
            var dialog = new BookingDetailEditDialog();
            var vm = new BookingDetailEditDialogViewModel(null, SelectedReservation.BookingDate?.ToDateTime(TimeOnly.MinValue));
            dialog.DataContext = vm;
            vm.RequestClose += result => dialog.DialogResult = result;
            if (dialog.ShowDialog() == true)
            {
                if (!ValidateDetail(vm, out string error))
                {
                    MessageBox.Show(error, "Lỗi nhập liệu", MessageBoxButton.OK, MessageBoxImage.Error);
                    return;
                }
                if (vm.SelectedRoom == null)
                {
                    MessageBox.Show("Bạn phải chọn một phòng!", "Lỗi", MessageBoxButton.OK, MessageBoxImage.Error);
                    return;
                }
                var newDetail = new BookingDetail
                {
                    BookingReservationId = SelectedReservation.BookingReservationId,
                    RoomId = vm.SelectedRoom.RoomId,
                    ActualPrice = vm.ActualPrice,
                    StartDate = vm.StartDate.HasValue ? DateOnly.FromDateTime(vm.StartDate.Value) : default,
                    EndDate = vm.EndDate.HasValue ? DateOnly.FromDateTime(vm.EndDate.Value) : default
                };
                _bookingService.AddDetail(newDetail);
                OnPropertyChanged(nameof(Details));
                Reload();
                MessageBox.Show("Thêm chi tiết đặt phòng thành công!", "Thông báo", MessageBoxButton.OK, MessageBoxImage.Information);
            }
        }

        private void EditDetail()
        {
            if (SelectedDetail == null) return;
            var dialog = new BookingDetailEditDialog();
            var vm = new BookingDetailEditDialogViewModel(SelectedDetail);
            dialog.DataContext = vm;
            vm.RequestClose += result => dialog.DialogResult = result;
            if (dialog.ShowDialog() == true)
            {
                if (!ValidateDetail(vm, out string error))
                {
                    MessageBox.Show(error, "Lỗi nhập liệu", MessageBoxButton.OK, MessageBoxImage.Error);
                    return;
                }
                SelectedDetail.RoomId = vm.RoomId;
                SelectedDetail.ActualPrice = vm.ActualPrice;
                SelectedDetail.StartDate = vm.StartDate.HasValue ? DateOnly.FromDateTime(vm.StartDate.Value) : default;
                SelectedDetail.EndDate = vm.EndDate.HasValue ? DateOnly.FromDateTime(vm.EndDate.Value) : default;
                _bookingService.UpdateDetail(SelectedDetail);
                OnPropertyChanged(nameof(Details));
                Reload();
                MessageBox.Show("Cập nhật chi tiết đặt phòng thành công!", "Thông báo", MessageBoxButton.OK, MessageBoxImage.Information);
            }
        }

        private void DeleteDetail()
        {
            if (SelectedDetail == null) return;
            var result = MessageBox.Show("Bạn có chắc muốn xóa chi tiết này?", "Xác nhận xóa", MessageBoxButton.YesNo, MessageBoxImage.Warning);
            if (result == MessageBoxResult.Yes)
            {
                _bookingService.RemoveDetail(SelectedDetail);
                SelectedReservation.BookingDetails.Remove(SelectedDetail);
                Details.Remove(SelectedDetail);
                OnPropertyChanged(nameof(Details));
                Reload();
                MessageBox.Show("Xóa chi tiết đặt phòng thành công!", "Thông báo", MessageBoxButton.OK, MessageBoxImage.Information);
            }
        }

        private void Reload()
        {
            Reservations.Clear();
            foreach (var r in _bookingService.GetAllReservations())
                Reservations.Add(r);
            if (SelectedReservation != null)
            {
                Details = new ObservableCollection<BookingDetail>(SelectedReservation.BookingDetails);
                OnPropertyChanged(nameof(Details));
            }
        }

        private bool ValidateReservation(BookingReservationEditDialogViewModel vm, out string error)
        {
            error = string.Empty;
            if (!_customerService.CheckCustomerExist(vm.CustomerId))
            {
                error = "Khách hàng không tồn tại";
            }
            if (vm.CustomerId <= 0)
                error = "Khách hàng không hợp lệ.";
            else if (vm.BookingDate == null)
                error = "Ngày đặt không hợp lệ.";
            else if (vm.TotalPrice == null || vm.TotalPrice < 0)
                error = "Tổng tiền không hợp lệ.";
            else if (vm.BookingStatus == null || vm.BookingStatus < 0)
                error = "Trạng thái không hợp lệ.";
            return string.IsNullOrEmpty(error);
        }

        private bool ValidateDetail(BookingDetailEditDialogViewModel vm, out string error)
        {
            error = string.Empty;
            if (vm.RoomId <= 0)
                error = "Phòng không hợp lệ.";
            else if (vm.ActualPrice == null || vm.ActualPrice < 0)
                error = "Giá thực tế không hợp lệ.";
            else if (vm.StartDate == null)
                error = "Ngày bắt đầu không hợp lệ.";
            else if (vm.EndDate == null)
                error = "Ngày kết thúc không hợp lệ.";
            // Kiểm tra ngày chi tiết phải nằm trong khoảng ngày của reservation
            else if (SelectedReservation != null && SelectedReservation.BookingDate != null)
            {
                var bookingDate = SelectedReservation.BookingDate.Value.ToDateTime(TimeOnly.MinValue).Date;
                var detailStart = vm.StartDate.Value.Date;
                var detailEnd = vm.EndDate.Value.Date;
                if (detailStart < bookingDate)
                {
                    error = "Ngày bắt đầu của chi tiết phải lớn hơn hoặc bằng ngày đặt phòng!";
                    return false;
                }
                if (detailEnd < bookingDate)
                {
                    error = "Ngày kết thúc của chi tiết phải lớn hơn hoặc bằng ngày đặt phòng!";
                    return false;
                }
            }

            else if (SelectedReservation != null && SelectedReservation.BookingDetails.Any(d => d.RoomId == vm.RoomId))
            {
                error = "Khách hàng này đã có chi tiết đặt phòng cho phòng này!";
                return false;
            }
            else
            {
                var newStart = vm.StartDate.Value.Date;
                var newEnd = vm.EndDate.Value.Date;

                var allDetails = _bookingDetailService.GetByRoomId(vm.RoomId);
                foreach (var d in allDetails)
                {
                    if (SelectedDetail != null && d == SelectedDetail) continue;
                    var existStart = d.StartDate.ToDateTime(TimeOnly.MinValue).Date;
                    var existEnd = d.EndDate.ToDateTime(TimeOnly.MinValue).Date;
                    if (!(existEnd < newStart))
                    {
                        error = "Phòng này đã có chi tiết đặt phòng bị trùng hoặc giao nhau ngày với khách hàng khác!";
                        return false;
                    }
                }
            }
            return string.IsNullOrEmpty(error);
        }

        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
