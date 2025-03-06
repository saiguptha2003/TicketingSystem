using Microsoft.EntityFrameworkCore;
using System;

namespace TicketBookingSystem.Models
{
    public class TicketContext : DbContext
    {


        public string DbPath { get; }

        // Use dependency injection or configuration to provide the DbPath
        public TicketContext(DbContextOptions<TicketContext> options) : base(options)
        {
            var folder = Environment.SpecialFolder.LocalApplicationData;
            var path = Environment.GetFolderPath(folder);
            DbPath = System.IO.Path.Join(path, "Tickets.db");

            
        }
        public DbSet<User> Users { get; set; } = null!;
        public DbSet<Theater> Theaters { get; set; }= null!;
        public DbSet<Showtime> Showtimes { get; set; }= null!;
        public DbSet<Order> Orders { get; set; }= null!;
        public DbSet<Movie> Movies { get; set; }= null!;

        // Configuring SQLite Database
        protected override void OnConfiguring(DbContextOptionsBuilder options)
        {
            // If options have not been set (this happens when using dependency injection)
            if (!options.IsConfigured)
            {
                options.UseSqlite($"Data Source={DbPath}");
            }
        }

        // Configuring relationships
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<User>()
                .Property(s => s.UserId);
            
            modelBuilder.Entity<Theater>()
                .Property(s => s.TheaterId);
            
            modelBuilder.Entity<Movie>()
                .Property(s => s.MovieId);

            modelBuilder.Entity<Showtime>()
                .HasOne(s => s.Movie)  
                .WithMany(m => m.Showtimes)  
                .HasForeignKey(s => s.MovieId);

            modelBuilder.Entity<Showtime>()
                .HasOne(s => s.Theater)
                .WithMany(t => t.Showtimes) 
                .HasForeignKey(s => s.TheaterId);

            modelBuilder.Entity<Order>()
                .HasOne(o => o.User)  
                .WithMany(u => u.Orders)
                .HasForeignKey(o => o.UserId);

            modelBuilder.Entity<Order>()
                .HasOne(o => o.Showtime)
                .WithMany(s => s.Orders)  
                .HasForeignKey(o => o.ShowTimeId);
        }
    }
}
