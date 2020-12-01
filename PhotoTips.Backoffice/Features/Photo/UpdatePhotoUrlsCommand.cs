#nullable enable
using System.ComponentModel.DataAnnotations;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using PhotoTips.Core.Repositories;

namespace PhotoTips.Backoffice.Features.Photo
{
    public class UpdatePhotoUrlsCommand : IRequest<IActionResult>
    {
        [Required] public string? PhotoId { get; set; }
        public string? FileUrl { get; set; }
        public string? ThumbnailUrl { get; set; }
    }

    public class UpdatePhotoUrlsCommandHandler : IRequestHandler<UpdatePhotoUrlsCommand, IActionResult>
    {
        private readonly IPhotoRepository _photoRepository;

        public UpdatePhotoUrlsCommandHandler(IPhotoRepository photoRepository)
        {
            _photoRepository = photoRepository;
        }

        public async Task<IActionResult> Handle(UpdatePhotoUrlsCommand request, CancellationToken cancellationToken)
        {
            if (request.PhotoId == null) return new BadRequestObjectResult("PhotoId is null");

            var photo = await _photoRepository.Get(request.PhotoId, cancellationToken);
            if (photo == null) return new NotFoundObjectResult($"Photo with id={request.PhotoId} not found");

            photo.FileUrl = request.FileUrl ?? photo.FileUrl;
            photo.ThumbnailUrl = request.ThumbnailUrl ?? photo.ThumbnailUrl;

            await _photoRepository.Update(photo, cancellationToken);

            return new OkResult();
        }
    }
}