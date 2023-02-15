using BugTrackingSystem.Models;
using BugTrackingSystem.Services;
using Microsoft.AspNetCore.Mvc;

namespace BugTrackingSystem.Controllers
{
    [Route("api/v1/[controller]")]
    [ApiController]
    public class AdminController : ControllerBase
    {
        private readonly IAdminService _service;
        
        public AdminController(IAdminService service)
        {
            _service = service;
        }

        // get Total number of open bugs for all projects
        [HttpGet("open")]
        public async Task<int> GetOpenBugsCountAsync()
        {
            return await _service.GetBugCountAsync(BugState.OPEN);
        }

        // get Total number of resolved bugs for all projects
        [HttpGet("resolved")]
        public async Task<int> GetResolvedBugsCountAsync()
        {
            return await _service.GetBugCountAsync(BugState.RESOLVED);
        }

        // get Total number of messages for all projects
        [HttpGet("messagecount")]
        public async Task<int> GetAllMessagesCountAsync()
        {
            return await _service.GetMessageCountAsync();
        }

        // get Bug resolution rate (=resolved/total bugs)
        [HttpGet("resolutionrate")]
        public async Task<double> GetBugResolutionRateAsync()
        {
            return await _service.GetBugResolutionRateAsync();
        }
    }
}
