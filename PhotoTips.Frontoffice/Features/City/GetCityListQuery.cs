using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using PhotoTips.Core.Repositories;
using PhotoTips.Api.DTOs;

namespace PhotoTips.Frontoffice.Features.City
{
    public class GetCityListQuery : IRequest<IActionResult>
    {
        public int? Skip { get; set; }
        public int? Count { get; set; }
    }

    public class GetCityListQueryHandler : IRequestHandler<GetCityListQuery, IActionResult>
    {
        private readonly ICityRepository _cityRepository;

        public GetCityListQueryHandler(ICityRepository cityRepository)
        {
            _cityRepository = cityRepository;
        }

        public async Task<IActionResult> Handle(GetCityListQuery request, CancellationToken cancellationToken)
        {
            var cities =
                await _cityRepository.Get(request.Skip, request.Count, cancellationToken);

            if (cities == null) return new NotFoundObjectResult("Cities not found");

            return new OkObjectResult(cities.Select(x => x.ToDto()));
        }
    }
}