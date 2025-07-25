using DAL.Entities;
using DAL.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL.Services
{
    public class BookingService
    {
        private readonly BookingReservationRepository _reservationRepo;
        private readonly BookingDetailRepository _detailRepo;
        public BookingService(BookingReservationRepository reservationRepo, BookingDetailRepository detailRepo)
        {
            _reservationRepo = reservationRepo;
            _detailRepo = detailRepo;
        }

        public List<BookingReservation> GetAllReservations()
        {
            return _reservationRepo.GetAll().ToList();
        }

        public void AddReservation(BookingReservation reservation)
        {
            if (reservation.TotalPrice == null)
                reservation.TotalPrice = 0;
            _reservationRepo.Add(reservation);
        }

        public void UpdateReservation(BookingReservation reservation)
        {
            _reservationRepo.Update(reservation);
        }

        public void RemoveReservation(BookingReservation reservation)
        {
            _reservationRepo.Remove(reservation);
        }

        public List<BookingDetail> GetDetailsByReservation(int reservationId)
        {
            return _detailRepo.GetAll().Where(d => d.BookingReservationId == reservationId).ToList();
        }

        public void AddDetail(BookingDetail detail)
        {
            var reservation = _reservationRepo.GetAll().FirstOrDefault(r => r.BookingReservationId == detail.BookingReservationId);
            if (reservation != null && reservation.BookingDetails.Any(d => d.RoomId == detail.RoomId))
            {
                throw new InvalidOperationException("Phòng này đã được thêm vào chi tiết đặt phòng!");
            }
            _detailRepo.Add(detail);
            UpdateReservationTotalPrice(detail.BookingReservationId);
        }

        public void UpdateDetail(BookingDetail detail)
        {
            _detailRepo.Update(detail);
            UpdateReservationTotalPrice(detail.BookingReservationId);
        }

        public void RemoveDetail(BookingDetail detail)
        {
            int reservationId = detail.BookingReservationId;
            _detailRepo.Remove(detail);
            UpdateReservationTotalPrice(reservationId);
        }

        private void UpdateReservationTotalPrice(int reservationId)
        {
            var reservation = _reservationRepo.GetAll().FirstOrDefault(r => r.BookingReservationId == reservationId);
            if (reservation != null)
            {
                reservation.TotalPrice = reservation.BookingDetails.Sum(d =>
                    d.ActualPrice.GetValueOrDefault() * (d.EndDate.DayNumber - d.StartDate.DayNumber)
                );
                _reservationRepo.Update(reservation);
            }
        }

        public int GenerateNewBookingReservationId()
        {
            var allIds = _reservationRepo.GetAll().Select(r => r.BookingReservationId);
            return allIds.Any() ? allIds.Max() + 1 : 1;
        }

        public List<BookingReservation> GetReservationsByCustomerId(int customerId)
        {
            return _reservationRepo.GetByCustomerId(customerId).ToList();
        }
    }
}
