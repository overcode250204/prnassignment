using DAL.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace FUMiniHotelManagementWPF.ViewModels
{
    public class CustomerEditDialogViewModel
    {
        public string CustomerFullName { get; set; }
        public string EmailAddress { get; set; }
        public string Telephone { get; set; }
        public DateTime? DateOfBirth { get; set; }
        public byte Status { get; set; }
        public string Password { get; set; }
        public string ErrorMessage { get; set; }

        public ICommand SaveCommand { get; }
        public event Action<bool> RequestClose;

        public CustomerEditDialogViewModel(Customer customer = null)
        {
            if (customer != null)
            {
                CustomerFullName = customer.CustomerFullName;
                EmailAddress = customer.EmailAddress;
                Telephone = customer.Telephone;
                DateOfBirth = customer.CustomerBirthday.HasValue ? customer.CustomerBirthday.Value.ToDateTime(TimeOnly.MinValue) : (DateTime?)null;
                Status = customer.CustomerStatus ?? 1;
                Password = customer.Password;
            }
            SaveCommand = new RelayCommand(_ => Save());
        }

        private void Save()
        {
            ErrorMessage = string.Empty;
            if (string.IsNullOrWhiteSpace(CustomerFullName))
                ErrorMessage = "Họ tên không được để trống.";
            else if (string.IsNullOrWhiteSpace(EmailAddress) || !Regex.IsMatch(EmailAddress, @"^[^@\s]+@[^@\s]+\.[^@\s]+$"))
                ErrorMessage = "Email không hợp lệ.";
            else if (string.IsNullOrWhiteSpace(Telephone) || !Regex.IsMatch(Telephone, @"^\d{9,12}$"))
                ErrorMessage = "Số điện thoại không hợp lệ.";
            else if (DateOfBirth == null)
                ErrorMessage = "Ngày sinh không hợp lệ.";
            else if (Status != 0 && Status != 1)
                ErrorMessage = "Trạng thái không hợp lệ.";
            else if (string.IsNullOrWhiteSpace(Password))
                ErrorMessage = "Mật khẩu không được để trống.";
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
