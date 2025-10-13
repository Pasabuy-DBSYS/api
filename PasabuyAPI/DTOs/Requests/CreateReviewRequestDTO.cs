using System.ComponentModel.DataAnnotations;

namespace PasabuyAPI.DTOs.Requests
{
    public class CreateReviewRequestDTO
    {
        [Required]
        public long OrderIDFK { get; set; }
        
        [Required]
        public long ReviewedUserID { get; set; }
        
        [Range(1, 5, ErrorMessage = "Rating must be between 1 and 5")]
        public byte Rating { get; set; }
        
        [MaxLength(500, ErrorMessage = "Comment cannot exceed 500 characters")]
        public string? Comment { get; set; }
    }
}