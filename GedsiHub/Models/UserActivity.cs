// Models/UserActivity.cs
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

public class UserActivity
{
    [Key]
    public int Id { get; set; }

    [Required]
    public string UserId { get; set; }

    [Required]
    public int ModuleId { get; set; }

    [Required]
    public string ActivityType { get; set; }

    public double? Score { get; set; }
    public bool? Success { get; set; }

    public double? TimeSpentSeconds { get; set; }

    public DateTime Timestamp { get; set; }
}
