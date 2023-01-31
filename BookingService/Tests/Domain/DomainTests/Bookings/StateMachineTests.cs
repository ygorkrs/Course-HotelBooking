using Domain.Entities;
using Domain.Enums;

namespace DomainTests.Bookings
{
    public class Tests
    {
        [SetUp]
        public void Setup()
        {
        }

        [Test]
        public void ShouldAwaysStartWithCreatedStatus()
        {
            var booking = new Booking();
            Assert.That(booking.CurrentStatus, Is.EqualTo(Status.Created));
        }

        [Test]
        public void ShouldSetStatusToPaidWhenPayingForABookingWithCreatedStatus()
        {
            var booking = new Booking();
            booking.ChangeStatus(Actions.Pay);
            Assert.That(booking.CurrentStatus, Is.EqualTo(Status.Paid));
        }

        [Test]
        public void ShouldSetStatusToCanceledWhenCancelingABookingWithCreatedStatus()
        {
            var booking = new Booking();
            booking.ChangeStatus(Actions.Cancel);
            Assert.That(booking.CurrentStatus, Is.EqualTo(Status.Canceled));
        }

        [Test]
        public void ShouldSetStatusToFinishedWhenFinishingAPaidBooking()
        {
            var booking = new Booking();
            booking.ChangeStatus(Actions.Pay);
            booking.ChangeStatus(Actions.Finish);
            Assert.That(booking.CurrentStatus, Is.EqualTo(Status.Finished));
        }

        [Test]
        public void ShouldSetStatusToRefoundedWhenRefoundingAPaidBooking()
        {
            var booking = new Booking();
            booking.ChangeStatus(Actions.Pay);
            booking.ChangeStatus(Actions.Refound);
            Assert.That(booking.CurrentStatus, Is.EqualTo(Status.Refounded));
        }

        [Test]
        public void ShouldSetCreatedWhenReopeningABooking()
        {
            var booking = new Booking();
            booking.ChangeStatus(Actions.Cancel);
            booking.ChangeStatus(Actions.Reopen);
            Assert.That(booking.CurrentStatus, Is.EqualTo(Status.Created));
        }

        [Test]
        public void ShoudNotChangeStatusWhenRefoundABookingWithCreatedStatus()
        {
            var booking = new Booking();
            booking.ChangeStatus(Actions.Refound);
            Assert.That(booking.CurrentStatus, Is.EqualTo(Status.Created));
        }

        [Test]
        public void ShoudNotChangeStatusWhenRefoundABookingWithFinishedStatus()
        {
            var booking = new Booking();
            booking.ChangeStatus(Actions.Pay);
            booking.ChangeStatus(Actions.Finish);
            booking.ChangeStatus(Actions.Refound);
            Assert.That(booking.CurrentStatus, Is.EqualTo(Status.Finished));
        }
    }
}