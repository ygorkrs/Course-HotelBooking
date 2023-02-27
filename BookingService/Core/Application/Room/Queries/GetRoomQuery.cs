using Application.Responses;
using Application.Room.DTO;
using Domain.Room.Ports;
using MediatR;

namespace Application.Room.Queries;

public class GetRoomQuery : IRequest<RoomResponse>
{
    public int Id { get; set; }
}

public class GetRoomQueryHandler : IRequestHandler<GetRoomQuery, RoomResponse>
{
    private readonly IRoomRepository _roomRepository;

    public GetRoomQueryHandler(IRoomRepository roomRepository)
    {
        _roomRepository = roomRepository;
    }

    public async Task<RoomResponse> Handle(GetRoomQuery request, CancellationToken cancellationToken)
    {
        var room = await _roomRepository.Get(request.Id);

        if (room == null)
        {
            return new RoomResponse
            {
                Sucess = false,
                ErrorCode = ErrorCode.NOT_FOUND_ROOM,
                Message = "Room not found for the given Id",
            };
        }

        return new RoomResponse
        {
            Data = RoomDTO.MapToDTO(room),
            Sucess = true,
        };
    }
}
