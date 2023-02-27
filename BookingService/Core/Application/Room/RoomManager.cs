using Application.Responses;
using Application.Room.DTO;
using Application.Room.Ports;
using Application.Room.Requests;
using Domain.Room.Exceptions;
using Domain.Room.Ports;

namespace Application.Room
{
    public class RoomManager : IRoomManager
    {
        private readonly IRoomRepository _roomRepository;

        public RoomManager(IRoomRepository roomRepository)
        {
            _roomRepository = roomRepository;
        }

        public async Task<RoomResponse> GetRoom(int idRoom)
        {
            var room = await _roomRepository.Get(idRoom);

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
}
