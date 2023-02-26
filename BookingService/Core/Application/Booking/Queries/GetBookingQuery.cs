using Application.Responses;
using MediatR;

namespace Application.Bookings.Queries
{
    public class GetBookingQuery : IRequest<BookingResponse>
    {
        public int Id { get; set; }
    }
}
