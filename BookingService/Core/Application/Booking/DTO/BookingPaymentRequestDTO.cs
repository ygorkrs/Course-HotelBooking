namespace Application.Booking.DTO
{
    public enum SupportedPaymentProviders
    {
        PayPal = 1,
        MercadoPago = 2,
    }

    public enum SupportedPaymentMethods
    {
        DebitCard = 1,
        CreditCard = 2,
        BankTransfer = 3,
    }

    public class BookingPaymentRequestDTO
    {
        public int BookingId { get; set; }
        public string PaymentIntention { get; set; }
        public SupportedPaymentProviders SelectedPaymentProvider { get; set; }
        public SupportedPaymentMethods SelectedPaymentMethod { get; set; }
    }
}
