using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using PhotoTips.Core.Repositories;
using PhotoTips.Api.DTOs;

namespace PhotoTips.Frontoffice.Features.LectureContent
{
    public class GetLectureContentListQuery : IRequest<IActionResult>
    {
        public int? Skip { get; set; }
        public int? Count { get; set; }
    }

    public class GetLectureContentListQueryHandler : IRequestHandler<GetLectureContentListQuery, IActionResult>
    {
        private readonly ILectureContentRepository _lectureContentRepository;

        public GetLectureContentListQueryHandler(ILectureContentRepository lectureContent)
        {
            _lectureContentRepository = lectureContent;
        }

        public async Task<IActionResult> Handle(GetLectureContentListQuery request, CancellationToken cancellationToken)
        {
            var lectureContent =
                await _lectureContentRepository.Get(request.Skip, request.Count, cancellationToken);

            if (lectureContent == null) return new NotFoundObjectResult("Lecture Contents not found");

            return new OkObjectResult(lectureContent.Select(x => x.ToDto()));
        }
    }
}