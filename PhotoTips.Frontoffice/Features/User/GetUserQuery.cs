using System;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using PhotoTips.Core.Repositories;
using PhotoTips.Core.Utils;
using PhotoTips.Api.DTOs;

namespace PhotoTips.Frontoffice.Features.User
{
    public class GetUserQuery : IRequest<IActionResult>
    {
        public string Token { get; set; }
    }

    public class GetUserQueryException : Exception
    {
        public GetUserQueryException(string message) : base(message)
        {
        }
    }

    public class GetUserQueryHandler : IRequestHandler<GetUserQuery, IActionResult>
    {
        private readonly IUserRepository _userRepository;

        public GetUserQueryHandler(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }
        
        public async Task<IActionResult> Handle(GetUserQuery request, CancellationToken cancellationToken)
        {
            var user = await new JwtManager().FindUserByToken(request.Token, _userRepository, cancellationToken);

            if (user == null) return new NotFoundObjectResult("User not found");
            return new OkObjectResult(user.ToDto());
        }
    }
}