using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace GedsiHub.Models
{
    [Table("user_login_analytics_tbl")]
    public class UserLogin
    {
        [Key]
        public int LoginId { get; set; }

        [Required]
        [ForeignKey("ApplicationUser")]
        public string UserId { get; set; }
        public virtual ApplicationUser ApplicationUser { get; set; }

        [Required]
        public DateTime LoginTime { get; set; } = DateTime.UtcNow;

    }
}
