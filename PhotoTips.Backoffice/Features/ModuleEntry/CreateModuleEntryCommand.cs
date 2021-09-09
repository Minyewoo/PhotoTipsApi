using System.Threading;
using System.Threading.Tasks;
using MediatR;
using PhotoTips.Core.Repositories;

namespace PhotoTips.Backoffice.Features.ModuleEntry
{
    public class CreateModuleEntryCommand : IRequest
    {
        public int IndexNumber { get; set; }
        public Core.Models.ModuleEntry.ModuleEntryType Type { get; set; }
        public string Name { get; set; }
        
        public string Description { get; set; }
        
        public string AdditionalInfo { get; set; }
    }
    
    public class CreateModuleEntryCommandHandler : AsyncRequestHandler<CreateModuleEntryCommand>
    {
        private readonly IModuleEntryRepository _moduleEntryRepository;

        public CreateModuleEntryCommandHandler(IModuleEntryRepository moduleEntryRepository)
        {
            _moduleEntryRepository = moduleEntryRepository;
        }

        protected override async Task Handle(CreateModuleEntryCommand request, CancellationToken cancellationToken)
        {
            var moduleEntry = new Core.Models.ModuleEntry
            {
                IndexNumber = request.IndexNumber,
                Name = request.Name,
                Type = request.Type,
                Description = request.Description,
                AdditionalInfo = request.AdditionalInfo
            };
            
            await _moduleEntryRepository.Create(moduleEntry, cancellationToken);
        }
    }
}