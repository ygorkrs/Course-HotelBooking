using Application.Booking.DTO;
using Application.Responses;
using Domain.Booking.Ports;
using MediatR;

namespace Application.Bookings.Queries
{
    public class GetBookingQueryHandler : IRequestHandler<GetBookingQuery, BookingResponse>
    {
        private readonly IBookingRepository _bookingRepository;

        public GetBookingQueryHandler(IBookingRepository bookingRepository)
        {
            _bookingRepository = bookingRepository;
        }

        public async Task<BookingResponse> Handle(GetBookingQuery request, CancellationToken cancellationToken)
        {
            var booking = await _bookingRepository.Get(request.Id);

            if (booking == null)
            {
                return new BookingResponse
                {
                    Sucess = false,
                    ErrorCode = ErrorCode.NOT_FOUND_BOOKING,
                    Message = "Booking not found for the given Id",
                };
            }

            return new BookingResponse
            {
                Data = BookingDTO.MapToDTO(booking),
                Sucess = true,
            };
        }
    }
}
