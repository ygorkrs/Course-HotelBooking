using Application.Payment;
using Application.Payment.Responses;
using Application.Responses;

namespace Payment.Application
{
    public class NotImplementedPaymentProvider : IPaymentProcessor
    {
        public Task<PaymentResponse> CapturePayment(string paymentIntention)
        {
            return Task.FromResult(new PaymentResponse()
            {
                Sucess = false,
                ErrorCode = ErrorCode.PAYMENT_PROVIDER_NOT_IMPLEMENTED,
                Message = "The selected payment provider is not available at the moment",
            });
        }
    }
}
