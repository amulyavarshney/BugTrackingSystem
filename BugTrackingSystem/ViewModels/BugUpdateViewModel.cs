using BugTrackingSystem.Models;

namespace BugTrackingSystem.ViewModels
{
    public class BugUpdateViewModel
    {
        public int BugId { get; set; }
        public BugState Title { get; set; }
    }
}
