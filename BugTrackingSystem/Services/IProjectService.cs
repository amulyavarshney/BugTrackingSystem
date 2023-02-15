using BugTrackingSystem.ViewModels;
namespace BugTrackingSystem.Services
{
    public interface IProjectService
    {
        Task<IEnumerable<ProjectViewModel>> GetAllAsync();
        Task<ProjectViewModel> GetByIdAsync(int projectId);
        Task<ProjectViewModel> CreateAsync(ProjectCreateViewModel Project);
        Task<BugViewModel> CreateByIdAsync(int projectId, BugCreateViewModel bug);
        Task DeleteAsync(int projectId);
    }
}
