using System;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PhotoTips.Core.Repositories;

namespace PhotoTips.Backoffice.Features.LectureContent
{
    public class UploadImageCommand : IRequest
    {
        public int? IndexNumber { get; set; }
        public IFormFile Image { get; set; }
    }

    public class UploadImageCommandHandler : AsyncRequestHandler<UploadImageCommand>
    {
        private readonly ILectureContentRepository _lectureContentRepository;
        private const string StorageDirectory = "lecture_images";

        public UploadImageCommandHandler(ILectureContentRepository lectureContentRepository)
        {
            _lectureContentRepository = lectureContentRepository;
        }

        protected override async Task Handle(UploadImageCommand request, CancellationToken cancellationToken)
        {
            var fileName = $"{Guid.NewGuid()}_{DateTime.UtcNow.Ticks}.{request.Image.FileName.Split(".").Last()}";
            var filePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", StorageDirectory,
                fileName);

            await using (var stream = File.Create(filePath))
            {
                await request.Image.CopyToAsync(stream, cancellationToken);
                await stream.FlushAsync(cancellationToken);
            }
                

            var image = new Core.Models.LectureContent
            {
                Type = Core.Models.LectureContent.ContentType.Image,
                Content = $"{StorageDirectory}/{fileName}",
                IndexNumber = request.IndexNumber ?? 0
            };

            await _lectureContentRepository.Create(image, cancellationToken);
        }
    }
}