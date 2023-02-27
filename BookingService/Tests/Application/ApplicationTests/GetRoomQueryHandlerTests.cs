using Application.Room.Queries;
using Domain.Entities;
using Domain.Room.Ports;
using Moq;

namespace ApplicationTests
{
    [TestFixture]
    internal class GetRoomQueryHandlerTests
    {
        [Test]
        public async Task Should_Return_Room()
        {
            var query = new GetRoomQuery { Id = 1 };

            var fakeRoom = new Room
            {
                Id = query.Id,
                Name = "Room Name",
                Level = 1,
                InMaintenance = false,
                Price = new Domain.Room.ValueObjects.Price
                {
                    Currency = Domain.Room.Enums.AcceptedCurrencies.EUR,
                    Value = 1000
                }
            };

            var roomRepository = new Mock<IRoomRepository>();
            roomRepository.Setup(x => x.Get(query.Id))
                .Returns(Task.FromResult(fakeRoom));

            var handler = new GetRoomQueryHandler(roomRepository.Object);

            var res = await handler.Handle(query, CancellationToken.None);

            Assert.NotNull(res);
            Assert.True(res.Sucess);
            Assert.NotNull(res.Data);
            Assert.AreEqual(query.Id, res.Data.Id);
        }

        [Test]
        public async Task Should_Return_Room_NotFound()
        {
            var query = new GetRoomQuery { Id = 1 };

            var roomRepository = new Mock<IRoomRepository>();

            var handler = new GetRoomQueryHandler(roomRepository.Object);

            var res = await handler.Handle(query, CancellationToken.None);

            Assert.NotNull(res);
            Assert.False(res.Sucess);
            Assert.NotNull(res.ErrorCode == Application.Responses.ErrorCode.NOT_FOUND_ROOM);
            Assert.NotNull(res.Message == "Room not found for the given Id");
        }
    }
}
