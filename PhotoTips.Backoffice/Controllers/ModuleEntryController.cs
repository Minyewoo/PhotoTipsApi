using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using PhotoTips.Backoffice.Features.ModuleEntry;

namespace PhotoTips.Backoffice.Controllers
{
    [Route("/api/module_entry")]
    public class ModuleEntryController : BackofficeBaseController
    {
        private readonly IMediator _mediator;

        public ModuleEntryController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpPost("create")]
        public async Task<IActionResult> Create([FromBody] CreateModuleEntryCommand command)
        {
            await _mediator.Send(command, HttpContext.RequestAborted);
            return Ok();
        }

        [HttpPut("update")]
        public async Task<IActionResult> Update([FromBody] UpdateModuleEntryCommand command)
        {
            return await _mediator.Send(command, HttpContext.RequestAborted);
        }

        [HttpGet("addToTextLecture")]
        public async Task<IActionResult> AddToTextLecture([FromQuery] AddToTextLectureCommand command)
        {
            await _mediator.Send(command, HttpContext.RequestAborted);
            return Ok();
        }

        [HttpGet("addToVideoLecture")]
        public async Task<IActionResult> AddToVideoLecture([FromQuery] AddToVideoLectureCommand command)
        {
            await _mediator.Send(command, HttpContext.RequestAborted);
            return Ok();
        }

        [HttpDelete("delete")]
        public async Task<IActionResult> Delete([FromQuery] DeleteModuleEntryCommand command)
        {
            await _mediator.Send(command, HttpContext.RequestAborted);
            return Ok();
        }
    }
}