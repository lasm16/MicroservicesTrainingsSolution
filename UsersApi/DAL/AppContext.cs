using Microsoft.EntityFrameworkCore;
using UsersApi.DAL.Models;

namespace UsersApi.DAL
{
    public class AppContext(DbContextOptions<AppContext> options) : DbContext(options)
    {
        public DbSet<User> Users { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<User>().HasKey(x => x.Id);
            modelBuilder.Entity<User>().Property(x => x.Name).HasMaxLength(140);
            base.OnModelCreating(modelBuilder);
        }
    }
}
