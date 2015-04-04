using System.Data.Entity;
using KladrTask.Domain.Entities;

namespace KladrTask.Domain.Concrete
{
    public class DbKladrContext:DbContext
    {
        public DbSet<User> Users { get; set; }
        public DbSet<Address> Addresses { get; set; }
        public DbSet<Region> Regions { get; set; }
        public DbSet<Road> Roads { get; set; }
        public DbSet<House> Houses { get; set; }
    }
}
