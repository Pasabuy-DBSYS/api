namespace PasabuyAPI.DTOs.Requests
{
    public class VerificationInfoRequestDTO
    {
        public long UserIdFK { get; set; }
        public string FrontIdPath { get; set; } = string.Empty;
        public string BackIdPath { get; set; } = string.Empty;
        public string InsurancePath { get; set; } = string.Empty;
    }
}