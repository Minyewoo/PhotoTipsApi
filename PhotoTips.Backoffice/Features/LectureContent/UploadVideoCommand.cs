using System;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Http;
using PhotoTips.Core.Repositories;

namespace PhotoTips.Backoffice.Features.LectureContent
{
    public class UploadVideoCommand : IRequest
    {
        public int? IndexNumber { get; set; }
        public IFormFile Video { get; set; }
    }

    public class UploadVideoCommandHandler : AsyncRequestHandler<UploadVideoCommand>
    {
        private readonly ILectureContentRepository _lectureContentRepository;
        private const string StorageDirectory = "lecture_videos";

        public UploadVideoCommandHandler(ILectureContentRepository lectureContentRepository)
        {
            _lectureContentRepository = lectureContentRepository;
        }

        protected override async Task Handle(UploadVideoCommand request, CancellationToken cancellationToken)
        {
            var fileName = $"{Guid.NewGuid()}_{DateTime.UtcNow.Ticks}.{request.Video.FileName.Split(".").Last()}";
            var filePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", StorageDirectory,
                fileName);

            await using (var stream = File.Create(filePath))
            {
                await request.Video.CopyToAsync(stream, cancellationToken);
                await stream.FlushAsync(cancellationToken);
            }


            var video = new Core.Models.LectureContent
            {
                Type = Core.Models.LectureContent.ContentType.Video, Content = $"{StorageDirectory}/{fileName}",
                IndexNumber = request.IndexNumber ?? 0
            };

            await _lectureContentRepository.Create(video, cancellationToken);
        }
    }
}