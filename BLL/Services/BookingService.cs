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
            _detailRepo.Add(detail);
        }

        public void UpdateDetail(BookingDetail detail)
        {
            _detailRepo.Update(detail);
        }

        public void RemoveDetail(BookingDetail detail)
        {
            _detailRepo.Remove(detail);
        }

        public int GenerateNewBookingReservationId()
        {
            var allIds = _reservationRepo.GetAll().Select(r => r.BookingReservationId);
            return allIds.Any() ? allIds.Max() + 1 : 1;
        }
    }
}
