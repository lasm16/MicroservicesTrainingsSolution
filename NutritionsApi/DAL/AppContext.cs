using Microsoft.EntityFrameworkCore;
using NutritionsApi.DAL.Models;

namespace NutritionsApi.DAL
{
    public class AppContext(DbContextOptions<AppContext> options) : DbContext(options)
    {
        public DbSet<Nutrition> Nutritions { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Nutrition>().HasKey(x => x.Id);
            modelBuilder.Entity<Nutrition>().Property(x => x.Description).HasMaxLength(140);
            base.OnModelCreating(modelBuilder);
        }
    }
}
