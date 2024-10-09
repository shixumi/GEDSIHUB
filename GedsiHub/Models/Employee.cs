// Employee.cs

using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace GedsiHub.Models
{
    [Table("employee_tbl")] // Table for Employee records
    public class Employee
    {
        [Key]
        [Column("employee_id")]
        public int EmployeeId { get; set; }

        [Required]
        [ForeignKey("User")]
        [Column("user_id")]
        public string UserId { get; set; }
        public virtual ApplicationUser User { get; set; }

        [Column("employee_type", TypeName = "VARCHAR(100)")]
        public string? EmployeeType { get; set; } // Employee Type

        [Column("employment_status", TypeName = "VARCHAR(100)")]
        public string? EmploymentStatus { get; set; } // Employment Status

        [Column("branch_office_section_unit", TypeName = "VARCHAR(100)")]
        public string? BranchOfficeSectionUnit { get; set; } // Employee's specific work unit

        [Column("position", TypeName = "VARCHAR(100)")]
        public string? Position { get; set; } // Job position

        [Column("sector", TypeName = "VARCHAR(100)")]
        public string? Sector { get; set; } // Sector or department

    }
}
