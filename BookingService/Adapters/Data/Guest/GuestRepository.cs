using Domain.Ports;
using Microsoft.EntityFrameworkCore;

namespace Data.Guest
{
    public class GuestRepository : IGuestRepository
    {
        private readonly HotelDBContext _hotelDBContext;
        public GuestRepository(HotelDBContext hotelDBContext) 
        {
            _hotelDBContext = hotelDBContext;
        }

        public async Task<int> Create(Domain.Entities.Guest guest)
        {
            _hotelDBContext.Guests.Add(guest);
            await _hotelDBContext.SaveChangesAsync();
            return guest.Id;
        }

        public Task<Domain.Entities.Guest?> Get(int id)
        {
            return _hotelDBContext.Guests.Where(guest => guest.Id == id).FirstOrDefaultAsync();
        }
    }
}
