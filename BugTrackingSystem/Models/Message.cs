namespace BugTrackingSystem.Models
{
    public class Message
    {
        public int MessageId { get; set; }
        public bool IsResolved { get; set; }
        public string Text { get; set; }
        public int BugId { get; set; }
        public Bug Bug { get; set; }
    }
}
