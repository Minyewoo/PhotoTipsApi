using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using PhotoTips.Core.Repositories;
using PhotoTips.Api.DTOs;

namespace PhotoTips.Frontoffice.Features.City
{
    public class GetCityQuery : IRequest<IActionResult>
    {
        public int CityId { get; set; }
    }

    public class GetCityQueryHandler : IRequestHandler<GetCityQuery, IActionResult>
    {
        private readonly ICityRepository _cityRepository;

        public GetCityQueryHandler(ICityRepository cityRepository)
        {
            _cityRepository = cityRepository;
        }

        public async Task<IActionResult> Handle(GetCityQuery request, CancellationToken cancellationToken)
        {
            var city = await _cityRepository.Get(request.CityId, cancellationToken);

            if (city == null) return new NotFoundObjectResult("City not found");

            return new OkObjectResult(city.ToDto());
        }
    }
}