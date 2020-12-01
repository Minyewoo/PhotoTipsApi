using System.Threading;
using System.Threading.Tasks;
using MediatR;
using PhotoTips.Core.Repositories;

namespace PhotoTips.Backoffice.Features.ModuleEntry
{
    public class DeleteModuleEntryCommand : IRequest
    {
        public long ModuleEntryId { get; set; }
    }

    public class DeleteModuleEntryCommandHandler : AsyncRequestHandler<DeleteModuleEntryCommand>
    {
        private readonly IModuleEntryRepository _moduleEntryRepository;

        public DeleteModuleEntryCommandHandler(IModuleEntryRepository moduleEntryRepository)
        {
            _moduleEntryRepository = moduleEntryRepository;
        }

        protected override async Task Handle(DeleteModuleEntryCommand request, CancellationToken cancellationToken)
        {
            await _moduleEntryRepository.Remove(request.ModuleEntryId, cancellationToken);
        }
    }
}