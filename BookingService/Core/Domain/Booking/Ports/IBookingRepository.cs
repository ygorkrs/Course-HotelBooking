using Entities = Domain.Entities;

namespace Domain.Booking.Ports
{
    public interface IBookingRepository
    {
        Task<Entities.Booking?> Get(int bookingId);
        Task<Entities.Booking> Create(Entities.Booking booking);
    }
}
