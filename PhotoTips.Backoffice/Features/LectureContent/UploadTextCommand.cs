using System.Threading;
using System.Threading.Tasks;
using MediatR;
using PhotoTips.Core.Repositories;

namespace PhotoTips.Backoffice.Features.LectureContent
{
    public class UploadTextCommand : IRequest
    {
        public int? IndexNumber { get; set; }
        public string Text { get; set; }
    }
    
    public class UploadTextCommandHandler : AsyncRequestHandler<UploadTextCommand>
    {
        private readonly ILectureContentRepository _lectureContentRepository;

        public UploadTextCommandHandler(ILectureContentRepository lectureContentRepository)
        {
            _lectureContentRepository = lectureContentRepository;
        }

        protected override async Task Handle(UploadTextCommand request, CancellationToken cancellationToken)
        {
            var lectureContent = new Core.Models.LectureContent
            {
                IndexNumber = request.IndexNumber ?? 0,
                Type = Core.Models.LectureContent.ContentType.Text,
                Content = request.Text,
            };
            
            await _lectureContentRepository.Create(lectureContent, cancellationToken);
        }
    }
}