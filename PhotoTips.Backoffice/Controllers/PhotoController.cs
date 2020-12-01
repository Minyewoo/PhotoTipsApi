using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using PhotoTips.Backoffice.Features.Photo;
using PhotoTips.Backoffice.Features.User;

namespace PhotoTips.Backoffice.Controllers
{
    [Route("/api/photo")]
    public class PhotoController : BackofficeBaseController
    {
        private readonly IMediator _mediator;

        public PhotoController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpPost("create")]
        public async Task<IActionResult> Create([FromForm] CreatePhotoCommand command)
        {
            return await _mediator.Send(command, HttpContext.RequestAborted);
        }

        [HttpPut("update")]
        public async Task<IActionResult> Update([FromBody] UpdatePhotoUrlsCommand command)
        {
            return await _mediator.Send(command, HttpContext.RequestAborted);
        }

        [HttpDelete("delete")]
        public async Task<IActionResult> Delete([FromQuery] DeletePhotoCommand command)
        {
            await _mediator.Send(command, HttpContext.RequestAborted);
            return Ok();
        }
    }
}