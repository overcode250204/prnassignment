using DAL.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.Repositories
{
    public class RoomInformationRepository
    {
        private readonly FuminiHotelManagementContext _context;
        public RoomInformationRepository(FuminiHotelManagementContext context)
        {
            _context = context;
        }

        public IEnumerable<RoomInformation> GetAll()
        {
            return _context.RoomInformations.Include(r => r.RoomType).ToList();
        }

        public void Remove(RoomInformation room)
        {
            _context.RoomInformations.Remove(room);
            _context.SaveChanges();
        }

        public void Update(RoomInformation room)
        {
            _context.RoomInformations.Update(room);
            _context.SaveChanges();
        }

        public void Add(RoomInformation room)
        {
            _context.RoomInformations.Add(room);
            _context.SaveChanges();
        }
    }
}
