using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace GedsiHub.Models
{
    [Table("forum_post_like_tbl")]
    public class ForumPostLike
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [ForeignKey("ForumPost")]
        public int PostId { get; set; }
        public ForumPost ForumPost { get; set; }

        [Required]
        [ForeignKey("User")]
        public string UserId { get; set; }
        public ApplicationUser User { get; set; }
    }
}
