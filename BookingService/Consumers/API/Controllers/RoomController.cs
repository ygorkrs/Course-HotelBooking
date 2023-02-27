using Application.Responses;
using Application.Room.DTO;
using Application.Room.Ports;
using Application.Room.Requests;
using Application.Rooms.Commands;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class RoomController : ControllerBase
    {
        private readonly ILogger<RoomController> _logger;
        private readonly IRoomManager _roomManager;
        private readonly IMediator _mediator;

        public RoomController(
            ILogger<RoomController> logger, 
            IRoomManager roomManager,
            IMediator mediator)
        {
            _logger = logger;
            _roomManager = roomManager;
            _mediator = mediator;
        }

        [HttpPost]
        public async Task<ActionResult<RoomDTO>> Post(RoomDTO room)
        {
            var command  = new CreateRoomCommand
            {
                CreateRoomRequest = new CreateRoomRequest
                {
                    Data = room,
                }
            };

            var res = await _mediator.Send(command);

            if (res == null) { return BadRequest(500); }

            if (res.Sucess)
            {
                return Created("", res.Data);
            }            
            else if (res.ErrorCode == ErrorCode.MISSING_ROOM_REQUIRED_INFORMATION ||
                res.ErrorCode == ErrorCode.COULD_NOT_STORE_ROOM)
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

            if (res.Sucess)
            {
                return Created("", res);
            }
            else if (res.ErrorCode == ErrorCode.NOT_FOUND_ROOM)
            {
                return NotFound(res);
            }

            return NotFound(res);
        }
    }
}
