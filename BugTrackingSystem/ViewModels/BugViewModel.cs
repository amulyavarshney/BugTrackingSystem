using BugTrackingSystem.Models;

namespace BugTrackingSystem.ViewModels
{
    public class BugViewModel
    {
        public int BugId { get; set; }
        public string Title { get; set; }
        public BugState State { get; set; }
        public List<MessageViewModel> Messages { get; set; }
    }
}
