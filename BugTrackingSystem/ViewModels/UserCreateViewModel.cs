using System.ComponentModel.DataAnnotations;

namespace BugTrackingSystem.ViewModels
{
    public class UserCreateViewModel
    {
        [Required]
        public string UserName { get; set; }
        public string FullName { get; set; }
        public string AvatarUrl { get; set; }
        [Required]
        public string Password { get; set; }
        [Required]
        public IEnumerable<string> Roles { get; set; } = new List<string>();
    }
}
