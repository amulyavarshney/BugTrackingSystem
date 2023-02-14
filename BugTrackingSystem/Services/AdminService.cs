using BugTrackingSystem.DAL;
using BugTrackingSystem.Models;

namespace BugTrackingSystem.Services
{
    public class AdminService : IAdminService
    {
        private readonly BugTrackingContext _context;
        public AdminService(BugTrackingContext context)
        {
            _context = context;
        }

        public async Task<int> GetBugCountAsync(BugState state)
        {
            return await _context.
        }
        public async Task<int> GetMessageCountAsync()
        {

        }
        public async Task<int> GetBugResolutionRateAsync()
        {

        }
    }
}
