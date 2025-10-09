using Microsoft.EntityFrameworkCore;
using PasabuyAPI.Models;

namespace PasabuyAPI.Data
{
    public class PasabuyDbContext(DbContextOptions<PasabuyDbContext> options) : DbContext(options)
    {
        public DbSet<Users> Users { get; set; }
        public DbSet<Orders> Orders { get; set; }
        public DbSet<DeliveryDetails> DeliveryDetails { get; set; }
        public DbSet<Payments> Payments { get; set; }
        public DbSet<VerificationInfo> VerificationInfo { get; set; }

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
                .HasForeignKey<DeliveryDetails>(d => d.OrderIdFK)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Orders>()
                .HasOne(o => o.Payment)
                .WithOne(p => p.Order)
                .HasForeignKey<Payments>(p => p.OrderIdFK)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Users>()
                .HasOne(u => u.VerifiactionInfo)
                .WithOne(v => v.User)
                .HasForeignKey<VerificationInfo>(v => v.UserIdFK)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Payments>()
                .HasIndex(p => p.TransactionId)
                .IsUnique();
        }
    }
}