using System.ComponentModel.DataAnnotations;

namespace GedsiHub.Models
{
    public class ContactInfo
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [EmailAddress]
        [StringLength(100)]
        public string SupportEmail { get; set; }

        [Required]
        [StringLength(20)]
        public string PhoneNumber { get; set; }

        [StringLength(100)]
        public string Facebook { get; set; }

        [StringLength(100)]
        public string TikTok { get; set; }

        [StringLength(100)]
        public string Instagram { get; set; }

        [StringLength(100)]
        public string X { get; set; }  // Twitter (X)

        [StringLength(100)]
        public string Website { get; set; }
    }
}
