using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using KladrTask.Domain.Abstract;
using KladrTask.Domain.Entities;

namespace KladrTask.Domain.Concrete
{
    public class DbKladrRepository : IKladrRepository
    {
        private readonly DbKladrContext kladrContext;

        public IQueryable<Address> Addresses { get { return kladrContext.Addresses; } }
        public IQueryable<User> Users { get { return kladrContext.Users; } }
        public IQueryable<Region> Regions { get { return kladrContext.Regions; } }
        public IQueryable<Road> Roads { get { return kladrContext.Roads; } }
        public IQueryable<House> Houses { get { return kladrContext.Houses; } }

        public void AddUser(User user)
        {
            kladrContext.Users.Add(user);
            kladrContext.SaveChanges();
        }

        public Address GetAddress(Address address)
        {
            var adr = kladrContext.Addresses.ToList().FirstOrDefault(a => a.Equals(address));
            if (adr != null)
            {
                return adr;
            }

            kladrContext.Addresses.Add(address);
            kladrContext.SaveChanges();

            return address;
        }

        public void SaveChanges()
        {
            kladrContext.SaveChanges();
        }

        public User GetUserByLogin(string login)
        {
            return kladrContext.Users.FirstOrDefault(user => user.Login == login);
        }

        public Region GetRegionByCode(string code)
        {
            return Regions.FirstOrDefault(region => region.Code == code);
        }

        public Road GetRoadByCode(string code)
        {
            return Roads.FirstOrDefault(road => road.Code == code);
        }

        public House GetHouseByCode(string code)
        {
            return Houses.FirstOrDefault(house => house.Code == code);
        }

        public DbKladrRepository()
        {
            kladrContext = new DbKladrContext();
        }
    }
}
