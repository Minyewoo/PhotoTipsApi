using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using PhotoTips.Backoffice.Features.LectureContent;

namespace PhotoTips.Backoffice.Controllers
{
    [Route("/api/lecture_content")]
    public class LectureContentController : BackofficeBaseController
    {
        private readonly IMediator _mediator;

        public LectureContentController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpPost("create")]
        public async Task<IActionResult> Create([FromBody] CreateLectureContentCommand command)
        {
            await _mediator.Send(command, HttpContext.RequestAborted);
            return Ok();
        }
        
        [HttpPut("update")]
        public async Task<IActionResult> Update([FromBody] UpdateLectureContentCommand command)
        {
            return await _mediator.Send(command, HttpContext.RequestAborted);
        }
        
        [HttpPost("uploadText")]
        public async Task<IActionResult> UploadText([FromBody] UploadTextCommand command)
        {
            await _mediator.Send(command, HttpContext.RequestAborted);
            return Ok();
        }
        
        [HttpPost("uploadImage")]
        public async Task<IActionResult> UploadImage([FromForm] UploadImageCommand command)
        {
            await _mediator.Send(command, HttpContext.RequestAborted);
            return Ok();
        }
        
        [HttpPost("uploadVideo")]
        public async Task<IActionResult> UploadVideo([FromForm] UploadVideoCommand command)
        {
            await _mediator.Send(command, HttpContext.RequestAborted);
            return Ok();
        }

        [HttpDelete("delete")]
        public async Task<IActionResult> Delete([FromQuery] DeleteLectureContentCommand command)
        {
            await _mediator.Send(command, HttpContext.RequestAborted);
            return Ok();
        }
    }
}