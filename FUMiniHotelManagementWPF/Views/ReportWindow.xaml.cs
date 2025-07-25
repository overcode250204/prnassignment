using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace FUMiniHotelManagementWPF.Views
{
    /// <summary>
    /// Interaction logic for ReportWindow.xaml
    /// </summary>
    public partial class ReportWindow : Window
    {
        public ReportWindow()
        {
            InitializeComponent();
        }
        private void StatisticButton_Click(object sender, RoutedEventArgs e)
        {
            if (FromDatePicker.SelectedDate == null || ToDatePicker.SelectedDate == null)
            {
                ResultTextBlock.Text = "Vui lòng chọn đủ ngày bắt đầu và kết thúc.";
                RoomStatisticsGrid.ItemsSource = null;
                return;
            }
            var from = DateOnly.FromDateTime(FromDatePicker.SelectedDate.Value);
            var to = DateOnly.FromDateTime(ToDatePicker.SelectedDate.Value);
            if (from > to)
            {
                ResultTextBlock.Text = "Ngày bắt đầu phải nhỏ hơn hoặc bằng ngày kết thúc.";
                RoomStatisticsGrid.ItemsSource = null;
                return;
            }
            var reservations = App._manageBookingServiceSingleton.GetAllReservations()
                .Where(r => r.BookingDate != null && r.BookingDate.Value >= from && r.BookingDate.Value <= to)
                .ToList();
            var totalRevenue = reservations.Sum(r => r.TotalPrice ?? 0);
            ResultTextBlock.Text = $"Tổng số đơn đặt phòng: {reservations.Count}\nTổng doanh thu: {totalRevenue:N0} VND";

            var roomStats = reservations
                .SelectMany(r => r.BookingDetails)
                .GroupBy(d => d.Room.RoomNumber)
                .Select(g => new
                {
                    RoomNumber = g.Key,
                    Count = g.Count(),
                    Revenue = g.Sum(d => d.ActualPrice.GetValueOrDefault() * (d.EndDate.DayNumber - d.StartDate.DayNumber))
                })
                .OrderByDescending(x => x.Revenue)
                .ToList();
            RoomStatisticsGrid.ItemsSource = roomStats;
        }
    }
}
