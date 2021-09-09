using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using PhotoTips.Frontoffice.Features.City;

namespace PhotoTips.Frontoffice.Controllers
{
    [Route("/api/city")]
    public class CityController : FrontofficeBaseController
    {
        private readonly IMediator _mediator;

        public CityController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpGet("list")]
        public async Task<IActionResult> GetList([FromQuery] GetCityListQuery query)
        {
            return await _mediator.Send(query, this.HttpContext.RequestAborted);
        }
        
        [HttpGet]
        public async Task<IActionResult> Get([FromQuery] GetCityQuery query)
        {
            return await _mediator.Send(query, this.HttpContext.RequestAborted);
        }
    }
}