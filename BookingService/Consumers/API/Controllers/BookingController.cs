using Application.Booking.DTO;
using Application.Booking.Ports;
using Application.Booking.Requests;
using Application.Responses;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class BookingController : ControllerBase
    {
        private readonly ILogger<BookingController> _logger;
        private readonly IBookingManager _bookingManager;

        public BookingController(
            ILogger<BookingController> logger,
            IBookingManager bookingManager)
        {
            _logger = logger;
            _bookingManager = bookingManager;
        }

        [HttpPost]
        public async Task<ActionResult<BookingDTO>> Post(BookingDTO booking)
        {
            var request = new CreateBookingRequest
            {
                Data = booking,
            };

            var res = await _bookingManager.CreateBooking(request);

            if (res == null) { return BadRequest(500); }

            if (res.Sucess)
            {
                return Created("", res.Data);
            }
            else if (res.ErrorCode == ErrorCode.MISSING_BK_PLACEAT_INFORMATION ||
                res.ErrorCode == ErrorCode.MISSING_BK_START_INFORMATION ||
                res.ErrorCode == ErrorCode.MISSING_BK_END_INFORMATION ||
                res.ErrorCode == ErrorCode.MISSING_BK_ROOM_INFORMATION ||
                res.ErrorCode == ErrorCode.MISSING_BK_GUEST_INFORMATION ||
                res.ErrorCode == ErrorCode.COULD_NOT_STORE_BOOKING)
            {
                return BadRequest(res);
            }

            _logger.LogError("Response with unknown ErrorCode Returned", res);
            return BadRequest(500);
        }

        [HttpGet]
        public async Task<ActionResult<BookingDTO>> Get(int idBooking)
        {
            var res = await _bookingManager.GetBooking(idBooking);

            if (res.Sucess)
            {
                return Created("", res);
            }
            else if (res.ErrorCode == ErrorCode.NOT_FOUND_BOOKING)
            {
                return NotFound(res);
            }

            return NotFound(res);
        }
    }
}
