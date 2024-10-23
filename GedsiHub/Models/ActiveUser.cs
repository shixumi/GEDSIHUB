using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace GedsiHub.Models
{
    [Table("active_user_tbl")]
    public class ActiveUser
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public string UserId { get; set; }

        [Required]
        public string ConnectionId { get; set; } // Added ConnectionId

        public DateTime LastActive { get; set; }
    }
}
