// File: Models/ApplicationUser.cs
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;

namespace GedsiHub.Models
{
    public class ApplicationUser : IdentityUser
    {
        // Add custom properties
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string GenderIdentity { get; set; }
        public string Pronouns { get; set; }
        public bool IsMemberOfIndigenousCommunity { get; set; }
        public bool IsDisabled { get; set; }
        public DateTime DateOfBirth { get; set; }
        public byte[] ProfilePicture { get; set; }

        // Navigation properties
        public virtual Student Student { get; set; }
        public virtual Employee Employee { get; set; }
        public virtual Admin Admin { get; set; }
        public virtual ICollection<Enrollment> Enrollments { get; set; }
        public virtual ICollection<UserProgress> UserProgresses { get; set; }
        public virtual ICollection<Certificate> Certificates { get; set; }
        public virtual ICollection<ForumPost> ForumPosts { get; set; }
        public virtual ICollection<ForumComment> ForumComments { get; set; }
    }
}
