using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace GedsiHub.Models
{
    [Table("student_tbl")] // Table for Student records
    public class Student
    {
        [Key]
        [Column("student_id")]
        public int StudentId { get; set; }

        [Required]
        [ForeignKey("User")]
        [Column("user_id")]
        public string UserId { get; set; }
        public virtual ApplicationUser User { get; set; }

        [Column("college", TypeName = "VARCHAR(100)")]
        public string College { get; set; } // Choice field for College (CollegeChoices)

        [Required]
        [Column("college_dept_id", TypeName = "VARCHAR(30)")]
        public string CollegeDeptId { get; set; }

        [Column("program", TypeName = "VARCHAR(100)")]
        public string Program { get; set; } // Program or course

        [Column("year")]
        public int? Year { get; set; } // Year of study

        [Column("section", TypeName = "VARCHAR(10)")]
        public string Section { get; set; } // Section

        [Column("first_name", TypeName = "VARCHAR(30)")]
        public string FirstName { get; set; } // First Name

        [Column("middle_name", TypeName = "VARCHAR(30)")]
        public string MiddleName { get; set; } // Middle Name

        [Column("last_name", TypeName = "VARCHAR(30)")]
        public string LastName { get; set; } // Last Name

        [Column("suffix", TypeName = "VARCHAR(10)")]
        public string Suffix { get; set; } // Suffix

        [Column("lived_name", TypeName = "VARCHAR(30)")]
        public string LivedName { get; set; } // Lived Name

        [Column("phone_number", TypeName = "VARCHAR(15)")]
        public string PhoneNumber { get; set; } // Phone Number

        [Column("birthday", TypeName = "DATE")]
        public DateTime? Birthday { get; set; } // Birthday

        [Column("sex", TypeName = "VARCHAR(20)")]
        public string Sex { get; set; } // Sex (Male, Female, etc.)

        [Column("gender_identity", TypeName = "VARCHAR(20)")]
        public string GenderIdentity { get; set; } // Gender Identity (Cisgender, Transgender, etc.)

        [Column("preferred_pronoun", TypeName = "VARCHAR(20)")]
        public string PreferredPronoun { get; set; } // Preferred Pronoun

        [Column("indigenous_cultural_community", TypeName = "VARCHAR(20)")]
        public string IndigenousCulturalCommunity { get; set; } // Indigenous Status

        [Column("differently_abled", TypeName = "VARCHAR(20)")]
        public string DifferentlyAbled { get; set; } // Differently Abled Status

        // Navigation property for college department
        public virtual CollegeDepartment CollegeDepartment { get; set; }
    }
}
