using Payment.Application;
using Application.Booking.DTO;
using Application.Responses;

namespace Payment.UnitTests
{
    public class PaymentProcessorFactoryTests
    {
        [SetUp]
        public void Setup()
        {
        }

        [Test]
        public void ShouldReturn_NotImplementedPaymentProvider_WhenAskingForPaypalProvider()
        {
            var factory = new PaymentProcessorFactory();

            var provider = factory.GetPaymentProcessor(SupportedPaymentProviders.PayPal);

            Assert.AreEqual(provider.GetType(), typeof(NotImplementedPaymentProvider));
        }

        [Test]
        public void ShouldReturn_MercadoPagoProvider_WhenAskingForMercadoPagoProvider()
        {
            var factory = new PaymentProcessorFactory();

            var provider = factory.GetPaymentProcessor(SupportedPaymentProviders.MercadoPago);

            Assert.AreEqual(provider.GetType(), typeof(MercadoPagoAdapter));
        }

        [Test]
        public async Task ShouldReturnFalse_WhenCapturePaymentFor_NotImplementedPaymentProvider()
        {
            var factory = new PaymentProcessorFactory();

            var provider = factory.GetPaymentProcessor(SupportedPaymentProviders.PayPal);

            var res = await provider.CapturePayment("https://www.mercadopago.com.br/pagamento/123");

            Assert.IsFalse(res.Sucess);
            Assert.AreEqual(res.ErrorCode, ErrorCode.PAYMENT_PROVIDER_NOT_IMPLEMENTED);
            Assert.AreEqual(res.Message, "The selected payment provider is not available at the moment");
        }
    }
}