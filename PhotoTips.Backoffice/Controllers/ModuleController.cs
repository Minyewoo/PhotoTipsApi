using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using PhotoTips.Backoffice.Features.Module;

namespace PhotoTips.Backoffice.Controllers
{
    [Route("/api/module")]
    public class ModuleController : BackofficeBaseController
    {
        private readonly IMediator _mediator;

        public ModuleController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpPost("create")]
        public async Task<IActionResult> Create([FromBody] CreateModuleCommand command)
        {
            await _mediator.Send(command, HttpContext.RequestAborted);
            return Ok();
        }
        
        [HttpPut("update")]
        public async Task<IActionResult> Update([FromBody] UpdateModuleCommand command)
        {
            return await _mediator.Send(command, HttpContext.RequestAborted);
        }
        
        [HttpGet("addModuleEntry")]
        public async Task<IActionResult> AddModuleEntry([FromQuery] AddModuleEntryCommand command)
        {
            await _mediator.Send(command, HttpContext.RequestAborted);
            return Ok();
        }

        [HttpDelete("delete")]
        public async Task<IActionResult> Delete([FromQuery] DeleteModuleCommand command)
        {
            await _mediator.Send(command, HttpContext.RequestAborted);
            return Ok();
        }
    }
}