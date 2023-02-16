using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using BugTrackingSystem.Services;
using BugTrackingSystem.ViewModels;
using Microsoft.AspNetCore.Authorization;

namespace BugTrackingSystem.Controllers
{
    [Authorize(Policy = "User")]
    [Route("api/v1/[controller]")]
    [ApiController]
    public class BugController : ControllerBase
    {
        private readonly IBugService _service;
        public BugController(IBugService service)
        {
            _service = service;
        }

        // get all bugs
        [HttpGet]
        public async Task<ActionResult<IEnumerable<BugViewModel>>> GetAsync()
        {
            return Ok(await _service.GetAllAsync());
        }

        // get bug by id
        [HttpGet("{bugId:int}")]
        public async Task<ActionResult<BugViewModel>> GetByIdAsync(int bugId)
        {
            return Ok(await _service.GetByIdAsync(bugId));
        }

        // create a new bug
        [HttpPost]
        public async Task<ActionResult<BugViewModel>> CreateAsync(int projectId, BugCreateViewModel bug)
        {
            return Ok(await _service.CreateAsync(projectId, bug));
        }

        // update status of bug
        [HttpPut("{bugId:int}")]
        public async Task<ActionResult<BugViewModel>> UpdateByIdAsync(int bugId, BugUpdateViewModel bug)
        {
            return Ok(await _service.UpdateAsync(bugId, bug));
        }
    }
}