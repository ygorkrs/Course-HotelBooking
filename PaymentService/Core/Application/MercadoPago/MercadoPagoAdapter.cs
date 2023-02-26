using Application.MercadoPago.Exceptions;
using Application.Payment;
using Application.Payment.DTO;
using Application.Payment.Responses;
using Application.Payment.Enums;
using Application.Responses;

namespace Payment.Application
{
    public class MercadoPagoAdapter : IMercadoPagoPaymentService
    {
        public Task<PaymentResponse> PayWithBankTransfer(string paymentIntention)
        {
            throw new NotImplementedException();
        }

        public Task<PaymentResponse> PayWithCreditCard(string paymentIntention)
        {
            try
            {
                if (string.IsNullOrEmpty(paymentIntention))
                {
                    throw new InvalidPaymentIntentionException();
                }

                paymentIntention += "/success";

                var dto = new PaymentResponse()
                {
                    Data = new PaymentStateDTO()
                    {
                        CreatedDate = DateTime.Now,
                        Message = $"Succcessfully paid {paymentIntention}",
                        PaymentId = "123",
                        Status = PaymentStatus.Success
                    },
                    Sucess = true,
                };

                return Task.FromResult(dto);
            }
            catch (InvalidPaymentIntentionException ex)
            {
                return Task.FromResult(new PaymentResponse()
                {
                    Sucess = false,
                    ErrorCode = ErrorCode.PAYMENT_INVALID_PAYMENT_INTENTION
                });
            }
            catch (Exception ex)
            {
                return Task.FromResult(new PaymentResponse()
                {
                    Sucess = false,
                    ErrorCode = ErrorCode.PAYMENT_GENERAL_ERROR,
                    Message = ex.Message
                });
            }
        }

        public Task<PaymentResponse> PayWithDebitCard(string paymentIntention)
        {
            throw new NotImplementedException();
        }
    }
}
