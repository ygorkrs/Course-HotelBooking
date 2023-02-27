using Application.Bookings.Commands;
using Application.Booking.Requests;
using Application.Booking.DTO;
using Moq;
using Domain.Guest.Ports;
using Domain.Booking.Ports;
using Domain.Entities;
using Domain.Room.Ports;
using Application.Responses;
using Domain.Guest.Enums;
using Domain.Room.Enums;

namespace ApplicationTests
{
    [TestFixture]
    internal class CreateBookingCommandTests
    {
        [Test]
        public async Task ShouldNot_CreateBooking_IfRoomIsMissing()
        {
            var command = GetCreateBookingCommand(0, 1, DateTime.UtcNow, DateTime.UtcNow.AddDays(2));

            var fakeGuest = GetFakeGuest(command.CreateBookingRequest.Data.GuestId, 
                "Pedro", 
                "Pereira", 
                "pedro@gmail.com", 
                DocumentType.Passport, 
                "ASBD2344");

            var guestRepository = new Mock<IGuestRepository>();
            guestRepository.Setup(x => x.Get(command.CreateBookingRequest.Data.GuestId))
                .Returns(Task.FromResult(fakeGuest));


            var handler = GetCommmandMock(null, guestRepository);

            var resp = await handler.Handle(command, CancellationToken.None);

            Assert.NotNull(resp);
            Assert.IsFalse(resp.Sucess);
            Assert.NotNull(resp.ErrorCode == ErrorCode.MISSING_BK_ROOM_INFORMATION);
            Assert.NotNull(resp.Message == "Missing Room information");
        }

        [Test]
        public async Task ShouldNot_CreateBooking_IfStartDateIsMissing()
        {
            var command = GetCreateBookingCommand(1, 1, default(DateTime), DateTime.UtcNow.AddDays(2));

            var fakeGuest = GetFakeGuest(command.CreateBookingRequest.Data.GuestId,
                "Pedro",
                "Pereira",
                "pedro@gmail.com",
                DocumentType.Passport,
                "ASBD2344");

            var guestRepository = new Mock<IGuestRepository>();
            guestRepository.Setup(x => x.Get(command.CreateBookingRequest.Data.GuestId))
                .Returns(Task.FromResult(fakeGuest));

            var fakeRoom = GetFakeRoom(command.CreateBookingRequest.Data.RoomId,
                false,
                1,
                "Room 101",
                AcceptedCurrencies.USD,
                1000);

            var roomRepository = new Mock<IRoomRepository>();
            roomRepository.Setup(x => x.GetAggregate(command.CreateBookingRequest.Data.RoomId))
                .Returns(Task.FromResult(fakeRoom));


            var handler = GetCommmandMock(null, guestRepository, roomRepository);

            var resp = await handler.Handle(command, CancellationToken.None);

            Assert.NotNull(resp);
            Assert.IsFalse(resp.Sucess);
            Assert.NotNull(resp.ErrorCode == ErrorCode.MISSING_BK_START_INFORMATION);
            Assert.NotNull(resp.Message == "Missing Start information");
        }

        [Test]
        public async Task Should_CreateBooking()
        {
            var command = GetCreateBookingCommand(1, 1, DateTime.UtcNow, DateTime.UtcNow.AddDays(2));

            var fakeGuest = GetFakeGuest(command.CreateBookingRequest.Data.GuestId,
                "Pedro",
                "Pereira",
                "pedro@gmail.com",
                DocumentType.Passport,
                "ASBD2344");

            var guestRepository = new Mock<IGuestRepository>();
            guestRepository.Setup(x => x.Get(command.CreateBookingRequest.Data.GuestId))
                .Returns(Task.FromResult(fakeGuest));

            var fakeRoom = GetFakeRoom(command.CreateBookingRequest.Data.RoomId,
                false,
                1,
                "Room 101",
                AcceptedCurrencies.USD,
                1000);

            var roomRepository = new Mock<IRoomRepository>();
            roomRepository.Setup(x => x.GetAggregate(command.CreateBookingRequest.Data.RoomId))
                .Returns(Task.FromResult(fakeRoom));

            var fakeBooking = new Booking
            {
                Id = 1,
            };
            var bookingRepository = new Mock<IBookingRepository>();
            bookingRepository.Setup(x => x.Create(It.IsAny<Booking>()))
                .Returns(Task.FromResult(fakeBooking));

            var handler = GetCommmandMock(bookingRepository, guestRepository, roomRepository);

            var resp = await handler.Handle(command, CancellationToken.None);

            Assert.NotNull(resp);
            Assert.IsTrue(resp.Sucess);
            Assert.NotNull(resp.Data);
            Assert.AreEqual(resp.Data.Id, fakeBooking.Id);
        }

        #region Aux
        private CreateBookingCommandHandler GetCommmandMock(
            Mock<IBookingRepository> bookingRepository = null,
            Mock<IGuestRepository> guestRepository = null,
            Mock<IRoomRepository> roomRepository = null)
        {
            var _bookingRepository = bookingRepository ?? new Mock<IBookingRepository>();
            var _guestRepository = guestRepository ?? new Mock<IGuestRepository>();
            var _roomRepository = roomRepository ?? new Mock<IRoomRepository>();

            return new CreateBookingCommandHandler(_bookingRepository.Object,
                _guestRepository.Object,
                _roomRepository.Object);
        }

        private CreateBookingCommand GetCreateBookingCommand(int roomId, int guestId, DateTime start, DateTime end)
        {
            return new CreateBookingCommand
            {
                CreateBookingRequest = new CreateBookingRequest
                {
                    Data = new BookingDTO
                    {
                        RoomId = roomId,
                        GuestId = guestId,
                        Start = start,
                        End = end,
                    }
                }
            };
        }

        private Guest GetFakeGuest(int id, string name, string surname, string email, DocumentType docType, string docNumber)
        {
            return new Guest
            {
                Id = id,
                Name = name,
                Surname = surname,
                Email = email,
                DocumentId = new Domain.Guest.ValueObjects.DocumentId
                {
                    DocumentType = docType,
                    IdNumber = docNumber
                }
            };
        }

        private Room GetFakeRoom(int id, bool inMaintenance, int level, string name, AcceptedCurrencies currency, decimal value)
        {
            return new Room
            {
                Id = id,
                InMaintenance = inMaintenance,
                Level = level,
                Name = name,
                Price = new Domain.Room.ValueObjects.Price
                {
                    Currency = currency,
                    Value = value,
                },
                Bookings = new List<Booking>() { }
            };
        }
        #endregion
    }
}
