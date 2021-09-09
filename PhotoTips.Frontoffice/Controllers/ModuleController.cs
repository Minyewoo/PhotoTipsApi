using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using PhotoTips.Frontoffice.Features.Module;

namespace PhotoTips.Frontoffice.Controllers
{
    [Route("/api/module")]
    public class ModuleController : FrontofficeBaseController
    {
        private readonly IMediator _mediator;

        public ModuleController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpGet("listAllIn")]
        public async Task<IActionResult> GetListAllIn([FromQuery] GetModuleListAllInQuery query)
        {
            return await _mediator.Send(query, this.HttpContext.RequestAborted);
        }
        
        [HttpGet("list")]
        public async Task<IActionResult> GetList([FromQuery] GetModuleListQuery query)
        {
            return await _mediator.Send(query, this.HttpContext.RequestAborted);
        }
        
        [HttpGet]
        public async Task<IActionResult> Get([FromQuery] GetModuleQuery query)
        {
            return await _mediator.Send(query, this.HttpContext.RequestAborted);
        }
    }
}