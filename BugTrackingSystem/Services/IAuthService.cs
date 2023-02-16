using BugTrackingSystem.ViewModels;

namespace BugTrackingSystem.Services
{
    public interface IAuthService
    {
        Task Register(UserCreateViewModel user);
        Task<JwtViewModel> Login(LoginViewModel loginView);
    }
}
