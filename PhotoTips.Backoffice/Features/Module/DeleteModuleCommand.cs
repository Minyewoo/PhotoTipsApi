using System.Threading;
using System.Threading.Tasks;
using MediatR;
using PhotoTips.Core.Repositories;

namespace PhotoTips.Backoffice.Features.Module
{
    public class DeleteModuleCommand : IRequest
    {
        public long ModuleId { get; set; }
    }

    public class DeleteModuleCommandHandler : AsyncRequestHandler<DeleteModuleCommand>
    {
        private readonly IModuleRepository _moduleRepository;

        public DeleteModuleCommandHandler(IModuleRepository moduleRepository)
        {
            _moduleRepository = moduleRepository;
        }

        protected override async Task Handle(DeleteModuleCommand request, CancellationToken cancellationToken)
        {
            await _moduleRepository.Remove(request.ModuleId, cancellationToken);
        }
    }
}