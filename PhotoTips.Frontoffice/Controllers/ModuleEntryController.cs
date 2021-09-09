using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using PhotoTips.Frontoffice.Features.ModuleEntry;

namespace PhotoTips.Frontoffice.Controllers
{
    [Route("/api/module_entry")]
    public class ModuleEntryController : FrontofficeBaseController
    {
        private readonly IMediator _mediator;

        public ModuleEntryController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpGet("listAllIn")]
        public async Task<IActionResult> GetListAllIn([FromQuery] GetModuleEntryListAllInQuery query)
        {
            return await _mediator.Send(query, this.HttpContext.RequestAborted);
        }
        
        [HttpGet("list")]
        public async Task<IActionResult> GetList([FromQuery] GetModuleEntryListQuery query)
        {
            return await _mediator.Send(query, this.HttpContext.RequestAborted);
        }
        
        [HttpGet]
        public async Task<IActionResult> Get([FromQuery] GetModuleEntryQuery query)
        {
            return await _mediator.Send(query, this.HttpContext.RequestAborted);
        }
    }
}