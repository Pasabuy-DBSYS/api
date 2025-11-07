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
        public DbSet<Reviews> Reviews { get; set; }
        public DbSet<VerificationInfo> VerificationInfo { get; set; }
        public DbSet<ChatMessages> ChatMessages { get; set; }
        public DbSet<ChatRooms> ChatRooms { get; set; }
        public DbSet<PhoneVerification> PhoneVerifications { get; set; }
        public DbSet<EmailVerification> EmailVerifications { get; set; }

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
                .HasOne(u => u.VerificationInfo)
                .WithOne(v => v.User)
                .HasForeignKey<VerificationInfo>(v => v.UserIdFK)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Users>()
                .Property(u => u.CurrentRole)
                .HasConversion<string>();

            modelBuilder.Entity<VerificationInfo>()
                .Property(v => v.VerificationInfoStatus)
                .HasConversion<string>();

            modelBuilder.Entity<Orders>()
                .Property(o => o.Priority)
                .HasConversion<string>();

            modelBuilder.Entity<Orders>()
                .Property(o => o.Status)
                .HasConversion<string>();

            modelBuilder.Entity<ChatMessages>()
                .Property(m => m.MessageType)
                .HasConversion<string>();

            modelBuilder.Entity<Payments>()
                .Property(p => p.PaymentStatus)
                .HasConversion<string>();

            modelBuilder.Entity<Payments>()
                .Property(p => p.PaymentMethod)
                .HasConversion<string>();

            modelBuilder.Entity<Orders>()
                .HasOne(o => o.ChatRoom)
                .WithOne(c => c.Order)
                .HasForeignKey<ChatRooms>(c => c.OrderIdFK)
                .OnDelete(DeleteBehavior.Restrict);
                
            modelBuilder.Entity<Payments>()
                .HasIndex(p => p.TransactionId)
                .IsUnique();
        }
    }
}