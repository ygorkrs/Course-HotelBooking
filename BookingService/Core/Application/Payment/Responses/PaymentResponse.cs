using Application.Payment.DTO;
using Application.Payment.Enums;
using Application.Responses;

namespace Application.Payment.Responses
{
    public class PaymentResponse : Response
    {
        public PaymentStateDTO Data { get; set; }
    }
}
