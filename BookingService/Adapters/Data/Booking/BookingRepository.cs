using Domain.Booking.Ports;
using Microsoft.EntityFrameworkCore;

namespace Data.Booking
{
    public class BookingRepository : IBookingRepository
    {
        private readonly HotelDBContext _hotelDBContext;

        public BookingRepository(HotelDBContext hotelDBContext) {
            _hotelDBContext = hotelDBContext;
        }

        public async Task<Domain.Entities.Booking> Create(Domain.Entities.Booking booking)
        {
            _hotelDBContext.Bookings.Add(booking);
            await _hotelDBContext.SaveChangesAsync();
            return booking;
        }

        public Task<Domain.Entities.Booking?> Get(int bookingId)
        {
            return _hotelDBContext.Bookings
                .Include(g => g.Guest)
                .Include(r => r.Room)
                .Where(booking => booking.Id == bookingId)
                .FirstOrDefaultAsync();
        }
    }
}
