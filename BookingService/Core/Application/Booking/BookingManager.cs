using Application.Booking.DTO;
using Application.Booking.Ports;
using Application.Booking.Requests;
using Application.Payment.Ports;
using Application.Payment.Responses;
using Application.Responses;
using Domain.Booking.Exceptions;
using Domain.Booking.Ports;
using Domain.Guest.Ports;
using Domain.Room.Ports;

namespace Application.Booking
{
    public class BookingManager : IBookingManager
    {
        private readonly IBookingRepository _bookingRepository;
        private readonly IGuestRepository _guestRepository;
        private readonly IRoomRepository _roomRepository;
        private readonly IPaymentProcessorFactory _paymentProcessorFactory;

        public BookingManager(IBookingRepository bookingRepository, 
            IGuestRepository guestRepository, 
            IRoomRepository roomRepository, 
            IPaymentProcessorFactory paymentProcessorFactory)
        {
            _bookingRepository = bookingRepository;
            _guestRepository = guestRepository;
            _roomRepository = roomRepository;
            _paymentProcessorFactory = paymentProcessorFactory;
        }

        public async Task<BookingResponse> CreateBooking(CreateBookingRequest request)
        {
            try
            {
                var booking = BookingDTO.MaptoEntity(request.Data);
                booking.Guest = await _guestRepository.Get(booking.Guest.Id);
                booking.Room = await _roomRepository.GetAggregate(booking.Room.Id);

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
            catch (RoomCannotBeBookedException e)
            {
                return new BookingResponse
                {
                    Sucess = false,
                    ErrorCode = ErrorCode.BK_ROOM_CANNOT_BE_BOOKED,
                    Message = "The given room cannot be booked",
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

        public async Task<PaymentResponse> PayForABooking(BookingPaymentRequestDTO paymentRequest)
        {
            var paymentProcessor = _paymentProcessorFactory.GetPaymentProcessor(paymentRequest.SelectedPaymentProvider);

            var response = await paymentProcessor.CapturePayment(paymentRequest.PaymentIntention);

            if (response.Sucess)
            {
                return new PaymentResponse
                {
                    Sucess = true,
                    Data = response.Data,
                    Message = "Payment successfully processed",
                };
            }

            return response;
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
