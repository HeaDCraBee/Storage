using Microsoft.EntityFrameworkCore;
using Storage.Domain;

namespace Storage.DAL
{
    internal class StorageDbContext : DbContext
    {
        
        public DbSet<Pallet> Pallets { get; set; }

        public DbSet<Box> Boxes { get; set; }

        public StorageDbContext() : base()
        {
            Database.EnsureCreated();
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer(@"Server=(localdb)\mssqllocaldb;Database=storage;Trusted_Connection=True;");
        }
    }
}
