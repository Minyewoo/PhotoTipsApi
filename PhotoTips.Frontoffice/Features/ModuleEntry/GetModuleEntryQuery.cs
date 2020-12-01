using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using PhotoTips.Core.Repositories;
using PhotoTips.Api.DTOs;

namespace PhotoTips.Frontoffice.Features.ModuleEntry
{
    public class GetModuleEntryQuery : IRequest<IActionResult>
    {
        public int ModuleEntryId { get; set; }
    }
    
    public class GetModuleEntryQueryHandler : IRequestHandler<GetModuleEntryQuery, IActionResult>
    {
        private readonly IModuleEntryRepository _moduleEntryRepository;

        public GetModuleEntryQueryHandler(IModuleEntryRepository moduleEntryRepository)
        {
            _moduleEntryRepository = moduleEntryRepository;
        }

        public async Task<IActionResult> Handle(GetModuleEntryQuery request, CancellationToken cancellationToken)
        {
            var moduleEntry = await _moduleEntryRepository.Get(request.ModuleEntryId, cancellationToken);
            
            if (moduleEntry == null) return new NotFoundObjectResult("Module Entry not found");

            return new OkObjectResult(moduleEntry.ToDto());
        }
    }
}