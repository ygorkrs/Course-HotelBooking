﻿using Domain.ValueObjects;

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
    }
}
