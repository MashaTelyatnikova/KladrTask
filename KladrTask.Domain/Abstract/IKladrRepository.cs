﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using KladrTask.Domain.Entities;

namespace KladrTask.Domain.Abstract
{
    public interface IKladrRepository
    {
        IQueryable<User> Users { get; }
        IQueryable<Address> Addresses { get; }
        IQueryable<Region> Regions { get; }
        IQueryable<Road> Roads { get; }
        IQueryable<House> Houses { get; }
    }
}