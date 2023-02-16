using BugTrackingSystem.Services;
using BugTrackingSystem.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BugTrackingSystem.Controllers
{
    [Authorize(Policy = "User")]
    [Route("api/v1/[controller]")]
    [ApiController]
    public class MessageController : ControllerBase
    {
        private readonly IMessageService _service;
        public MessageController(IMessageService service)
        {
            _service = service;
        }

        // get all messages
        [HttpGet]
        public async Task<ActionResult<IEnumerable<MessageViewModel>>> GetAsync()
        {
            return Ok(await _service.GetAllAsync());
        }

        // get message by id
        [HttpGet("{id:int}")]
        public async Task<ActionResult<MessageViewModel>> GetByIdAsync(int id)
        {
            return Ok(await _service.GetByIdAsync(id));
        }

        // create a new message
        [HttpPost()]
        public async Task<ActionResult<MessageViewModel>> CreateAsync(int bugId, MessageCreateViewModel message)
        {
            return Ok(await _service.CreateAsync(bugId, message));
        }
    }
}
