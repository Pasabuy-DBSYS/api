using System.ComponentModel.DataAnnotations;

namespace PasabuyAPI.DTOs.Requests
{
    public class VerifyPhoneRequestDTO
    {
        [Phone]
        public required string PhoneNumber { get; set; }
        public required string Code { get; set; }
    }
}