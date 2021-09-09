using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using PhotoTips.Core.Repositories;
using PhotoTips.Api.DTOs;

namespace PhotoTips.Frontoffice.Features.Module
{
    public class GetModuleListAllInQuery : IRequest<IActionResult>
    {
        public int? Skip { get; set; }
        public int? Count { get; set; }
    }
    
    public class GetModuleListAllInQueryHandler : IRequestHandler<GetModuleListAllInQuery, IActionResult>
    {
        private readonly IModuleRepository _moduleRepository;

        public GetModuleListAllInQueryHandler(IModuleRepository moduleRepository)
        {
            _moduleRepository = moduleRepository;
        }

        public async Task<IActionResult> Handle(GetModuleListAllInQuery request, CancellationToken cancellationToken)
        {
            var modules = await _moduleRepository.Get(request.Skip, request.Count,cancellationToken);
            
            if (modules == null) return new NotFoundObjectResult("Modules not found");

            return new OkObjectResult(modules.Select(x=>x.ToDto()).ToArray());
        }
    }
}