using Entities = Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Data.Guest;
using Data.Room;

namespace Data
{
    public class HotelDBContext : DbContext
    {
        public HotelDBContext(DbContextOptions<HotelDBContext> options) : base(options) { }

        public virtual DbSet<Entities.Guest> Guests { get; set; }
        public virtual DbSet<Entities.Booking> Bookings { get; set; }
        public virtual DbSet<Entities.Room> Rooms { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfiguration(new GuestConfiguration());
            modelBuilder.ApplyConfiguration(new RoomConfiguration());
        }
    }
}
