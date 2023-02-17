using Application.Booking.DTO;
using Application.Booking.Ports;
using Application.Booking.Requests;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    public class BookingController : Controller
    {
        private readonly ILogger<BookingController> _logger;
        public readonly IBookingManager _bookingManager;

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

            return BadRequest(500);
        }
    }
}
