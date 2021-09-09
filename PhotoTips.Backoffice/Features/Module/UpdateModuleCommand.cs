using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using PhotoTips.Core.Repositories;
using PhotoTips.Api.DTOs;

namespace PhotoTips.Backoffice.Features.Module
{
    public class UpdateModuleCommand : IRequest<IActionResult>
    {
        [Required] public long ModuleId { get; set; }
        public int? IndexNumber { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }

        public ModuleEntryListDto.ModuleEntryListItemDto[] Entries { get; set; }
    }

    public class UpdateModuleCommandHandler : IRequestHandler<UpdateModuleCommand, IActionResult>
    {
        private readonly IModuleRepository _moduleRepository;

        public UpdateModuleCommandHandler(IModuleRepository moduleRepository)
        {
            _moduleRepository = moduleRepository;
        }

        public async Task<IActionResult> Handle(UpdateModuleCommand request, CancellationToken cancellationToken)
        {
            var module = await _moduleRepository.Get(request.ModuleId, cancellationToken);
            if (module == null) return new NotFoundObjectResult($"Module with id={request.ModuleId} not found");

            module.IndexNumber = request.IndexNumber ?? 0;
            module.Name = request.Name;
            module.Description = request.Description;

            if (request.Entries != null) module.Entries = request.Entries.Select(x => x.ToEntity()).ToArray();

            await _moduleRepository.Update(module, cancellationToken);

            return new OkResult();
        }
    }
}