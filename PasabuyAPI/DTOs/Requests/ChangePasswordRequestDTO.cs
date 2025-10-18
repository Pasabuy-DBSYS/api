namespace PasabuyAPI.DTOs.Requests
{
    public class ChangePasswordRequestDTO
    {
        public string Password { get; set; } = string.Empty;
        public string ConfirmPassword { get; set; } = string.Empty;
    }
}