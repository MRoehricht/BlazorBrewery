using BlazorBreweryDatabase.Entities;
using Microsoft.EntityFrameworkCore;

namespace BlazorBreweryDatabase.Context
{
    public class UserContext : DbContext
    {

        public DbSet<User> Users { get; set; }

        public UserContext(DbContextOptions<UserContext> dbContextOptions) : base(dbContextOptions)
        {
        }

        //protected override void OnModelCreating(ModelBuilder modelBuilder)
        //{
        //    base.OnModelCreating(modelBuilder);

        //    modelBuilder.Entity<User>(entity =>
        //    {
        //        entity.HasKey(e => e.Id);
        //        entity.Property(e => e.Name).IsRequired();
        //        entity.Property(e => e.Count).HasDefaultValue(0);
        //    });
        //}

    }
}
