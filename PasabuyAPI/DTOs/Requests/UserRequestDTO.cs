using System.ComponentModel.DataAnnotations;
using PasabuyAPI.Enums;

namespace PasabuyAPI.DTOs.Requests
{
    public class UserRequestDTO
    {
        [Required, EmailAddress]
        public string Email { get; set; } = string.Empty;

        [Required, MaxLength(50)]
        public string Username { get; set; } = string.Empty;

        [Required, MinLength(6)]
        public string Password { get; set; } = string.Empty; // plain password, hash later

        [Required, MaxLength(100)]
        public string Name { get; set; } = string.Empty;

        [Required, Phone]
        public string Phone { get; set; } = string.Empty;

        public DateOnly Birthday { get; set; }

        public YearLevel YearLevel { get; set; }

        public bool IsActive { get; set; } = true;
    }
}
