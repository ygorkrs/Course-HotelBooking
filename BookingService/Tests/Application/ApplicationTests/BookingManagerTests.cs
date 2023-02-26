using Application.Booking;
using Application.Payment.DTO;
using Application.Payment.Ports;
using Domain.Booking.Ports;
using Domain.Guest.Ports;
using Domain.Room.Ports;
using Application.Payment.Enums;
using Moq;
using Application.Booking.DTO;
using Application.Payment.Responses;
using Application.Payment;

namespace ApplicationTests
{
    public class BookingManagerTests
    {
        BookingManager bookingManager;

        [SetUp]
        public void Setup()
        {
            
        }

        [Test]
        public async Task Should_PayForABooking()
        {
            var bookingRepository = new Mock<IBookingRepository>();
            var guestRepository = new Mock<IGuestRepository>();
            var roomRepository = new Mock<IRoomRepository>();
            var paymentProcessorFactory = new Mock<IPaymentProcessorFactory>();
            var paymentProcessor = new Mock<IPaymentProcessor>();

            var dto = new BookingPaymentRequestDTO
            {
                SelectedPaymentProvider = SupportedPaymentProviders.MercadoPago,
                PaymentIntention = "https://www.mercadopago.com.br/pagamento/123",
                SelectedPaymentMethod = SupportedPaymentMethods.DebitCard,
            };            

            var responseDto = new PaymentStateDTO
            {
                CreatedDate = DateTime.Now,
                PaymentId = "123",
                Message = $"Succcessfully paid {dto.PaymentIntention}",
                Status = PaymentStatus.Success,
            };

            var response = new PaymentResponse
            {
                Data = responseDto,
                Sucess = true,
                Message = "Payment successfully processed"
            };

            paymentProcessor
                .Setup(x => x.CapturePayment(dto.PaymentIntention))
                .Returns(Task.FromResult(response));

            paymentProcessorFactory
                .Setup(x => x.GetPaymentProcessor(dto.SelectedPaymentProvider))
                .Returns(paymentProcessor.Object);

            bookingManager = new BookingManager(bookingRepository.Object,
                guestRepository.Object,
                roomRepository.Object,
                paymentProcessorFactory.Object);

            var res = await bookingManager.PayForABooking(dto);

            Assert.NotNull(res);
            Assert.True(res.Sucess);
            Assert.That(res.Message, Is.EqualTo("Payment successfully processed"));
        }
    }
}
