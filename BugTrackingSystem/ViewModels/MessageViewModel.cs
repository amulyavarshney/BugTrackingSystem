using BugTrackingSystem.Models;

namespace BugTrackingSystem.ViewModels
{
    public class MessageViewModel
    {
        public int MessageId { get; set; }
        public bool IsResolved { get; set; }
        public string Text { get; set; }
    }
}
