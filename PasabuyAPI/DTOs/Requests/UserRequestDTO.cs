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
        public string FirstName { get; set; } = string.Empty;
        [MaxLength(100)]
        public string? MiddleName { get; set; } = string.Empty;
        [MaxLength(100)]
        public string LastName { get; set; } = string.Empty;

        [Required, Phone]
        public string Phone { get; set; } = string.Empty;

        public DateOnly Birthday { get; set; }

        public bool IsActive { get; set; } = true;

        public string FrontIdPath { get; set; } = string.Empty;
        public string BackIdPath { get; set; } = string.Empty;
        public string InsurancePath { get; set; } = string.Empty;
    }
}
