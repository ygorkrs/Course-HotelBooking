using Domain.Booking.Enums;
using Domain.Booking.Exceptions;
using Domain.Booking.Ports;
using Action = Domain.Booking.Enums.Actions;

namespace Domain.Entities
{
    public class Booking
    {
        public int Id { get; set; }
        public DateTime PlacedAt { get; set; }
        public DateTime Start { get; set; }
        public DateTime End { get; set; }
        public Room Room { get; set; }
        public Guest Guest { get; set; }
        public Status Status { get; set; }

        public Booking()
        {
            this.Status = Status.Created;
            this.PlacedAt = DateTime.UtcNow;
        }

        public void ChangeStatus(Action action)
        {
            this.Status = (this.Status, action) switch
            {
                (Status.Created,  Action.Pay)       => Status.Paid,
                (Status.Created,  Action.Cancel)    => Status.Canceled,
                (Status.Paid,     Action.Finish)    => Status.Finished,
                (Status.Paid,     Action.Refound)   => Status.Refounded,
                (Status.Canceled, Action.Reopen)    => Status.Created,
                _ => this.Status
            };
        }

        private void IsValid()
        {
            if (this.PlacedAt == default(DateTime))
            {
                throw new MissingPlaceAtIformationException();
            }
            if (this.Start == default(DateTime))
            {
                throw new MissingStartInformationException(); 
            }
            if (this.End == default(DateTime))
            {
                throw new MissingEndInformationException();
            }
            if (this.Room == null)
            {
                throw new MissingRoomInformationException();
            }
            if (this.Guest == null)
            {
                throw new MissingGuestInformationException();
            }
            if (!this.Room.CanBeBooked())
            {
                throw new RoomCannotBeBookedException();
            }
            this.Guest.CheckIfIsValid();
        }

        public async Task Save(IBookingRepository bookingRepository)
        {
            this.IsValid();

            if (this.Id == 0)
            {
                var resp = await bookingRepository.Create(this);
                this.Id = resp.Id;
            }
            else
            {
                // update
            }
        }
    }
}
