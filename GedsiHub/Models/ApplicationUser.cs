// ApplicationUser.cs

using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace GedsiHub.Models
{
    [Table("user_tbl")] // Custom table name for Identity user table
    public class ApplicationUser : IdentityUser
    {
        [Required]
        [StringLength(50)]
        [Column("first_name")]
        public string FirstName { get; set; } = string.Empty; // First Name

        [Required]
        [StringLength(50)]
        [Column("last_name")]
        public string LastName { get; set; } = string.Empty; // Last Name

        [StringLength(30)]
        [Column("gender_identity", TypeName = "VARCHAR(30)")]
        public string GenderIdentity { get; set; } = string.Empty; // Gender Identity (Cisgender, Transgender, etc.)

        [StringLength(30)]
        [Column("pronouns", TypeName = "VARCHAR(30)")]
        public string Pronouns { get; set; } = string.Empty; // Preferred Pronoun (He/Him, She/Her, etc.)

        [Column("is_indigenous_member")]
        public bool IsMemberOfIndigenousCommunity { get; set; } // Indigenous Community Status

        [Column("is_disabled")]
        public bool IsDisabled { get; set; } // Differently Abled Status

        [Required]
        [Column("date_of_birth", TypeName = "DATE")]
        public DateTime DateOfBirth { get; set; } // Birthday

        [StringLength(200)]
        [Column("profile_picture_path")]
        public string ProfilePicturePath { get; set; } = string.Empty; // Profile Picture Path

        [Column("is_active")]
        public bool IsActive { get; set; } = false; // Account activation status

        [Column("role", TypeName = "VARCHAR(20)")]
        public string Role { get; set; } = "Student"; // Role (Student or Employee)

        // Navigation properties for extended tables
        public virtual Admin Admin { get; set; }  // Ensure Admin is referenced properly
        public virtual Student Student { get; set; }
        public virtual Employee Employee { get; set; }

        // Other navigation properties
        public virtual ICollection<Enrollment> Enrollments { get; set; } = new List<Enrollment>();
        public virtual ICollection<UserProgress> UserProgresses { get; set; } = new List<UserProgress>();
        public virtual ICollection<Certificate> Certificates { get; set; } = new List<Certificate>();
        public virtual ICollection<ForumPost> ForumPosts { get; set; } = new List<ForumPost>();
        public virtual ICollection<ForumComment> ForumComments { get; set; } = new List<ForumComment>();
        public virtual ICollection<ForumPostReport> ForumPostReports { get; set; } = new List<ForumPostReport>();
        public virtual ICollection<ForumCommentReport> ForumCommentReports { get; set; } = new List<ForumCommentReport>();
    }
}
