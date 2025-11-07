namespace PasabuyAPI.DTOs.Requests
{
    public class VerifyPhoneRequestDTO
    {
        public string PhoneNumber { get; set; }
        public string Code { get; set; }
    }
}