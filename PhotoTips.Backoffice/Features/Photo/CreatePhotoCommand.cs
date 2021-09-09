using System;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Http;
using System.Drawing;
using System.Drawing.Imaging;
using Microsoft.AspNetCore.Mvc;
using PhotoTips.Core.Repositories;
using PhotoTips.Core.Utils;

namespace PhotoTips.Backoffice.Features.Photo
{
    public class CreatePhotoCommand : IRequest<IActionResult>
    {
        public string UserToken { get; set; }
        public IFormFile File { get; set; }
    }

    public class CreatePhotoCommandHandler : IRequestHandler<CreatePhotoCommand, IActionResult>
    {
        private readonly IUserRepository _userRepository;
        private readonly IPhotoRepository _photoRepository;
        private const string StorageDirectory = "images";

        public CreatePhotoCommandHandler(IUserRepository userRepository, IPhotoRepository photoRepository)
        {
            _userRepository = userRepository;
            _photoRepository = photoRepository;
        }

        public async Task<IActionResult> Handle(CreatePhotoCommand request, CancellationToken cancellationToken)
        {
            var user = await new JwtManager().FindUserByToken(request.UserToken, _userRepository, cancellationToken);

            if (user == null) return new NotFoundObjectResult("User not found");

            try
            {
                var name =$"{Guid.NewGuid().ToString()}_{DateTime.UtcNow.Ticks.ToString()}";
                var photoName = $"{name}.jpg";
                var thumbnailName = $"{name}_thumb.jpg";
                var photoPath = Path.Combine(
                    Directory.GetCurrentDirectory(),
                    "wwwroot", StorageDirectory,
                    photoName);
                var thumbnailPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", StorageDirectory,
                    thumbnailName);

                await using (var photoStream = new MemoryStream())
                {
                    await request.File.CopyToAsync(photoStream, cancellationToken);
                    photoStream.Seek(0, SeekOrigin.Begin);

                    SaveImageWithThumbnail(photoStream, photoPath, thumbnailPath);
                }

                var photo = new Core.Models.Photo
                {
                    FileUrl = $"{StorageDirectory}/{photoName}", ThumbnailUrl = $"{StorageDirectory}/{thumbnailName}"
                };

                var createdPhoto = await _photoRepository.Create(photo, cancellationToken);
                user.Photos.Add(createdPhoto);
                await _userRepository.Update(user, cancellationToken);
                //await _photoRepository.Create(photo, cancellationToken);
                
                return new OkResult();
            }
            catch (Exception e)
            {
                return new BadRequestObjectResult(e.Message);
            }
        }

        private void SaveImageWithThumbnail(Stream resourceImage, string imagePath, string thumbnailPath)
        {
            using var image = Image.FromStream(resourceImage);
            image.RotateFlip(RotateFlipType.Rotate90FlipNone);

            var size = image.Size;
            var aspectRatio = size.Width / (double) size.Height;
            const int boxSize = 360;
            var scaleFactor = boxSize / (double) (1 > aspectRatio ? size.Height : size.Width);

            using var thumb = image.GetThumbnailImage((int) (image.Width * scaleFactor),
                (int) (image.Height * scaleFactor),
                () => false, IntPtr.Zero);

            thumb.Save(thumbnailPath, ImageFormat.Jpeg);
            image.Save(imagePath, ImageFormat.Jpeg);
        }
    }
}