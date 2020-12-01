using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using PhotoTips.Core.Repositories;
using PhotoTips.Api.DTOs;

namespace PhotoTips.Frontoffice.Features.ModuleEntry
{
    public class GetModuleEntryListAllInQuery : IRequest<IActionResult>
    {
        public int? Skip { get; set; }
        public int? Count { get; set; }
    }

    public class GetModuleEntryListAllInQueryHandler : IRequestHandler<GetModuleEntryListAllInQuery, IActionResult>
    {
        private readonly IModuleEntryRepository _moduleEntryRepository;

        public GetModuleEntryListAllInQueryHandler(IModuleEntryRepository moduleEntryRepository)
        {
            _moduleEntryRepository = moduleEntryRepository;
        }

        public async Task<IActionResult> Handle(GetModuleEntryListAllInQuery request,
            CancellationToken cancellationToken)
        {
            var moduleEntries =
                await _moduleEntryRepository.Get(request.Skip, request.Count, cancellationToken);

            if (moduleEntries == null) return new NotFoundObjectResult("Module Entries not found");

            return new OkObjectResult(moduleEntries.Select(x => x.ToDto()).ToArray());
        }
    }
}