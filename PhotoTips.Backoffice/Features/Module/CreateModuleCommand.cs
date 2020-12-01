using System.Threading;
using System.Threading.Tasks;
using MediatR;
using PhotoTips.Core.Repositories;

namespace PhotoTips.Backoffice.Features.Module
{
    public class CreateModuleCommand : IRequest
    {
        public int IndexNumber { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
    }
    
    public class CreateModuleCommandHandler : AsyncRequestHandler<CreateModuleCommand>
    {
        private readonly IModuleRepository _moduleRepository;

        public CreateModuleCommandHandler(IModuleRepository moduleRepository)
        {
            _moduleRepository = moduleRepository;
        }

        protected override async Task Handle(CreateModuleCommand request, CancellationToken cancellationToken)
        {
            var module = new Core.Models.Module
            {
                IndexNumber = request.IndexNumber,
                Name = request.Name,
                Description = request.Description,
            };
            
            await _moduleRepository.Create(module, cancellationToken);
        }
    }
}