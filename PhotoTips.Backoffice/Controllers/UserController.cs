using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using PhotoTips.Backoffice.Features.User;

namespace PhotoTips.Backoffice.Controllers
{
    [Route("/api/user")]
    public class UserController : BackofficeBaseController
    {
        private readonly IMediator _mediator;

        public UserController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpPost("create")]
        public async Task<IActionResult> Create([FromBody] CreateUserCommand command)
        {
            var result = await _mediator.Send(command, HttpContext.RequestAborted);
            return string.IsNullOrEmpty(result) ? (IActionResult) Ok() : BadRequest(result);
        }
        
        [HttpPut("updateInfo")]
        public async Task<IActionResult> UpdateInfo([FromBody] UpdateUserInfoQuery command)
        {
            return await _mediator.Send(command, HttpContext.RequestAborted);
        }
        
        [HttpPut("updatePassword")]
        public async Task<IActionResult> UpdatePassword([FromBody] UpdatePasswordQuery query)
        {
            return await _mediator.Send(query, HttpContext.RequestAborted);
        }

        [HttpDelete("delete")]
        public async Task<IActionResult> Delete([FromQuery] DeleteUserCommand command)
        {
            await _mediator.Send(command, HttpContext.RequestAborted);
            return Ok();
        }
    }
}