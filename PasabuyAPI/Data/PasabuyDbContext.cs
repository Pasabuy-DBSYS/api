using Microsoft.EntityFrameworkCore;
using PasabuyAPI.Models;

namespace PasabuyAPI.Data
{
    public class PasabuyDbContext(DbContextOptions<PasabuyDbContext> options) : DbContext(options)
    {
        public DbSet<Users> Users { get; set; }
        public DbSet<Orders> Orders { get; set; }
        public DbSet<DeliveryDetails> DeliveryDetails { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Orders>()
                .HasOne(o => o.Customer)
                .WithMany(u => u.CustomerOrders)
                .HasForeignKey(o => o.CustomerId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Orders>()
                .HasOne(o => o.Courier)
                .WithMany(u => u.CourierOrders)
                .HasForeignKey(o => o.CourierId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Orders>()
                .HasOne(o => o.DeliveryDetails)
                .WithOne(d => d.Order)
                .HasForeignKey<DeliveryDetails>(d => d.OrderIdPK)
                .IsRequired(false);
        }
    }
}