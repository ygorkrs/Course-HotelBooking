using Application;
using Application.Guest.DTO;
using Application.Guest.Requests;
using Moq;
using Domain.Entities;
using Domain.Ports;
using Application.Responses;

namespace ApplicationTests
{
    public class Tests
    {
        GuestManager guestManager;
        int expectedId = 999;

        [SetUp]
        public void Setup()
        {
            var mockRepo = new Mock<IGuestRepository>();

            mockRepo.Setup(x => x.Create(
                It.IsAny<Guest>())).Returns(Task.FromResult(expectedId));

            guestManager = new GuestManager(mockRepo.Object);
        }

        [Test]
        public async Task HappyPath()
        {
            var guestDTO = new GuestDTO
            {
                Name = "Pedro",
                Surname = "Silva",
                Email = "pedro.silva@hotmail.com",
                IdNumber = "BA558255",
                IdTypeCode = 2
            };

            var guestRequest = new CreateGuestRequest()
            {
                Data = guestDTO,
            };            

            var res = await guestManager.CreateGuest(guestRequest);

            Assert.IsNotNull(res);
            Assert.IsTrue(res.Sucess);
            Assert.That(res.Data.Id, Is.EqualTo(expectedId));
        }

        [TestCase(null)]
        [TestCase("")]
        [TestCase("a")]
        [TestCase("ab")]
        [TestCase("abc")]
        public async Task ShouldReturnInvalidDocumentIdExceptionWhenDocIsInvalid(string docNumer)
        {
            var guestDTO = new GuestDTO
            {
                Name = "Pedro",
                Surname = "Silva",
                Email = "pedro.silva@hotmail.com",
                IdNumber = docNumer,
                IdTypeCode = 2
            };

            var guestRequest = new CreateGuestRequest()
            {
                Data = guestDTO,
            };

            var res = await guestManager.CreateGuest(guestRequest);

            Assert.IsNotNull(res);
            Assert.IsFalse(res.Sucess);
            Assert.AreEqual(res.ErrorCode, ErrorCode.INVALID_DOCUMENT_ID);
        }

        [TestCase(null, null, null)]
        [TestCase("", "Surname", "email@email.com")]
        [TestCase("Name", "", "email@email.com")]
        [TestCase("Name", "Surname", "")]
        public async Task ShouldReturnMissingRequiredInformationExceptionWhenMissingFields(string name, string surname, string email)
        {
            var guestDTO = new GuestDTO
            {
                Name = name,
                Surname = surname,
                Email = email,
                IdNumber = "BA558255",
                IdTypeCode = 2
            };

            var guestRequest = new CreateGuestRequest()
            {
                Data = guestDTO,
            };

            var res = await guestManager.CreateGuest(guestRequest);

            Assert.IsNotNull(res);
            Assert.IsFalse(res.Sucess);
            Assert.AreEqual(res.ErrorCode, ErrorCode.MISSING_REQUIRED_INFORMATION);
        }

        [TestCase("abc")]
        [TestCase("abc@")]
        [TestCase("abc@.com")]
        public async Task ShouldReturnInvalidEmailExceptionWhenEmailInvalid(string email)
        {
            var guestDTO = new GuestDTO
            {
                Name = "Pedro",
                Surname = "Silva",
                Email = email,
                IdNumber = "BA558255",
                IdTypeCode = 2
            };

            var guestRequest = new CreateGuestRequest()
            {
                Data = guestDTO,
            };

            var res = await guestManager.CreateGuest(guestRequest);

            Assert.IsNotNull(res);
            Assert.IsFalse(res.Sucess);
            Assert.AreEqual(res.ErrorCode, ErrorCode.INVALID_EMAIL);
        }
    }
}