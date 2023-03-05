using BlazorBrewery.Database.Entities;
using Microsoft.EntityFrameworkCore;

namespace BlazorBrewery.Database.Context
{
    public class ConfigContext : DbContext
    {
        public DbSet<ConfigEntity> Configurationitems { get; set; }

        public ConfigContext(DbContextOptions<ConfigContext> dbContextOptions) : base(dbContextOptions)
        {
        }
    }
}
