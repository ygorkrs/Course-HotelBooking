using Application.Guest.DTO;
using Application.Room.DTO;
using Domain.Booking.Enums;
using Entities = Domain.Entities;

namespace Application.Booking.DTO
{
    public class BookingDTO
    {
        public BookingDTO()
        {
            this.PlacedAt = DateTime.UtcNow;
        }

        public int Id { get; set; }
        public DateTime PlacedAt { get; set; }
        public DateTime Start { get; set; }
        public DateTime End { get; set; }
        public int RoomId { get; set; }
        public int GuestId { get; set; }
        public Status CurrentStatus { get; set; }

        public static Entities.Booking MaptoEntity(BookingDTO bookingDTO)
        {
            return new Entities.Booking
            {
                Id = bookingDTO.Id,
                Start = bookingDTO.Start,
                End = bookingDTO.End,
                Room = new Entities.Room { Id = bookingDTO.RoomId },
                Guest = new Entities.Guest { Id = bookingDTO.GuestId },
                //CurrentStatus = bookingDTO.CurrentStatus
            };
        }

        public static BookingDTO MapToDTO (Entities.Booking booking)
        {
            return new BookingDTO
            {
                Id = booking.Id,
                PlacedAt = booking.PlacedAt,
                Start = booking.Start,
                End = booking.End,
                RoomId = booking.Room.Id,
                GuestId = booking.Guest.Id,
                CurrentStatus = booking.CurrentStatus,
            };
        }
    }
}
