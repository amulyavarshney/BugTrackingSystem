using BugTrackingSystem.Models;
namespace BugTrackingSystem.Services
{
    public interface IAdminService
    {
        Task<int> GetBugCountAsync(BugState state);
        Task<int> GetMessageCountAsync();
        Task<double> GetBugResolutionRateAsync();
    }
}
