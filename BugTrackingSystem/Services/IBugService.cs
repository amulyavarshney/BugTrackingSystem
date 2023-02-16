using BugTrackingSystem.Models;
using BugTrackingSystem.ViewModels;
namespace BugTrackingSystem.Services
{
    public interface IBugService
    {
        Task<IEnumerable<BugViewModel>> GetAllAsync();
        Task<BugViewModel> GetByIdAsync(int bugId);
        Task<BugViewModel> CreateAsync(int projectId, BugCreateViewModel bug);
        // Task<MessageViewModel> CreateByIdAsync(int bugId, MessageCreateViewModel message);
        Task<BugViewModel> UpdateAsync(int bugId, BugUpdateViewModel bug);
    }
}
