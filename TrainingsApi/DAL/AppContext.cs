using Microsoft.EntityFrameworkCore;
using TrainingsApi.DAL.Models;

namespace TrainingsApi.DAL
{
    public class AppContext(DbContextOptions<AppContext> options) : DbContext(options)
    {
        public DbSet<Training> Trainings { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Training>().HasKey(x => x.Id);
            modelBuilder.Entity<Training>().Property(x => x.Description).HasMaxLength(140);
            base.OnModelCreating(modelBuilder);
        }
    }
}
