using Application.Booking.DTO;
using Application.Responses;
using Payment.Application;

namespace Payment.UnitTests
{
    public class MercadoPagoTests
    {
        [SetUp]
        public void Setup()
        {
        }

        [Test]
        public void ShouldReturn_MercadoPagoAdapter_Provider()
        {
            var factory = new PaymentProcessorFactory();

            var provider = factory.GetPaymentProcessor(SupportedPaymentProviders.MercadoPago);

            Assert.AreEqual(provider.GetType(), typeof(MercadoPagoAdapter));
        }

        [Test]
        public async Task ShouldFail_WhenPaymentIntentionStringIsNotValid()
        {
            var factory = new PaymentProcessorFactory();

            var provider = factory.GetPaymentProcessor(SupportedPaymentProviders.MercadoPago);

            var res = await provider.CapturePayment("");

            Assert.IsFalse(res.Sucess);
            Assert.AreEqual(res.ErrorCode, ErrorCode.PAYMENT_INVALID_PAYMENT_INTENTION);
            Assert.AreEqual(res.Message, "The given PaymentIntention is invalid");
        }

        [Test]
        public async Task Should_SuccessfullyProcessPayment()
        {
            var factory = new PaymentProcessorFactory();

            var provider = factory.GetPaymentProcessor(SupportedPaymentProviders.MercadoPago);

            var res = await provider.CapturePayment("https://www.mercadopago.com.br/pagamento/123");

            Assert.IsTrue(res.Sucess);
            Assert.AreEqual(res.Message, "Payment successfully processed");
            Assert.NotNull(res.Data);
            Assert.NotNull(res.Data.CreatedDate);
            Assert.NotNull(res.Data.PaymentId);
        }
    }
}
