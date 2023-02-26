using Application.Booking.DTO;
using Application.Booking.Ports;
using Application.Booking.Requests;
using Application.Payment.Responses;
using Application.Responses;
using Microsoft.AspNetCore.Mvc;
using MediatR;
using Application.Bookings.Commands;
using Application.Bookings.Queries;

namespace API.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class BookingController : ControllerBase
    {
        private readonly ILogger<BookingController> _logger;
        private readonly IBookingManager _bookingManager;
        private readonly IMediator _mediator;

        public BookingController(
            ILogger<BookingController> logger,
            IBookingManager bookingManager,
            IMediator mediator)
        {
            _logger = logger;
            _bookingManager = bookingManager;
            _mediator = mediator;
        }

        [HttpPost]
        [Route("{bookingId}/Pay")]
        public async Task<ActionResult<PaymentResponse>> Pay(BookingPaymentRequestDTO paymentRequest, int bookingId)
        {
            paymentRequest.BookingId = bookingId;
            var res = await _bookingManager.PayForABooking(paymentRequest);

            if (res.Sucess) 
                return Ok(res.Data);

            return BadRequest(res);
        }

        [HttpPost]
        public async Task<ActionResult<BookingDTO>> Post(BookingDTO booking)
        {
            var request = new CreateBookingRequest
            {
                Data = booking,
            };

            var res = await _mediator.Send(new CreateBookingCommand
            {
                CreateBookingRequest = request,
            });

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
            var res = await _mediator.Send(new GetBookingQuery { Id = idBooking });

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
