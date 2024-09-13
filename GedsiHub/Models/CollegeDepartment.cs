using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Collections.Generic;

namespace GedsiHub.Models
{
    [Table("college_dept_tbl")]
    public class CollegeDepartment
    {
        [Key]
        [Column("college_dept_id", TypeName = "VARCHAR(30)")]
        public string CollegeDeptId { get; set; }

        [Column("college_name", TypeName = "VARCHAR(100)")]
        public string CollegeName { get; set; }

        // **Added Navigation Property**
        public virtual ICollection<Student> Students { get; set; } = new List<Student>();
    }
}
