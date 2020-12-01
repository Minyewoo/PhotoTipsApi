using System.ComponentModel.DataAnnotations;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using PhotoTips.Core.Repositories;

namespace PhotoTips.Backoffice.Features.City
{
    public class UpdateCityCommand : IRequest<IActionResult>
    {
        [Required] public long CitiId { get; set; }
        public string Name { get; set; }
    }

    public class UpdateCityCommandHandler : IRequestHandler<UpdateCityCommand, IActionResult>
    {
        private readonly ICityRepository _cityRepository;

        public UpdateCityCommandHandler(ICityRepository cityRepository)
        {
            _cityRepository = cityRepository;
        }

        public async Task<IActionResult> Handle(UpdateCityCommand request, CancellationToken cancellationToken)
        {
            var city = await _cityRepository.Get(request.CitiId, cancellationToken);
            if (city == null) return new NotFoundObjectResult($"City with id={request.CitiId} not found");

            city.Name = request.Name;
            await _cityRepository.Update(city, cancellationToken);

            return new OkResult();
        }
    }
}