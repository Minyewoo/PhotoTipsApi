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
        
        [HttpGet("listChecking")]
        public async Task<IActionResult> GetListChecking([FromQuery] GetCheckingSubmissionsByUserTokenQuery query)
        {
            return await _mediator.Send(query, this.HttpContext.RequestAborted);
        }
    }
}