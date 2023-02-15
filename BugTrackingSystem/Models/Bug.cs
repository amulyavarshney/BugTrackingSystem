namespace BugTrackingSystem.Models
{
    public enum BugState
    {
        OPEN,
        WORKING,
        RESOLVED
    }
    public class Bug
    {
        public int BugId { get; set; }
        public string Title { get; set; }
        public BugState State { get; set; }
        public int ProjectId { get; set; }
        public Project Project {get; set;}
        public List<Message> Messages { get; set; } = new List<Message>();
    }
}
