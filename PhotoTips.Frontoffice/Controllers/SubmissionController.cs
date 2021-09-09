using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using PhotoTips.Frontoffice.Features.Submission;
using PhotoTips.Frontoffice.Features.Submission.PhotoTips.Frontoffice.Features.Submission;

namespace PhotoTips.Frontoffice.Controllers
{
    [Route("/api/submission")]
    public class SubmissionController : FrontofficeBaseController
    {
        private readonly IMediator _mediator;

        public SubmissionController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpGet("listBy")]
        public async Task<IActionResult> GetList([FromQuery] GetSubmissionsByUserTokenQuery query)
        {
            return await _mediator.Send(query, this.HttpContext.RequestAborted);
        }
        
        [HttpGet("listAllBy")]
        public async Task<IActionResult> GetAllList([FromQuery] GetAllSubmissionsByAdminTokenQuery query)
        {
            return await _mediator.Send(query, this.HttpContext.RequestAborted);
        }
    }
}