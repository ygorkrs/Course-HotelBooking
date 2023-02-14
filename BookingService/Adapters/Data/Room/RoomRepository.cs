using Domain.Room.Ports;
using Microsoft.EntityFrameworkCore;

namespace Data.Room
{
    public class RoomRepository : IRoomRepository
    {
        private readonly HotelDBContext _hotelDBContext;

        public RoomRepository(HotelDBContext hotelDBContext)
        {
            _hotelDBContext = hotelDBContext;
        }

        public async Task<int> Create(Domain.Entities.Room room)
        {
            _hotelDBContext.Rooms.Add(room);
            await _hotelDBContext.SaveChangesAsync();
            return room.Id;
        }

        public Task<Domain.Entities.Room?> Get(int roomId)
        {
            return _hotelDBContext.Rooms.Where(room => room.Id == roomId).FirstOrDefaultAsync();
        }
    }
}
