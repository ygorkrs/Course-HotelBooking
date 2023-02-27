using Application.Responses;
using Application.Room.DTO;
using Application.Room.Requests;
using Domain.Room.Exceptions;
using Domain.Room.Ports;
using MediatR;

namespace Application.Room.Commands;

public class CreateRoomCommand : IRequest<RoomResponse>
{
    public CreateRoomRequest CreateRoomRequest { get; set; }
}

public class CreateRoomCommandHandler : IRequestHandler<CreateRoomCommand, RoomResponse>
{
    private readonly IRoomRepository _roomRepository;

    public CreateRoomCommandHandler(IRoomRepository roomRepository)
    {
        _roomRepository = roomRepository;
    }

    public async Task<RoomResponse> Handle(CreateRoomCommand request, CancellationToken cancellationToken)
    {
        try
        {
            var room = RoomDTO.MapToEntity(request.CreateRoomRequest.Data);

            await room.Save(_roomRepository);

            request.CreateRoomRequest.Data.Id = room.Id;

            return new RoomResponse
            {
                Data = request.CreateRoomRequest.Data,
                Sucess = true,
            };
        }
        catch (MissingRoomRequiredInformationException e)
        {
            return new RoomResponse
            {
                Sucess = false,
                ErrorCode = ErrorCode.MISSING_ROOM_REQUIRED_INFORMATION,
                Message = "Missing required information passed",
            };
        }
        catch (Exception e)
        {
            return new RoomResponse
            {
                Sucess = false,
                ErrorCode = ErrorCode.COULD_NOT_STORE_ROOM,
                Message = "There was an error when saving to DB",
            };
        }
    }
}
