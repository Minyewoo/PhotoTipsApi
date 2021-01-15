using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using PhotoTips.Backoffice.Features.Submission;

namespace PhotoTips.Backoffice.Controllers
{
    [Route("/api/submission")]
    public class SubmissionController : BackofficeBaseController
    {
        private readonly IMediator _mediator;

        public SubmissionController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpPost("create")]
        public async Task<IActionResult> Create([FromForm] CreateSubmissionCommand command)
        {
            return await _mediator.Send(command, HttpContext.RequestAborted);
        }

        [HttpPut("updateStatus")]
        public async Task<IActionResult> UpdateStatus([FromBody] UpdateSubmissionStatusCommand command)
        {
            return await _mediator.Send(command, HttpContext.RequestAborted);
        }

        [HttpDelete("delete")]
        public async Task<IActionResult> Delete([FromQuery] DeleteSubmissionCommand command)
        {
            await _mediator.Send(command, HttpContext.RequestAborted);
            return Ok();
        }
    }
}