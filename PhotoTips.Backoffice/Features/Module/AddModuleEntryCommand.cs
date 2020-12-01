using System.Threading;
using System.Threading.Tasks;
using MediatR;
using PhotoTips.Core.Repositories;

namespace PhotoTips.Backoffice.Features.Module
{
    public class AddModuleEntryCommand : IRequest
    {
        public long ModuleId { get; set; }
        public long ModuleEntryId { get; set; }
    }

    public class AddModuleEntryCommandHandler : AsyncRequestHandler<AddModuleEntryCommand>
    {
        private readonly IModuleRepository _moduleRepository;
        private readonly IModuleEntryRepository _moduleEntryRepository;

        public AddModuleEntryCommandHandler(IModuleRepository moduleRepository,
            IModuleEntryRepository moduleEntryRepository)
        {
            _moduleRepository = moduleRepository;
            _moduleEntryRepository = moduleEntryRepository;
        }

        protected override async Task Handle(AddModuleEntryCommand request, CancellationToken cancellationToken)
        {
            var module = await _moduleRepository.Get(request.ModuleId, cancellationToken);
            var moduleEntry = await _moduleEntryRepository.Get(request.ModuleEntryId, cancellationToken);

            module.Entries.Add(moduleEntry);
            await _moduleRepository.Update(module, cancellationToken);
        }
    }
}