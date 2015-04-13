using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using KladrTask.Domain.Entities;

namespace KladrTask.Domain.Abstract
{
    public interface IKladrRepository
    {
        IQueryable<Address> Addresses { get; }
        IQueryable<User> Users { get; }
        IQueryable<Region> Regions { get; }
        IQueryable<Road> Roads { get; }
        IQueryable<House> Houses { get; }

        void AddUser(User user);
        void AddAddress(Address address);
        void SaveChanges();
        User GetUserByLogin(string login);
        Region GetRegionByCode(string code);
        Road GetRoadByCode(string code);
        House GetHouseByCode(string code);
    }
}
