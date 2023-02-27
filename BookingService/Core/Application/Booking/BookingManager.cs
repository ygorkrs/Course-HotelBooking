using Application.Booking.DTO;
using Application.Booking.Ports;
using Application.Booking.Requests;
using Application.Payment.Ports;
using Application.Payment.Responses;
using Application.Responses;
using Domain.Booking.Exceptions;
using Domain.Booking.Ports;
using Domain.Guest.Ports;
using Domain.Room.Ports;

namespace Application.Booking
{
    public class BookingManager : IBookingManager
    {
        private readonly IPaymentProcessorFactory _paymentProcessorFactory;

        public BookingManager(IPaymentProcessorFactory paymentProcessorFactory)
        {
            _paymentProcessorFactory = paymentProcessorFactory;
        }

        public async Task<PaymentResponse> PayForABooking(BookingPaymentRequestDTO paymentRequest)
        {
            var paymentProcessor = _paymentProcessorFactory.GetPaymentProcessor(paymentRequest.SelectedPaymentProvider);

            var response = await paymentProcessor.CapturePayment(paymentRequest.PaymentIntention);

            if (response.Sucess)
            {
                return new PaymentResponse
                {
                    Sucess = true,
                    Data = response.Data,
                    Message = "Payment successfully processed",
                };
            }

            return response;
        }
    }
}
