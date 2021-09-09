using System.Threading;
using System.Threading.Tasks;
using MediatR;
using PhotoTips.Core.Repositories;

namespace PhotoTips.Backoffice.Features.City
{
    public class CreateCityCommand : IRequest
    {
        public string Name { get; set; }
    }
    
    public class CreateCityCommandHandler : AsyncRequestHandler<CreateCityCommand>
    {
        private readonly ICityRepository _cityRepository;

        public CreateCityCommandHandler(ICityRepository cityRepository)
        {
            _cityRepository = cityRepository;
        }

        protected override async Task Handle(CreateCityCommand request, CancellationToken cancellationToken)
        {
            var city = new Core.Models.City
            {
                Name = request.Name
            };
            
            await _cityRepository.Create(city, cancellationToken);
        }
    }
}