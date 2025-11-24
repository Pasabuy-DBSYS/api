using System.ComponentModel.DataAnnotations;

namespace PasabuyAPI.DTOs.Requests
{
    public class CreateAdminRequestDTO
    {
        [Required, EmailAddress]
        public string Email { get; set; } = string.Empty;

        [Required, MaxLength(50)]
        public string Username { get; set; } = string.Empty;

        [Required, MinLength(6)]
        public string Password { get; set; } = string.Empty;

        [Required, MaxLength(100)]
        public string FirstName { get; set; } = string.Empty;

        [MaxLength(100)]
        public string? MiddleName { get; set; } = string.Empty;

        [Required, MaxLength(100)]
        public string LastName { get; set; } = string.Empty;

        [Required, Phone]
        public string Phone { get; set; } = string.Empty;
        [Required]
        public DateOnly Birthday { get; set; }
    }
}