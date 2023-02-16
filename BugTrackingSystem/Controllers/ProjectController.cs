using Microsoft.AspNetCore.Mvc;
using BugTrackingSystem.Services;
using BugTrackingSystem.ViewModels;
using Microsoft.AspNetCore.Authorization;

namespace WebApplication2.Controllers
{
    [Authorize(Policy = "Manager")]
    [Route("api/v1/[controller]")]
    [ApiController]
    public class ProjectController : ControllerBase
    {
        private readonly IProjectService _service;

        public ProjectController(IProjectService service)
        {
            _service = service;
        }

        // get all projects
        [HttpGet]
        public async Task<ActionResult<IEnumerable<ProjectViewModel>>> GetAsync()
        {
            return Ok(await _service.GetAllAsync());
        }

        // get project by id
        [HttpGet("{projectId:int}")]
        public async Task<ActionResult<ProjectViewModel>> GetByIdAsync(int projectId)
        {
            return Ok(await _service.GetByIdAsync(projectId));
        }

        // create a new project
        [HttpPost]
        public async Task<ActionResult<ProjectViewModel>> CreateAsync(ProjectCreateViewModel project)
        {
            return Ok(await _service.CreateAsync(project));
        }

        // delete a project
        [HttpDelete("{projectId:int}")]
        public async Task<IActionResult> DeleteAsync(int projectId)
        {
            await _service.DeleteAsync(projectId);
            return Ok();
        }
    }
}