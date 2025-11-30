using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace PasabuyAPI.Models
{
    [Index(nameof(OrderIDFK), nameof(ReviewerIDFK), IsUnique = true)]
    public class Reviews
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long ReviewIDPK { get; set; }
        [ForeignKey("Order")]
        public long OrderIDFK { get; set; }
        public Orders Order { get; set; }
        [ForeignKey("Reviewer")]
        public long ReviewerIDFK { get; set; }
        public Users Reviewer { get; set; }
        [ForeignKey("Reviewed")]
        public long ReviewedUserID { get; set; }
        public Users Reviewed { get; set; }

        [Range(1, 5)]
        public byte Rating { get; set; }

        [MaxLength(500)]
        public string? Comment { get; set; }

        public DateTime CreatedAt { get; set; }
    }
}
