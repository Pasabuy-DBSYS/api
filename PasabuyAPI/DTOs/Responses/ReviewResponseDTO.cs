namespace PasabuyAPI.DTOs.Responses
{
    public class ReviewResponseDTO
    {
        public long ReviewIDPK { get; set; }
        public long OrderIDFK { get; set; }
        public long ReviewerIDFK { get; set; }
        public long ReviewedUserID { get; set; }
        public byte Rating { get; set; }
        public string? Comment { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}