﻿// Module.cs
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using GedsiHub.Models.Quiz;

namespace GedsiHub.Models
{
    public enum ModuleStatus
    {
        Unpublished = 0,
        Published = 1
    }

    public class Module
    {
        [Key]
        public int ModuleId { get; set; }

        [Required]
        [StringLength(100)]
        public string Title { get; set; } = string.Empty;

        [Required]
        public string Description { get; set; } = string.Empty;

        public int PositionInt { get; set; }

        public DateTime CreatedDate { get; set; } = DateTime.UtcNow;

        public DateTime? LastModifiedDate { get; set; }

        [Required]
        public ModuleStatus Status { get; set; } = ModuleStatus.Unpublished;

        [Required]
        [StringLength(7, MinimumLength = 7, ErrorMessage = "Color must be a valid hex code.")]
        public string Color { get; set; } = "#000000";

        // New Property
        public bool IsCertificateEnabled { get; set; } = true; // Default is enabled

        // Navigation Properties
        public Exam? Exam { get; set; }
        public virtual ICollection<Lesson> Lessons { get; set; } = new List<Lesson>();
        public virtual ICollection<UserProgress> UserProgresses { get; set; } = new List<UserProgress>();
        public virtual ICollection<Certificate> Certificates { get; set; } = new List<Certificate>();
        public virtual ICollection<Enrollment> Enrollments { get; set; } = new List<Enrollment>();
    }

}
