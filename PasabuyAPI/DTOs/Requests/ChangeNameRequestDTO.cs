namespace PasabuyAPI.DTOs.Requests
{
    public class ChangeNameRequestDTO
    {
        public string FirstName { get; set; } = string.Empty;
        public string? MiddleName { get; set; } = string.Empty;
        public string LastName { get; set; } = string.Empty;
    }
}