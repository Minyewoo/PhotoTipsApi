using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using PhotoTips.Core.Repositories;

namespace PhotoTips.Backoffice.Features.Submission
{
    public class UpdateSubmissionStatusCommand : IRequest<IActionResult>
    {
        public string SubmissionId { get; set; }
        public Core.Models.Submission.SubmissionStatus Status { get; set; }
    }

    public class UpdateSubmissionStatusCommandHandler : IRequestHandler<UpdateSubmissionStatusCommand, IActionResult>
    {
        private readonly ISubmissionRepository _submissionRepository;

        public UpdateSubmissionStatusCommandHandler(ISubmissionRepository submissionRepository)
        {
            _submissionRepository = submissionRepository;
        }

        public async Task<IActionResult> Handle(UpdateSubmissionStatusCommand request,
            CancellationToken cancellationToken)
        {
            if (request.SubmissionId == null) return new BadRequestObjectResult("SubmissionId is null");

            var submission = await _submissionRepository.Get(request.SubmissionId, cancellationToken);
            if (submission == null)
                return new NotFoundObjectResult($"Submission with id={request.SubmissionId} not found");

            submission.Status = request.Status;

            await _submissionRepository.Update(submission, cancellationToken);

            return new OkResult();
        }
    }
}