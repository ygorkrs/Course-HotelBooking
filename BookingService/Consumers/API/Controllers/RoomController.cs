using Application.Responses;
using Application.Room.DTO;
using Application.Room.Ports;
using Application.Room.Requests;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class RoomController : ControllerBase
    {
        private readonly ILogger<RoomController> _logger;
        private readonly IRoomManager _roomManager;

        public RoomController(
            ILogger<RoomController> logger, 
            IRoomManager roomManager)
        {
            _logger = logger;
            _roomManager = roomManager;
        }

        [HttpPost]
        public async Task<ActionResult<RoomDTO>> Post(RoomDTO room)
        {
            var request = new CreateRoomRequest
            {
                Data = room,
            };

            var res = await _roomManager.CreateRoom(request);

            if (res == null) { return BadRequest(500); }

            if (res.Sucess)
            {
                return Created("", res.Data);
            }
            else if (res.ErrorCode == ErrorCode.NOT_FOUND)
            {
                return NotFound(res);
            }
            else if (res.ErrorCode == ErrorCode.COULD_NOT_STORE_DATA ||
                res.ErrorCode == ErrorCode.MISSING_REQUIRED_INFORMATION)
            {
                return BadRequest(res);
            }

            _logger.LogError("Response with unknown ErrorCode Returned", res);
            return BadRequest(500);
        }

        [HttpGet]
        public async Task<ActionResult<RoomDTO>> Get(int idRoom)
        {
            var res = await _roomManager.GetRoom(idRoom);

            if (res.Sucess) return Created("", res);

            return NotFound(res);
        }
    }
}
