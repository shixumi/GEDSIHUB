// File: Models/CollegeDepartment.cs
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace GedsiHub.Models
{
    [Table("college_dept_tbl")]
    public class CollegeDepartment
    {
        [Key]
        [Column("college_dept_id")]
        public string CollegeDeptId { get; set; }

        [Column("college_name")]
        public string CollegeName { get; set; }
    }
}
