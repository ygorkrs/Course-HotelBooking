﻿using Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Data
{
    public class HotelDBContext : DbContext
    {
        public HotelDBContext(DbContextOptions<HotelDBContext> options) : base(options) { }

        public virtual DbSet<Guest> Guests { get; set; }
        public virtual DbSet<Booking> Bookings { get; set; }
        public virtual DbSet<Room> Rooms { get; set; }
    }
}
