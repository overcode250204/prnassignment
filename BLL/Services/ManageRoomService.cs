using DAL.Entities;
using DAL.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL.Services
{
    public class ManageRoomService
    {
        private readonly RoomInformationRepository _roomRepository;
        private readonly BookingDetailRepository _bookingDetailRepository;
        public ManageRoomService(RoomInformationRepository roomRepository, BookingDetailRepository bookingDetailRepository)
        {
            _roomRepository = roomRepository;
            _bookingDetailRepository = bookingDetailRepository;
        }

        public List<RoomInformation> GetAll()
        {
            return _roomRepository.GetAll().ToList();
        }

        public bool Remove(RoomInformation room)
        {
            var hasBooking = _bookingDetailRepository.GetAll().Any(bd => bd.RoomId == room.RoomId);
            if (!hasBooking)
            {
                _roomRepository.Remove(room);
                return true;
            }
            else
            {
                room.RoomStatus = 0;
                _roomRepository.Update(room);
                return false;
            }
        }

        public void Add(RoomInformation room)
        {
            _roomRepository.Add(room);
        }

        public void Update(RoomInformation room)
        {
            _roomRepository.Update(room);
        }

        public List<RoomInformation> Search(string keyword)
        {
            var all = GetAll();
            if (string.IsNullOrWhiteSpace(keyword)) return all;
            keyword = keyword.ToLower();
            return all.Where(r => (r.RoomNumber != null && r.RoomNumber.ToLower().Contains(keyword))
                               || (r.RoomDetailDescription != null && r.RoomDetailDescription.ToLower().Contains(keyword))
                               || (r.RoomType != null && r.RoomType.RoomTypeName.ToLower().Contains(keyword))
                               || (r.RoomStatus != null && r.RoomStatus.ToString().Contains(keyword)))
                      .ToList();
        }
    }
}
