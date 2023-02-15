using BugTrackingSystem.Models;

namespace BugTrackingSystem.ViewModels
{
    public class ProjectViewModel
    {
        public int ProjectId { get; set; }
        public string Title { get; set; }
        public List<BugViewModel> Bugs { get; set; }
    }
}
