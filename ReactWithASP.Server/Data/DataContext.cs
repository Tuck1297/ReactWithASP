using Microsoft.EntityFrameworkCore;
using ReactWithASP.Server.Models;
using BC = BCrypt.Net.BCrypt;

namespace ReactWithASP.Server.Data
{
    public class DataContext : DbContext
    {
        public DataContext(DbContextOptions options) :base(options)
        {
            
        }
        // define our tables

        public DbSet<User> Users { get; set; }
        public DbSet<ConnectionStrings> ConnectionStrings { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<User>().HasKey(u => new { u.UserId });

            modelBuilder.Entity<ConnectionStrings>().HasKey(u => new { u.Id });

            modelBuilder.Entity<ConnectionStrings>()
           .HasOne(cs => cs.User)
           .WithMany(u => u.ConnectionStrings)
           .HasForeignKey(cs => cs.UserId)
           .OnDelete(DeleteBehavior.Cascade); // Cascade delete if a user is deleted

            modelBuilder.Entity<User>().HasData(new User
            {
                UserId = Guid.NewGuid(),
                Email = "dev@tuckerjohnson.me",
                FirstName = "Tucker",
                LastName = "Johnson",
                PasswordHash = BC.HashPassword("Password1!"),
                Role = "SuperUser"
            });

            modelBuilder.Entity<User>().HasData(new User
            {
                UserId = Guid.NewGuid(),
                Email = "hashtimemail@gmail.com",
                FirstName = "Tucker",
                LastName = "Johnson",
                PasswordHash = BC.HashPassword("Password1!"),
                Role = "Admin"
            });

            base.OnModelCreating(modelBuilder);
        }

    }
}
