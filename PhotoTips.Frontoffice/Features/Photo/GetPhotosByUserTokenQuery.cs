using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using PhotoTips.Core.Repositories;
using PhotoTips.Core.Utils;
using PhotoTips.Api.DTOs;

namespace PhotoTips.Frontoffice.Features.Photo
{
    public class GetPhotosByUserTokenQuery : IRequest<IActionResult>
    {
        public string UserToken { get; set; }
    }

    public class GetPhotosByUserTokenQueryHandler : IRequestHandler<GetPhotosByUserTokenQuery, IActionResult>
    {
        private readonly IUserRepository _userRepository;

        public GetPhotosByUserTokenQueryHandler(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        public async Task<IActionResult> Handle(GetPhotosByUserTokenQuery request, CancellationToken cancellationToken)
        {
            var user = await new JwtManager().FindUserByToken(request.UserToken, _userRepository, cancellationToken);

            if (user == null) return new NotFoundObjectResult("User not found");

            return new OkObjectResult(user.Photos?.Select(x => x.ToDto()).ToArray());
        }
    }
}