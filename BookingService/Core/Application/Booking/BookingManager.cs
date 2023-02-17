using Application.Booking.DTO;
using Application.Booking.Ports;
using Application.Booking.Requests;
using Application.Responses;
using Domain.Booking.Exceptions;
using Domain.Booking.Ports;

namespace Application.Booking
{
    public class BookingManager : IBookingManager
    {
        private readonly IBookingRepository _bookingRepository;

        public BookingManager(IBookingRepository bookingRepository)
        {
            _bookingRepository = bookingRepository;
        }

        public async Task<BookingResponse> CreateBooking(CreateBookingRequest request)
        {
            try
            {
                var booking = BookingDTO.MaptoEntity(request.Data);

                await booking.Save(_bookingRepository);

                request.Data.Id = booking.Id;

                return new BookingResponse
                {
                    Data = request.Data,
                    Sucess = true,
                };
            }
            catch (MissingPlaceAtIformationException e)
            {
                return new BookingResponse
                {
                    Sucess = false,
                    ErrorCode = ErrorCode.MISSING_BK_PLACEAT_INFORMATION,
                    Message = "Missing PlaceAt information",
                };
            }
            catch (MissingStartInformationException e)
            {
                return new BookingResponse
                {
                    Sucess = false,
                    ErrorCode = ErrorCode.MISSING_BK_START_INFORMATION,
                    Message = "Missing Start information",
                };
            }
            catch (MissingEndInformationException e)
            {
                return new BookingResponse
                {
                    Sucess = false,
                    ErrorCode = ErrorCode.MISSING_BK_END_INFORMATION,
                    Message = "Missing End information",
                };
            }
            catch (MissingRoomInformationException e)
            {
                return new BookingResponse
                {
                    Sucess = false,
                    ErrorCode = ErrorCode.MISSING_BK_ROOM_INFORMATION,
                    Message = "Missing Room information",
                };
            }
            catch (MissingGuestInformationException e)
            {
                return new BookingResponse
                {
                    Sucess = false,
                    ErrorCode = ErrorCode.MISSING_BK_GUEST_INFORMATION,
                    Message = "Missing Guest information",
                };
            }
            catch (Exception e)
            {
                return new BookingResponse
                {
                    Sucess = false,
                    ErrorCode = ErrorCode.COULD_NOT_STORE_BOOKING,
                    Message = "There was an error when saving to DB",
                };
            }
        }

        public async Task<BookingResponse> GetBooking(int idBooking)
        {
            var booking = await _bookingRepository.Get(idBooking);

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
