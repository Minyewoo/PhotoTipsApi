using System.Threading;
using System.Threading.Tasks;
using MediatR;
using PhotoTips.Core.Repositories;

namespace PhotoTips.Backoffice.Features.Submission
{
    public class DeleteSubmissionCommand : IRequest
    {
        public string SubmissionId { get; set; }
    }
    
    public class DeleteSubmissionCommandHandler : AsyncRequestHandler<DeleteSubmissionCommand>
    {
        private readonly ISubmissionRepository _submissionRepository;

        public DeleteSubmissionCommandHandler(ISubmissionRepository submissionRepository)
        {
            _submissionRepository = submissionRepository;
        }

        protected override async Task Handle(DeleteSubmissionCommand request, CancellationToken cancellationToken)
        {
            await _submissionRepository.Remove(request.SubmissionId, cancellationToken);
        }
    }
}