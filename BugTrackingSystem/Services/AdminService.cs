using BugTrackingSystem.DAL;
using BugTrackingSystem.Models;
using Microsoft.EntityFrameworkCore;

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
            return await _context.Bugs.CountAsync(bug => bug.State == state);
        }
        public async Task<int> GetMessageCountAsync()
        {
            return await _context.Messages.CountAsync();
        }
        public async Task<double> GetBugResolutionRateAsync()
        {
            int totalBugs = _context.Bugs.Count();
            int resolvedBugs = await GetBugCountAsync(BugState.RESOLVED);
            return totalBugs > 0 ? (double)resolvedBugs / (double)totalBugs : 0;
        }
    }
}
