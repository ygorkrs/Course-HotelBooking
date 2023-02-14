﻿using Entities = Domain.Entities;

namespace Domain.Room.Ports
{
    public interface IRoomRepository
    {
        Task<Entities.Room?> Get(int id);
        Task<int> Create(Entities.Room room);
    }
}
