using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using PasabuyAPI.Enums;

namespace PasabuyAPI.Models
{
    public class VerificationInfo
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long VerifiactionInfoId { get; set; }
        public long UserIdFK { get; set; }
        public Users User { get; set; } = null!;
        public string FrontIdPath { get; set; } = string.Empty;
        public string BackIdPath { get; set; } = string.Empty;
        public string InsurancePath { get; set; } = string.Empty;
        public VerificationInfoStatus VerificationInfoStatus { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
    }
}