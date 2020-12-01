using System.ComponentModel.DataAnnotations;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using PhotoTips.Core.Repositories;

namespace PhotoTips.Backoffice.Features.LectureContent
{
    public class UpdateLectureContentCommand : IRequest<IActionResult>
    {
        [Required] public long LectureContentId { get; set; }
        public int? IndexNumber { get; set; }
        public Core.Models.LectureContent.ContentType? Type { get; set; }
        public string Content { get; set; }
    }

    public class UpdateLectureContentCommandHandler : IRequestHandler<UpdateLectureContentCommand, IActionResult>
    {
        private readonly ILectureContentRepository _lectureContentRepository;

        public UpdateLectureContentCommandHandler(ILectureContentRepository lectureContentRepository)
        {
            _lectureContentRepository = lectureContentRepository;
        }

        public async Task<IActionResult> Handle(UpdateLectureContentCommand request,
            CancellationToken cancellationToken)
        {
            var lectureContent = await _lectureContentRepository.Get(request.LectureContentId, cancellationToken);
            if (lectureContent == null)
                return new NotFoundObjectResult($"Lecture Content with id={request.LectureContentId} not found");

            lectureContent.IndexNumber = request.IndexNumber ?? 0;
            lectureContent.Type = request.Type ?? lectureContent.Type;
            lectureContent.Content = request.Content;

            await _lectureContentRepository.Update(lectureContent, cancellationToken);

            return new OkResult();
        }
    }
}