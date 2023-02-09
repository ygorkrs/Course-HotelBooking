using Application.Guest.DTO;
using Application.Guest.Ports;
using Application.Guest.Requests;
using Application.Responses;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class GuestController : ControllerBase
    {
        private readonly ILogger<GuestController> _logger;
        private readonly IGuestManager _guestManager;

        public GuestController(
            ILogger<GuestController> logger,
            IGuestManager guestManager)
        {
            _logger = logger;
            _guestManager = guestManager;
        }

        [HttpPost]
        public async Task<ActionResult<GuestDTO>> Post(GuestDTO guest)
        {
            var request = new CreateGuestRequest
            {
                Data = guest,
            };

            var res = await _guestManager.CreateGuest(request);

            if (res == null) { return BadRequest(500); }

            if (res.Sucess)
            {
                return Created("", res.Data);
            }
            else if (res.ErrorCode == ErrorCode.NOT_FOUND)
            {
                return NotFound(res);
            }
            else if (res.ErrorCode == ErrorCode.INVALID_DOCUMENT_ID ||
                res.ErrorCode == ErrorCode.MISSING_REQUIRED_INFORMATION ||
                res.ErrorCode == ErrorCode.INVALID_EMAIL ||
                res.ErrorCode == ErrorCode.COULD_NOT_STORE_DATA)
            {
                return BadRequest(res);
            }

            _logger.LogError("Response with unknown ErrorCode Returned", res);
            return BadRequest(500);
        }

        [HttpGet]
        public async Task<ActionResult<GuestDTO>> Get(int idGuest)
        {
            var res = await _guestManager.GetGuest(idGuest);

            if (res.Sucess) return Created("", res);

            return NotFound(res);
        }
    }
}
