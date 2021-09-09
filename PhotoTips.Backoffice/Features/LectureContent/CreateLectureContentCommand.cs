using System.Threading;
using System.Threading.Tasks;
using MediatR;
using PhotoTips.Core.Repositories;

namespace PhotoTips.Backoffice.Features.LectureContent
{
    public class CreateLectureContentCommand : IRequest
    {
        public int IndexNumber { get; set; }
        public Core.Models.LectureContent.ContentType Type { get; set; }
        public string Content { get; set; }
    }
    
    public class CreateLectureContentCommandHandler : AsyncRequestHandler<CreateLectureContentCommand>
    {
        private readonly ILectureContentRepository _lectureContentRepository;

        public CreateLectureContentCommandHandler(ILectureContentRepository lectureContentRepository)
        {
            _lectureContentRepository = lectureContentRepository;
        }

        protected override async Task Handle(CreateLectureContentCommand request, CancellationToken cancellationToken)
        {
            var lectureContent = new Core.Models.LectureContent
            {
                IndexNumber = request.IndexNumber,
                Type = request.Type,
                Content = request.Content,
            };
            
            await _lectureContentRepository.Create(lectureContent, cancellationToken);
        }
    }
}