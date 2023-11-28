using Microsoft.EntityFrameworkCore;
using ReactWithASP.Server.Models;
using ReactWithASP.Server.Services;
using System.Security.Cryptography;
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
        public DbSet<UserAccount> UserAccount { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<User>().HasKey(u => new { u.UserId });
            modelBuilder.Entity<User>().Property(u => u.UserId).ValueGeneratedNever();

            modelBuilder.Entity<UserAccount>().HasKey(u => new { u.UserId });
            modelBuilder.Entity<UserAccount>().Property(u =>  u.UserId).ValueGeneratedNever();


            modelBuilder.Entity<ConnectionStrings>().HasKey(u => new { u.Id });

            modelBuilder.Entity<ConnectionStrings>()
           .HasOne(cs => cs.User)
           .WithMany(u => u.ConnectionStrings)
           .HasForeignKey(cs => cs.UserId)
           .OnDelete(DeleteBehavior.Cascade); // Cascade delete if a user is deleted

            modelBuilder.Entity<UserAccount>().HasKey(u => new { u.UserId });

            modelBuilder.Entity<User>().HasData(new User
            {
                UserId = Guid.Parse("D63F0CA3-E25D-4583-9354-57F110538F45"),
                Email = "dev@tuckerjohnson.me",
                FirstName = "Tucker",
                LastName = "Johnson",
                Role = "SuperUser"
            });

            modelBuilder.Entity<UserAccount>().HasData(new UserAccount
            {
                UserId = Guid.Parse("D63F0CA3-E25D-4583-9354-57F110538F45"),
                Email = "dev@tuckerjohnson.me",
                PasswordHash = BC.HashPassword("Password1!"),
                RefreshToken = Convert.ToBase64String(RandomNumberGenerator.GetBytes(64)),
                TokenExpires = DateTime.UtcNow.AddMinutes(30),
                TokenCreated = DateTime.UtcNow

            });

            modelBuilder.Entity<User>().HasData(new User
            {
                UserId = Guid.Parse("D63F0CA3-E25D-4583-9354-57F110538A55"),
                Email = "hashtimemail@gmail.com",
                FirstName = "Tucker",
                LastName = "Johnson",
                Role = "Admin"
            });

            modelBuilder.Entity<UserAccount>().HasData(new UserAccount
            {
                UserId = Guid.Parse("D63F0CA3-E25D-4583-9354-57F110538A55"),
                Email = "hashtimemail@gmail.com",
                PasswordHash = BC.HashPassword("Password1!"),
                RefreshToken = Convert.ToBase64String(RandomNumberGenerator.GetBytes(64)),
                TokenExpires = DateTime.UtcNow.AddMinutes(30),
                TokenCreated = DateTime.UtcNow
            });



            base.OnModelCreating(modelBuilder);
        }

    }
}
