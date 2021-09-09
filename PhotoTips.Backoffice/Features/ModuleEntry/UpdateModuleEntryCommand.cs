using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using PhotoTips.Core.Repositories;
using PhotoTips.Api.DTOs;

namespace PhotoTips.Backoffice.Features.ModuleEntry
{
    public class UpdateModuleEntryCommand : IRequest<IActionResult>
    {
        [Required] public long ModuleEntryId { get; set; }
        public int? IndexNumber { get; set; }
        public Core.Models.ModuleEntry.ModuleEntryType? Type { get; set; }
        public string Name { get; set; }

        public string Description { get; set; }

        public string AdditionalInfo { get; set; }

        public LectureContentDto[] VideoLecture { get; set; }

        public LectureContentDto[] TextLecture { get; set; }
    }

    public class UpdateModuleEntryCommandHandler : IRequestHandler<UpdateModuleEntryCommand, IActionResult>
    {
        private readonly IModuleEntryRepository _moduleEntryRepository;

        public UpdateModuleEntryCommandHandler(IModuleEntryRepository moduleEntryRepository)
        {
            _moduleEntryRepository = moduleEntryRepository;
        }

        public async Task<IActionResult> Handle(UpdateModuleEntryCommand request, CancellationToken cancellationToken)
        {
            var moduleEntry = await _moduleEntryRepository.Get(request.ModuleEntryId, cancellationToken);
            if (moduleEntry == null)
                return new NotFoundObjectResult($"Module Entry with id={request.ModuleEntryId} not found");

            moduleEntry.IndexNumber = request.IndexNumber ?? 0;
            moduleEntry.Name = request.Name;
            moduleEntry.Description = request.Description;
            moduleEntry.AdditionalInfo = request.AdditionalInfo;
            moduleEntry.Type = request.Type ?? moduleEntry.Type;

            if (request.TextLecture != null)
                moduleEntry.TextLecture = request.TextLecture.Select(x => x.ToEntity()).ToArray();
            if (request.VideoLecture != null)
                moduleEntry.VideoLecture = request.VideoLecture.Select(x => x.ToEntity()).ToArray();

            await _moduleEntryRepository.Update(moduleEntry, cancellationToken);

            return new OkResult();
        }
    }
}