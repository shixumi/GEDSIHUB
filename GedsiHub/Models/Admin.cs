// File: Models/Admin.cs
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace GedsiHub.Models
{
    [Table("admin_tbl")]
    public class Admin
    {
        [Key]
        [Column("admin_id")]
        public int AdminId { get; set; }

        [ForeignKey("User")]
        [Column("user_id")]
        public string UserId { get; set; }
        public ApplicationUser User { get; set; }

        [Column("admin_name")]
        public string AdminName { get; set; }
    }
}
