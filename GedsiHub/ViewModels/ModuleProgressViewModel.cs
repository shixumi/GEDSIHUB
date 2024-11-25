using GedsiHub.Models;

namespace GedsiHub.ViewModels
{
    public class ModuleProgressViewModel
    {
        public Module Module { get; set; }
        public decimal ProgressPercentage { get; set; }
        public bool IsUnlocked { get; set; }

        // Expose Module properties directly
        public int ModuleId => Module.ModuleId;
        public string Title => Module.Title;
        public string Color => Module.Color;
        public ModuleStatus Status => Module.Status;
        public ICollection<Lesson> Lessons => Module.Lessons;
    }
}
