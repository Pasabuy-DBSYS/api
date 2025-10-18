using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;
using PasabuyAPI.Enums;

namespace PasabuyAPI.Models
{
    [Index(nameof(Email), IsUnique = true)]
    [Index(nameof(Username), IsUnique = true)]
    [Index(nameof(Phone), IsUnique = true)]
    public class Users
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long UserIdPK { get; set; }

        [Required, EmailAddress]
        public string Email { get; set; } = string.Empty;

        [Required, MaxLength(50)]
        public string Username { get; set; } = string.Empty;

        [Required]
        public string PasswordHash { get; set; } = string.Empty;

        [Required, MaxLength(100)]
        public string FirstName { get; set; } = string.Empty;

        [MaxLength(100)]
        public string? MiddleName { get; set; } = string.Empty;

        [Required, MaxLength(100)]
        public string LastName { get; set; } = string.Empty;

        [Phone]
        public string Phone { get; set; } = string.Empty;

        public DateOnly Birthday { get; set; }


        public Roles CurrentRole { get; set; }

        [Range(0, 5)]
        [Column(TypeName = "decimal(2,1)")]
        public decimal RatingAverage { get; set; } = 0m;

        public long TotalDeliveries { get; set; } = 0;
        public long TotalOrders { get; set; } = 0;

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;
        public required VerificationInfo VerificationInfo { get; set; }
        public List<Orders> CustomerOrders { get; set; } = [];
        public List<Orders> CourierOrders { get; set; } = [];
    }
}
