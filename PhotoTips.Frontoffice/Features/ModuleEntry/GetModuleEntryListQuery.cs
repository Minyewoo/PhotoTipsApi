using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using PhotoTips.Core.Repositories;
using PhotoTips.Api.DTOs;

namespace PhotoTips.Frontoffice.Features.ModuleEntry
{
    public class GetModuleEntryListQuery : IRequest<IActionResult>
    {
        public int? Skip { get; set; }
        public int? Count { get; set; }
    }

    public class GetModuleEntryListQueryHandler : IRequestHandler<GetModuleEntryListQuery, IActionResult>
    {
        private readonly IModuleEntryRepository _moduleEntryRepository;

        public GetModuleEntryListQueryHandler(IModuleEntryRepository moduleEntryRepository)
        {
            _moduleEntryRepository = moduleEntryRepository;
        }

        public async Task<IActionResult> Handle(GetModuleEntryListQuery request, CancellationToken cancellationToken)
        {
            var moduleEntries =
                await _moduleEntryRepository.Get(request.Skip, request.Count, cancellationToken);

            if (moduleEntries == null) return new NotFoundObjectResult("Module Entries not found");

            return new OkObjectResult(moduleEntries.Select(x => x.ToListItemDto()).ToArray());
        }
    }
}