namespace BugTrackingSystem.Models
{
    public class Project
    {
        public int ProjectId { get; set; }
        public string Title { get; set; }
        public List<Bug> Bugs { get; set; } = new List<Bug>();
    }
}
