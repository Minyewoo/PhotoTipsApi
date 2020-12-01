using System.Threading;
using System.Threading.Tasks;
using MediatR;
using PhotoTips.Core.Repositories;

namespace PhotoTips.Backoffice.Features.City
{
    public class DeleteCityCommand : IRequest
    {
        public long CityId { get; set; } 
    }
    
    public class DeleteCityCommandHandler : AsyncRequestHandler<DeleteCityCommand>
    {
        private readonly ICityRepository _cityRepository;

        public DeleteCityCommandHandler(ICityRepository cityRepository)
        {
            _cityRepository = cityRepository;
        }

        protected override async Task Handle(DeleteCityCommand request, CancellationToken cancellationToken)
        {
            await _cityRepository.Remove(request.CityId, cancellationToken);
        }
    }
}