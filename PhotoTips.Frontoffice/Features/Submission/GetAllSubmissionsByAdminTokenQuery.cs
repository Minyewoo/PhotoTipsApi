using PhotoTips.Api.DTOs;
using PhotoTips.Core.Repositories;
using PhotoTips.Core.Utils;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace PhotoTips.Frontoffice.Features.Submission
{
    namespace PhotoTips.Frontoffice.Features.Submission
    {
        public class GetAllSubmissionsByAdminTokenQuery : IRequest<IActionResult>
        {
            public string AdminToken { get; set; }
        }

        public class
            GetCheckingSubmissionsByUserTokenQueryHandler : IRequestHandler<GetAllSubmissionsByAdminTokenQuery,
                IActionResult>
        {
            private readonly IUserRepository _userRepository;
            private readonly ISubmissionRepository _submissionRepository;

            public GetCheckingSubmissionsByUserTokenQueryHandler(IUserRepository userRepository,
                ISubmissionRepository submissionRepository)
            {
                _userRepository = userRepository;
                _submissionRepository = submissionRepository;
            }

            public async Task<IActionResult> Handle(GetAllSubmissionsByAdminTokenQuery request,
                CancellationToken cancellationToken)
            {
                var user = await new JwtManager().FindUserByToken(request.AdminToken, _userRepository,
                    cancellationToken);

                if (user == null) return new NotFoundObjectResult("User not found");
                if (!user.IsAdmin) return new BadRequestObjectResult("Only Admin allowed");

                var submissions = await _submissionRepository.Get(cancellationToken);
                return new OkObjectResult(submissions.Select(x => x.ToDto()).ToArray());
            }
        }
    }
}