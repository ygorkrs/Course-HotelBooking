using Application.Payment.Responses;

namespace Application.Payment
{
    public interface IPaymentProcessor
    {
        public Task<PaymentResponse> CapturePayment(string paymentIntention);
    }
}
