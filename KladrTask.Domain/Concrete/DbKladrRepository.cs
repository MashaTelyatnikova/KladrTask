using System.Linq;
using KladrTask.Domain.Abstract;
using KladrTask.Domain.Entities;

namespace KladrTask.Domain.Concrete
{
    public class DbKladrRepository : IKladrRepository
    {
        private readonly DbKladrContext kladrContext;

        public IQueryable<User> Users { get { return kladrContext.Users; } }
        public IQueryable<Address> Addresses { get { return kladrContext.Addresses; } }
        public IQueryable<Region> Regions { get { return kladrContext.Regions; } }
        public IQueryable<Road> Roads { get { return kladrContext.Roads; } }
        public IQueryable<House> Houses { get { return kladrContext.Houses; } }

        public DbKladrRepository()
        {
            kladrContext = new DbKladrContext();
        }
    }
}
