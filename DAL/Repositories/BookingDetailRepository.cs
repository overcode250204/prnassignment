using DAL.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.Repositories
{
    public class BookingDetailRepository
    {
        private readonly FuminiHotelManagementContext _context;
        public BookingDetailRepository(FuminiHotelManagementContext context) 
        {
            _context = context;
        }

        public IEnumerable<BookingDetail> GetAll()
        {
            return _context.BookingDetails.Include(d => d.Room).ToList();
        }

        public void Add(BookingDetail detail)
        {
            _context.BookingDetails.Add(detail);
            _context.SaveChanges();
        }

        public void Update(BookingDetail detail)
        {
            _context.BookingDetails.Update(detail);
            _context.SaveChanges();
        }

        public void Remove(BookingDetail detail)
        {
            _context.BookingDetails.Remove(detail);
            _context.SaveChanges();
        }

    }
}
