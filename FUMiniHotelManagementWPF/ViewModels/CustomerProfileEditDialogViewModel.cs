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
    public class CustomerProfileEditDialogViewModel : INotifyPropertyChanged
    {
        private Customer _customer;
        public string CustomerFullName { get => _customer.CustomerFullName; set { _customer.CustomerFullName = value; OnPropertyChanged(); } }
        public string EmailAddress { get => _customer.EmailAddress; set { _customer.EmailAddress = value; OnPropertyChanged(); } }
        public string Telephone { get => _customer.Telephone; set { _customer.Telephone = value; OnPropertyChanged(); } }
        public DateTime? DateOfBirth
        {
            get => _customer.CustomerBirthday.HasValue ? _customer.CustomerBirthday.Value.ToDateTime(TimeOnly.MinValue) : (DateTime?)null;
            set { _customer.CustomerBirthday = value.HasValue ? DateOnly.FromDateTime(value.Value) : (DateOnly?)null; OnPropertyChanged(); }
        }
        public string Password { get => _customer.Password; set { _customer.Password = value; OnPropertyChanged(); } }
        public string ErrorMessage { get; set; }

        public ICommand SaveCommand { get; }
        public event Action<bool> RequestClose;

        public CustomerProfileEditDialogViewModel(Customer customer)
        {
            _customer = customer;
            SaveCommand = new RelayCommand(_ => Save());
        }

        private void Save()
        {
            ErrorMessage = string.Empty;
            if (string.IsNullOrWhiteSpace(CustomerFullName))
                ErrorMessage = "Họ tên không được để trống.";
            else if (string.IsNullOrWhiteSpace(EmailAddress))
                ErrorMessage = "Email không được để trống.";
            else if (string.IsNullOrWhiteSpace(Telephone))
                ErrorMessage = "Số điện thoại không được để trống.";
            else if (DateOfBirth == null)
                ErrorMessage = "Ngày sinh không hợp lệ.";
            else if (DateOfBirth >= DateTime.Today)
                ErrorMessage = "Ngày sinh phải là ngày trong quá khứ.";
            else if (string.IsNullOrWhiteSpace(Password))
                ErrorMessage = "Mật khẩu không được để trống.";
            else
            {
                App._manageCustomerServiceSingleton.Update(_customer);
                MessageBox.Show("Cập nhật thành công!", "Thông báo", MessageBoxButton.OK, MessageBoxImage.Information);
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
