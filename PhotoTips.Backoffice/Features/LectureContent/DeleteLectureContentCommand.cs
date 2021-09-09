using System.Threading;
using System.Threading.Tasks;
using MediatR;
using PhotoTips.Core.Repositories;

namespace PhotoTips.Backoffice.Features.LectureContent
{
    public class DeleteLectureContentCommand : IRequest
    {
        public long LectureContentId { get; set; }
    }
    
    public class DeleteLectureContentCommandHandler : AsyncRequestHandler<DeleteLectureContentCommand>
    {
        private readonly ILectureContentRepository _lectureContentRepository;

        public DeleteLectureContentCommandHandler(ILectureContentRepository lectureContentRepository)
        {
            _lectureContentRepository = lectureContentRepository;
        }

        protected override async Task Handle(DeleteLectureContentCommand request, CancellationToken cancellationToken)
        {
            await _lectureContentRepository.Remove(request.LectureContentId, cancellationToken);
        }
    }
}