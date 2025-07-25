using DAL.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.Repositories
{
    public class BookingReservationRepository
    {
        private readonly FuminiHotelManagementContext _context;
        public BookingReservationRepository(FuminiHotelManagementContext context)
        {
            _context = context;
        }

        public IEnumerable<BookingReservation> GetAll()
        {
            return _context.BookingReservations
                .Include(r => r.Customer)
                .Include(r => r.BookingDetails)
                .ThenInclude(d => d.Room)
                .ToList();
        }

        public void Add(BookingReservation reservation)
        {
            _context.BookingReservations.Add(reservation);
            _context.SaveChanges();
        }

        public void Update(BookingReservation reservation)
        {
            _context.BookingReservations.Update(reservation);
            _context.SaveChanges();
        }

        public void Remove(BookingReservation reservation)
        {
            _context.BookingReservations.Remove(reservation);
            _context.SaveChanges();
        }

        public IEnumerable<BookingReservation> GetByCustomerId(int customerId)
        {
            return _context.BookingReservations
                .Include(r => r.Customer)
                .Include(r => r.BookingDetails)
                .ThenInclude(r => r.Room)
                .ThenInclude(r => r.RoomType)
                .Where(r => r.CustomerId == customerId)
                .ToList();
        }
    }
}
