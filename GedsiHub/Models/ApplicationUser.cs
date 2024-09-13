using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace GedsiHub.Models
{
    [Table("user_tbl")]
    public class ApplicationUser : IdentityUser
    {
        [Required]
        [StringLength(50)]
        [Column("first_name")]
        public string FirstName { get; set; } = string.Empty;

        [Required]
        [StringLength(50)]
        [Column("last_name")]
        public string LastName { get; set; } = string.Empty;

        [StringLength(30)]
        [Column("gender_identity")]
        public string GenderIdentity { get; set; } = string.Empty;

        [StringLength(30)]
        [Column("pronouns")]
        public string Pronouns { get; set; } = string.Empty;

        [Column("is_indigenous_member")]
        public bool IsMemberOfIndigenousCommunity { get; set; }

        [Column("is_disabled")]
        public bool IsDisabled { get; set; }

        [Required]
        [Column("date_of_birth", TypeName = "DATE")]
        public DateTime DateOfBirth { get; set; }

        [StringLength(200)]
        [Column("profile_picture_path")]
        public string ProfilePicturePath { get; set; } = string.Empty;

        // Navigation properties
        public virtual Student Student { get; set; }
        public virtual Employee Employee { get; set; }
        public virtual Admin Admin { get; set; }
        public virtual ICollection<Enrollment> Enrollments { get; set; } = new List<Enrollment>();
        public virtual ICollection<UserProgress> UserProgresses { get; set; } = new List<UserProgress>();
        public virtual ICollection<Certificate> Certificates { get; set; } = new List<Certificate>();
        public virtual ICollection<ForumPost> ForumPosts { get; set; } = new List<ForumPost>();
        public virtual ICollection<ForumComment> ForumComments { get; set; } = new List<ForumComment>();
    }
}
