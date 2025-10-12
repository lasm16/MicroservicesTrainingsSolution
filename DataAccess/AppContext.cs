using DataAccess.Models;
using Microsoft.EntityFrameworkCore;

namespace DataAccess
{
    public class AppContext : DbContext
    {
        public DbSet<Achievement> Achievements { get; set; }
        public DbSet<Nutrition> Nutritions { get; set; }
        public DbSet<Training> Trainings { get; set; }
        public DbSet<User> Users { get; set; }

        public AppContext(DbContextOptions options) : base(options)
        {
            Database.EnsureCreated();
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Nutrition>().HasKey(x => x.Id);
            modelBuilder.Entity<Nutrition>().Property(x => x.Description).HasMaxLength(140);

            modelBuilder.Entity<Training>().HasKey(x => x.Id);
            modelBuilder.Entity<Training>().Property(x => x.Description).HasMaxLength(140);

            modelBuilder.Entity<User>().HasKey(x => x.Id);
            modelBuilder.Entity<User>().Property(x => x.Name).HasMaxLength(140);

            modelBuilder.Entity<Achievement>().HasKey(x => x.Id);
            base.OnModelCreating(modelBuilder);
        }
    }
}
