using Enums = Domain.Booking.Enums;
using Domain.Room.Exceptions;
using Domain.Room.Ports;
using Domain.Room.ValueObjects;

namespace Domain.Entities
{
    public class Room
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int Level { get; set; }
        public bool InMaintenance { get; set; }
        public Price Price { get; set; }
        public ICollection<Booking> Bookings { get; set; }

        public bool IsAvailable 
        {
            get {
                if (this.InMaintenance || this.HasGuest)
                    return false;

                return true;
            }
        }

        public bool HasGuest 
        {
            get {
                var nonAvailableStates = new List<Enums.Status>()
                {
                    Enums.Status.Created,
                    Enums.Status.Paid,
                };

                return this.Bookings.Where(
                    bk => bk.Room.Id == this.Id &&
                    nonAvailableStates.Contains(bk.Status)).Count() > 0;
            }
        }

        private void IsValid()
        {
            if (string.IsNullOrEmpty(Name) ||
                Level < 1 ||
                Price.Value < 1 ||
                Price.Currency == 0)
            {
                throw new MissingRoomRequiredInformationException();
            }
        }

        public bool CheckIfIsValid()
        {
            this.IsValid();
            return true;
        }

        public async Task Save(IRoomRepository roomRepository)
        {
            this.IsValid();

            if (this.Id == 0)
            {
                this.Id = await roomRepository.Create(this);
            }
            else
            {
                //await roomRepository.Update(this);
            }
        }

        public bool CanBeBooked()
        {
            try
            {
                this.IsValid();
            }
            catch (Exception ex)
            {
                return false;
            }

            if (!this.IsAvailable)
            {
                return false;
            }

            return true;
        }
    }
}
