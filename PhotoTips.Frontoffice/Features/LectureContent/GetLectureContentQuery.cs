using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using PhotoTips.Core.Repositories;
using PhotoTips.Api.DTOs;

namespace PhotoTips.Frontoffice.Features.LectureContent
{
    public class GetLectureContentQuery : IRequest<IActionResult>
    {
        public int LectureContentId { get; set; }
    }
    
    public class GetLectureContentQueryHandler : IRequestHandler<GetLectureContentQuery, IActionResult>
    {
        private readonly ILectureContentRepository _lectureContentRepository;

        public GetLectureContentQueryHandler(ILectureContentRepository lectureContent)
        {
            _lectureContentRepository = lectureContent;
        }

        public async Task<IActionResult> Handle(GetLectureContentQuery request, CancellationToken cancellationToken)
        {
            var lectureContent = await _lectureContentRepository.Get(request.LectureContentId, cancellationToken);
            
            if (lectureContent == null) return new NotFoundObjectResult("Lecture Content not found");

            return new OkObjectResult(lectureContent.ToDto());
        }
    }
}