using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using PhotoTips.Core.Repositories;
using PhotoTips.Api.DTOs;

namespace PhotoTips.Frontoffice.Features.Module
{
    public class GetModuleQuery : IRequest<IActionResult>
    {
        public int ModuleId { get; set; }
    }
    
    public class GetModuleQueryHandler : IRequestHandler<GetModuleQuery, IActionResult>
    {
        private readonly IModuleRepository _moduleRepository;

        public GetModuleQueryHandler(IModuleRepository moduleRepository)
        {
            _moduleRepository = moduleRepository;
        }

        public async Task<IActionResult> Handle(GetModuleQuery request, CancellationToken cancellationToken)
        {
            var module = await _moduleRepository.Get(request.ModuleId, cancellationToken);
            
            if (module == null) return new NotFoundObjectResult("Module not found");

            return new OkObjectResult(module.ToDto());
        }
    }
}