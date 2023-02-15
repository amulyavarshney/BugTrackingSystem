using BugTrackingSystem.ViewModels;

namespace BugTrackingSystem.Services
{
    public interface IMessageService
    {
        Task<IEnumerable<MessageViewModel>> GetAllAsync();
        Task<MessageViewModel> GetByIdAsync(int bugId);
    }
}
