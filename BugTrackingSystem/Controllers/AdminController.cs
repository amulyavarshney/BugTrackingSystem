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
        [HttpGet("open-bugs")]
        public async Task<ActionResult<object>> GetOpenBugsCountAsync()
        {
            return Ok(new { NumberOfOpenBugs = await _service.GetBugCountAsync(BugState.OPEN) });
        }

        // get Total number of resolved bugs for all projects
        [HttpGet("resolved-bugs")]
        public async Task<ActionResult<object>> GetResolvedBugsCountAsync()
        {
            return Ok(new { NumberOfResolvedBugs = await _service.GetBugCountAsync(BugState.RESOLVED) });
        }

        // get Total number of messages for all projects
        [HttpGet("total-message-count")]
        public async Task<ActionResult<object>> GetAllMessagesCountAsync()
        {
            return Ok(new { TotalMessageCount = await _service.GetMessageCountAsync() });
        }

        // get Bug resolution rate (=resolved/total bugs)
        [HttpGet("resolution-rate")]
        public async Task<ActionResult<object>> GetBugResolutionRateAsync()
        {
            return Ok(new { ResolutionRate = await _service.GetBugResolutionRateAsync() });
        }
    }
}
