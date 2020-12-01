using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using PhotoTips.Frontoffice.Features.User;

namespace PhotoTips.Frontoffice.Controllers
{
    [Route("/api/user")]
    public class UserController : FrontofficeBaseController
    {
        private readonly IMediator _mediator;

        public UserController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpGet]
        public async Task<IActionResult> GetUser([FromQuery] GetUserQuery query)
        {
            return await _mediator.Send(query, this.HttpContext.RequestAborted);
        }

        [HttpGet("token")]
        public async Task<IActionResult> GetToken([FromQuery] GetTokenQuery query)
        {
            return await _mediator.Send(query, this.HttpContext.RequestAborted);
        }
    }
}