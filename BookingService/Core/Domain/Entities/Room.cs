using Domain.Exceptions;
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
            // check if there is some booking for this room
            get { return true; }
        }

        private void IsValid()
        {
            if (string.IsNullOrEmpty(Name) ||
                Level < 1 ||
                Price.Value < 1 ||
                Price.Currency == 0)
            {
                throw new MissingRequiredInformationException();
            }
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
    }
}
