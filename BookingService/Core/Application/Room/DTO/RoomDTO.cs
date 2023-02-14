using Domain.Room.Enums;
using Domain.Room.ValueObjects;
using Entities = Domain.Entities;

namespace Application.Room.DTO
{
    public class RoomDTO
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int Level { get; set; }
        public bool InMaintenance { get; set; }
        public decimal PriceValue { get; set; }
        public AcceptedCurrencies PriceCurrency { get; set; }

        public static Entities.Room MapToEntity(RoomDTO roomDTO)
        {
            return new Entities.Room
            {
                Id = roomDTO.Id,
                Name = roomDTO.Name,
                Level = roomDTO.Level,
                InMaintenance = roomDTO.InMaintenance,
                Price = new Price
                {
                    Value = roomDTO.PriceValue,
                    Currency = roomDTO.PriceCurrency,
                },
            };
        }

        public static RoomDTO MapToDTO(Entities.Room room)
        {
            return new RoomDTO
            {
                Id = room.Id,
                Name = room.Name,
                Level = room.Level,
                InMaintenance = room.InMaintenance,
                PriceValue = room.Price.Value,
                PriceCurrency = room.Price.Currency,
            };
        }
    }
}
