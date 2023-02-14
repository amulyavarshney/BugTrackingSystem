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
        public BugState State { get; set; }
        public List<ProjectBug> Bugs {get; set;}
        public List<BugMessage> Messages { get; set; }
    }
    public class Message
    {
        public int MessageId { get; set; }
        public bool Flag { get; set; }
        public string content { get; set; }
        public List<BugMessage> Messages { get; set; }
    }

    public class BugMessage
    {
        public int BugId { get; set; }
        public Bug Bug { get; set; }
        public int MessageId { get; set; }
        public Message Message { get; set; }
    }

    public class Project
    {
        public int ProjectId { get; set; }
        public List<ProjectBug> Bugs { get; set; }
    }

    public class ProjectBug
    {
        public int ProjectId { get; set; }
        public Project Project { get; set; }
        public int BugId {get; set; }
        public Bug Bug { get; set; }
    }
}
