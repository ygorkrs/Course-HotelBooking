using Application.Booking.DTO;
using Application.Booking.Requests;
using Application.Payment.Responses;
using Application.Responses;

namespace Application.Booking.Ports
{
    public interface IBookingManager
    {
        Task<PaymentResponse> PayForABooking(BookingPaymentRequestDTO paymentRequest);
    }
}