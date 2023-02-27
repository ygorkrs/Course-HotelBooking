using Application.Room.Commands;
using Domain.Entities;
using Domain.Room.Ports;
using Moq;

namespace ApplicationTests
{
    [TestFixture]
    internal class CreateRoomCommandTests
    {
        private CreateRoomCommandHandler GetMockCommandHandler(Mock<IRoomRepository> roomRepository = null)
        {
            roomRepository = roomRepository ?? new Mock<IRoomRepository>();

            return new CreateRoomCommandHandler(roomRepository.Object);
        }

        [Test]
        public async Task ShouldNot_CreateRoom_IfMissingRoom_NameInformation()
        {
            var command = new CreateRoomCommand
            {
                CreateRoomRequest = new Application.Room.Requests.CreateRoomRequest
                {
                    Data = new Application.Room.DTO.RoomDTO
                    {
                        InMaintenance = false,
                        Level = 1,
                        PriceCurrency = Domain.Room.Enums.AcceptedCurrencies.USD,
                        PriceValue = 1000,
                    }
                }
            };

            var handler = GetMockCommandHandler(null);

            var res = await handler.Handle(command, CancellationToken.None);

            Assert.NotNull(res);
            Assert.IsFalse(res.Sucess);
            Assert.NotNull(res.ErrorCode == Application.Responses.ErrorCode.MISSING_ROOM_REQUIRED_INFORMATION);
            Assert.NotNull(res.Message == "Missing required information passed");
        }

        [Test]
        public async Task ShouldNot_CreateRoom_IfMissingRoom_LevelInformation()
        {
            var command = new CreateRoomCommand
            {
                CreateRoomRequest = new Application.Room.Requests.CreateRoomRequest
                {
                    Data = new Application.Room.DTO.RoomDTO
                    {
                        InMaintenance = false,
                        Name = "Room Name",
                        PriceCurrency = Domain.Room.Enums.AcceptedCurrencies.USD,
                        PriceValue = 1000,
                    }
                }
            };

            var handler = GetMockCommandHandler(null);

            var res = await handler.Handle(command, CancellationToken.None);

            Assert.NotNull(res);
            Assert.IsFalse(res.Sucess);
            Assert.NotNull(res.ErrorCode == Application.Responses.ErrorCode.MISSING_ROOM_REQUIRED_INFORMATION);
            Assert.NotNull(res.Message == "Missing required information passed");
        }

        [Test]
        public async Task ShouldNot_CreateRoom_IfMissingRoom_PriceCurrencyInformation()
        {
            var command = new CreateRoomCommand
            {
                CreateRoomRequest = new Application.Room.Requests.CreateRoomRequest
                {
                    Data = new Application.Room.DTO.RoomDTO
                    {
                        InMaintenance = false,
                        Level = 1,
                        Name = "Room Name",
                        PriceValue = 1000,
                    }
                }
            };

            var handler = GetMockCommandHandler(null);

            var res = await handler.Handle(command, CancellationToken.None);

            Assert.NotNull(res);
            Assert.IsFalse(res.Sucess);
            Assert.NotNull(res.ErrorCode == Application.Responses.ErrorCode.MISSING_ROOM_REQUIRED_INFORMATION);
            Assert.NotNull(res.Message == "Missing required information passed");
        }

        [Test]
        public async Task ShouldNot_CreateRoom_IfMissingRoom_PriceValueInformation()
        {
            var command = new CreateRoomCommand
            {
                CreateRoomRequest = new Application.Room.Requests.CreateRoomRequest
                {
                    Data = new Application.Room.DTO.RoomDTO
                    {
                        InMaintenance = false,
                        Level = 1,
                        Name = "Room Name",
                        PriceCurrency = Domain.Room.Enums.AcceptedCurrencies.USD,
                        PriceValue = 0,
                    }
                }
            };

            var handler = GetMockCommandHandler(null);

            var res = await handler.Handle(command, CancellationToken.None);

            Assert.NotNull(res);
            Assert.IsFalse(res.Sucess);
            Assert.NotNull(res.ErrorCode == Application.Responses.ErrorCode.MISSING_ROOM_REQUIRED_INFORMATION);
            Assert.NotNull(res.Message == "Missing required information passed");
        }

        [Test]
        public async Task Should_CreateRoom()
        {
            var command = new CreateRoomCommand
            {
                CreateRoomRequest = new Application.Room.Requests.CreateRoomRequest
                {
                    Data = new Application.Room.DTO.RoomDTO
                    {
                        InMaintenance = false,
                        Level = 1,
                        Name = "Room Name",
                        PriceCurrency = Domain.Room.Enums.AcceptedCurrencies.USD,
                        PriceValue = 1000,
                    }
                }
            };

            int fakeRoomId = 1;

            var roomRepository = new Mock<IRoomRepository>();
            roomRepository.Setup(x => x.Create(It.IsAny<Room>()))
                .Returns(Task.FromResult(fakeRoomId));

            var handler = GetMockCommandHandler(roomRepository);

            var res = await handler.Handle(command, CancellationToken.None);

            Assert.NotNull(res);
            Assert.IsTrue(res.Sucess);
            Assert.NotNull(res.Data);
            Assert.NotNull(res.Data.Id == fakeRoomId);
        }
    }
}
