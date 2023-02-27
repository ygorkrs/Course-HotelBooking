using Application.Booking.DTO;
using Application.Booking.Requests;
using Application.Responses;
using Domain.Booking.Exceptions;
using Domain.Booking.Ports;
using Domain.Guest.Ports;
using Domain.Room.Ports;
using MediatR;

namespace Application.Bookings.Commands;

public class CreateBookingCommand : IRequest<BookingResponse>
{
    public CreateBookingRequest CreateBookingRequest { get; set; }
}

public class CreateBookingCommandHandler : IRequestHandler<CreateBookingCommand, BookingResponse>
{
    private readonly IBookingRepository _bookingRepository;
    private readonly IGuestRepository _guestRepository;
    private readonly IRoomRepository _roomRepository;

    public CreateBookingCommandHandler(IBookingRepository bookingRepository,
        IGuestRepository guestRepository,
        IRoomRepository roomRepository)
    {
        _bookingRepository = bookingRepository;
        _guestRepository = guestRepository;
        _roomRepository = roomRepository;
    }

    public async Task<BookingResponse> Handle(CreateBookingCommand request, CancellationToken cancellationToken)
    {
        try
        {
            var booking = BookingDTO.MaptoEntity(request.CreateBookingRequest.Data);
            booking.Guest = await _guestRepository.Get(booking.Guest.Id);
            booking.Room = await _roomRepository.GetAggregate(booking.Room.Id);

            await booking.Save(_bookingRepository);

            request.CreateBookingRequest.Data.Id = booking.Id;

            return new BookingResponse
            {
                Data = request.CreateBookingRequest.Data,
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
}
