// Models/XApiStatement.cs
using GedsiHub.Models;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

public class XApiStatement
{
    [Key]
    public int Id { get; set; }

    public DateTime Timestamp { get; set; }

    public string ActorName { get; set; }
    public string ActorMbox { get; set; }
    public string ActorAccountHomePage { get; set; }
    public string ActorAccountName { get; set; }

    public string VerbId { get; set; }
    public string VerbDisplay { get; set; }

    public string ObjectId { get; set; }
    public string ObjectDefinitionName { get; set; }
    public string ObjectDefinitionDescription { get; set; }

    public double? ResultScore { get; set; }
    public bool? ResultSuccess { get; set; }

    public string ContextRegistration { get; set; }

    public string AdditionalData { get; set; }

    [ForeignKey("User")]
    public string UserId { get; set; }
    public virtual ApplicationUser User { get; set; }

    [ForeignKey("Module")]
    public int? ModuleId { get; set; }
    public virtual Module Module { get; set; }

}
