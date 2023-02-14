using BugTrackingSystem.Models;
using BugTrackingSystem.Services;
using Microsoft.AspNetCore.Mvc;

namespace BugTrackingSystem.Controllers
{
    public class AdminController : ControllerBase
    {
        private readonly IAdminService _service;
        
        public AdminController(IAdminService service)
        {
            _service = service;
        }

        // get Total number of open bugs for all projects
        [HttpGet]
        public async Task<int> GetOpenBugsCountAsync()
        {
            return await _service.GetBugCountAsync(BugState.OPEN);
        }

        // get Total number of resolved bugs for all projects
        [HttpGet]
        public async Task<int> GetResolvedBugsCountAsync()
        {
            return await _service.GetBugCountAsync(BugState.RESOLVED);
        }

        // get Total number of messages for all projects
        [HttpGet]
        public async Task<int> GetAllMessagesCountAsync()
        {
            return await _service.GetMessageCountAsync();
        }

        // get Bug resolution rate (=resolved/total bugs)
        [HttpGet]
        public async Task<int> GetBugResolutionRateAsync()
        {
            return await _service.GetBugResolutionRateAsync();
        }
    }
}
