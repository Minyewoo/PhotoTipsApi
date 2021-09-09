using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using PhotoTips.Backoffice.Features.City;

namespace PhotoTips.Backoffice.Controllers
{
    [Route("/api/city")]
    public class CityController : BackofficeBaseController
    {
        private readonly IMediator _mediator;

        public CityController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpPost("create")]
        public async Task<IActionResult> Create([FromBody] CreateCityCommand command)
        {
            await _mediator.Send(command, HttpContext.RequestAborted);
            return Ok();
        }
        
        [HttpPut("update")]
        public async Task<IActionResult> Update([FromBody] UpdateCityCommand command)
        {
            return await _mediator.Send(command, HttpContext.RequestAborted);
        }

        [HttpDelete("delete")]
        public async Task<IActionResult> Delete([FromQuery] DeleteCityCommand command)
        {
            await _mediator.Send(command, HttpContext.RequestAborted);
            return Ok();
        }
    }
}