using Application.Responses;
using Application.Room.DTO;
using Application.Room.Ports;
using Application.Room.Requests;
using Domain.Exceptions;
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

        public async Task<RoomResponse> CreateRoom(CreateRoomRequest request)
        {
            try
            {
                var room = RoomDTO.MapToEntity(request.Data);

                await room.Save(_roomRepository);

                request.Data.Id = room.Id;

                return new RoomResponse
                {
                    Data = request.Data,
                    Sucess = true,
                };
            }
            catch (MissingRequiredInformationException e)
            {
                return new RoomResponse
                {
                    Sucess = false,
                    ErrorCode = ErrorCode.MISSING_REQUIRED_INFORMATION,
                    Message = "Missing required information passed",
                };
            }
            catch (Exception e)
            {
                return new RoomResponse
                {
                    Sucess = false,
                    ErrorCode = ErrorCode.COULD_NOT_STORE_DATA,
                    Message = "There was an error when saving to DB",
                };
            }
        }

        public async Task<RoomResponse> GetRoom(int idRoom)
        {
            var room = await _roomRepository.Get(idRoom);

            if (room == null)
            {
                return new RoomResponse
                {
                    Sucess = false,
                    ErrorCode = ErrorCode.NOT_FOUND,
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
