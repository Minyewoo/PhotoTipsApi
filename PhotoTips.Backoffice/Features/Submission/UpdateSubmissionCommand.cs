using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using PhotoTips.Core.Repositories;

namespace PhotoTips.Backoffice.Features.Submission
{
    public class UpdateSubmissionCommand : IRequest<IActionResult>
    {
        public string SubmissionId { get; set; }
        public Core.Models.Submission.SubmissionStatus Status { get; set; }
        public string Comment { get; set; }
        public int Mark { get; set; }
    }

    public class UpdateSubmissionCommandHandler : IRequestHandler<UpdateSubmissionCommand, IActionResult>
    {
        private readonly ISubmissionRepository _submissionRepository;

        public UpdateSubmissionCommandHandler(ISubmissionRepository submissionRepository)
        {
            _submissionRepository = submissionRepository;
        }

        public async Task<IActionResult> Handle(UpdateSubmissionCommand request,
            CancellationToken cancellationToken)
        {
            if (request.SubmissionId == null) return new BadRequestObjectResult("SubmissionId is null");

            var submission = await _submissionRepository.Get(request.SubmissionId, cancellationToken);
            if (submission == null)
                return new NotFoundObjectResult($"Submission with id={request.SubmissionId} not found");

            submission.Status = request.Status;
            submission.Mark = request.Mark;
            submission.Comment = request.Comment;

            await _submissionRepository.Update(submission, cancellationToken);

            return new OkResult();
        }
    }
}