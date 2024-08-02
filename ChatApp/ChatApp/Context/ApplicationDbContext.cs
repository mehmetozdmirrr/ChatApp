using Microsoft.EntityFrameworkCore;
using ChatApp.Models;

namespace ChatApp.Context;

    public sealed class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions options) : base(options)
        {

        }

        public DbSet<User> Users { get; set; }
        public DbSet<Chat> Chats { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {


            modelBuilder.Entity<User>(entity =>
            {
                entity.ToTable("Users");
                entity.HasKey(e => e.Id);
                entity.Property(e => e.Name).IsRequired().HasMaxLength(100);
                entity.Property(e => e.Password).IsRequired();
                entity.Property(e => e.Salt).IsRequired();
                entity.Property(e => e.Avatar).HasMaxLength(500);
                entity.Property(e => e.Status).IsRequired().HasMaxLength(100);
                entity.Property(e => e.Role).HasMaxLength(50);
                entity.Property(e => e.refreshToken).IsRequired();
                entity.Property(e => e.ExpiryDate).IsRequired(false);
            });

            modelBuilder.Entity<Chat>(entity =>
            {
                entity.ToTable("Chats");
                entity.HasKey(e => e.Id);
                entity.Property(e => e.UserId).IsRequired();
                entity.Property(e => e.toUserId).IsRequired();
                entity.Property(e => e.Message).IsRequired();
                entity.Property(e => e.Image).HasMaxLength(500);
                entity.Property(e => e.IsRead).IsRequired();
                entity.Property(e => e.Date).IsRequired();
            });
        }
    }
