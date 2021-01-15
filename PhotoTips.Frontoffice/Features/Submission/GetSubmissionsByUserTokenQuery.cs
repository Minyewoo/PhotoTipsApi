using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using PhotoTips.Core.Repositories;
using PhotoTips.Core.Utils;


namespace PhotoTips.Frontoffice.Features.Submission
{
    public class GetSubmissionsByUserTokenQuery : IRequest<IActionResult>
    {
        public string UserToken { get; set; }
    }

    public class GetSubmissionsByUserTokenQueryHandler : IRequestHandler<GetSubmissionsByUserTokenQuery, IActionResult>
    {
        private readonly IUserRepository _userRepository;
        private readonly ISubmissionRepository _submissionRepository;

        public GetSubmissionsByUserTokenQueryHandler(IUserRepository userRepository,
            ISubmissionRepository submissionRepository)
        {
            _userRepository = userRepository;
            _submissionRepository = submissionRepository;
        }

        public async Task<IActionResult> Handle(GetSubmissionsByUserTokenQuery request,
            CancellationToken cancellationToken)
        {
            var user = await new JwtManager().FindUserByToken(request.UserToken, _userRepository, cancellationToken);

            if (user == null) return new NotFoundObjectResult("User not found");

            return new OkObjectResult(_submissionRepository.Get(user, cancellationToken));
        }
    }
}