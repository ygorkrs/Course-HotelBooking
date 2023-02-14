using Application.Room.Requests;
using Application.Responses;

namespace Application.Room.Ports
{
    public interface IRoomManager
    {
        Task<RoomResponse> CreateRoom(CreateRoomRequest request);
        Task<RoomResponse> GetRoom(int idRoom);
    }
}
