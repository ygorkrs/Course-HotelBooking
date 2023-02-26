using Application.Booking.Requests;
using Application.Responses;
using MediatR;

namespace Application.Bookings.Commands
{
    public class CreateBookingCommand : IRequest<BookingResponse>
    {
        public CreateBookingRequest CreateBookingRequest { get; set; }
    }
}
