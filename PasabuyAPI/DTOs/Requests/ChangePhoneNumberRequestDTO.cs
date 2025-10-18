using System.ComponentModel.DataAnnotations;

namespace PasabuyAPI.DTOs.Requests
{
    public class ChangePhoneNumberRequestDTO
    {
        [Required]
        [Phone]
        public string Phone { get; set; } = string.Empty;
    }
}