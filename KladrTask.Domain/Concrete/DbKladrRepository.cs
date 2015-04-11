using System.Linq;
using KladrTask.Domain.Abstract;
using KladrTask.Domain.Entities;
using Microsoft.SqlServer.Server;

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

        public void AddAddress(Address address)
        {
            kladrContext.Addresses.Add(address);
            kladrContext.SaveChanges();
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
