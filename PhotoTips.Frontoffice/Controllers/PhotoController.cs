using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using PhotoTips.Frontoffice.Features.Photo;

namespace PhotoTips.Frontoffice.Controllers
{
    [Route("/api/photo")]
    public class PhotoController : FrontofficeBaseController
    {
        private readonly IMediator _mediator;

        public PhotoController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpGet("listBy")]
        public async Task<IActionResult> GetList([FromQuery] GetPhotosByUserTokenQuery query)
        {
            return await _mediator.Send(query, this.HttpContext.RequestAborted);
        }
    }
}