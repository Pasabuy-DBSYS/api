using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace PasabuyAPI.DTOs.Requests
{
    public class UserRequestDTO
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

        [MaxLength(100)]
        public string LastName { get; set; } = string.Empty;

        [Required, Phone]
        public string Phone { get; set; } = string.Empty;

        public DateOnly Birthday { get; set; }

        // File uploads handled manually with System.IO
        public IFormFile? FrontId { get; set; }
        public IFormFile? BackId { get; set; }
        public IFormFile? Insurance { get; set; }

        // Saved file paths -> changed later in the services
        public string? FrontIdPath { get; set; }
        public string? BackIdPath { get; set; }
        public string? InsurancePath { get; set; }
    }
}
