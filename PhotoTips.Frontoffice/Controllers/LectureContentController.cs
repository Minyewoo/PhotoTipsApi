using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using PhotoTips.Frontoffice.Features.LectureContent;

namespace PhotoTips.Frontoffice.Controllers
{
    [Route("/api/lecture_content")]
    public class LectureContentController : FrontofficeBaseController
    {
        private readonly IMediator _mediator;

        public LectureContentController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpGet("list")]
        public async Task<IActionResult> GetList([FromQuery] GetLectureContentListQuery query)
        {
            return await _mediator.Send(query, this.HttpContext.RequestAborted);
        }
        
        [HttpGet]
        public async Task<IActionResult> Get([FromQuery] GetLectureContentQuery query)
        {
            return await _mediator.Send(query, this.HttpContext.RequestAborted);
        }
    }
}