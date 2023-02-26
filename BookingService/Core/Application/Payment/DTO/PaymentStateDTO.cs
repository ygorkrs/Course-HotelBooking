using Application.Payment.Enums;

namespace Application.Payment.DTO
{
    public class PaymentStateDTO
    {
        public PaymentStatus Status { get; set; }
        public string PaymentId { get; set; }
        public DateTime CreatedDate { get; set; }  = DateTime.UtcNow;
        public string Message { get; set; }
    }
}
