using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PhotoTips.Core.Repositories;
using PhotoTips.Core.Utils;

namespace PhotoTips.Backoffice.Features.Submission
{
    public class CreateSubmissionCommand : IRequest<IActionResult>
    {
        public string UserToken { get; set; }
        public IFormFile File { get; set; }
        public long ModuleEntryId { get; set; }
    }

    public class CreateSubmissionCommandHandler : IRequestHandler<CreateSubmissionCommand, IActionResult>
    {
        private readonly IUserRepository _userRepository;
        private readonly IPhotoRepository _photoRepository;
        private readonly IModuleEntryRepository _moduleEntryRepository;
        private readonly ISubmissionRepository _submissionRepository;
        private const string StorageDirectory = "images";

        public CreateSubmissionCommandHandler(IUserRepository userRepository, IPhotoRepository photoRepository,
            IModuleEntryRepository moduleEntryRepository, ISubmissionRepository submissionRepository)
        {
            _userRepository = userRepository;
            _photoRepository = photoRepository;
            _moduleEntryRepository = moduleEntryRepository;
            _submissionRepository = submissionRepository;
        }

        public async Task<IActionResult> Handle(CreateSubmissionCommand request, CancellationToken cancellationToken)
        {
            var user = await new JwtManager().FindUserByToken(request.UserToken, _userRepository, cancellationToken);

            if (user == null) return new NotFoundObjectResult("User not found");

            var moduleEntry = await _moduleEntryRepository.Get(request.ModuleEntryId, cancellationToken);
            if (moduleEntry == null)
                return new NotFoundObjectResult($"Module Entry with id={request.ModuleEntryId} not found");

            var submission = new Core.Models.Submission
            {
                Submitter = user,
                ModuleEntry = moduleEntry,
                Status = Core.Models.Submission.SubmissionStatus.Checking,
                Time = DateTime.Now
            };

            try
            {
                if (moduleEntry.Type == Core.Models.ModuleEntry.ModuleEntryType.Homework)
                {
                    var name = $"{Guid.NewGuid().ToString()}_{DateTime.UtcNow.Ticks.ToString()}";
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
                        FileUrl = $"{StorageDirectory}/{photoName}",
                        ThumbnailUrl = $"{StorageDirectory}/{thumbnailName}"
                    };

                    var createdPhoto = await _photoRepository.Create(photo, cancellationToken);
                    user.Photos.Add(createdPhoto);
                    await _userRepository.Update(user, cancellationToken);

                    submission.Photo = createdPhoto;
                }

                await _submissionRepository.Create(submission, cancellationToken);

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
            //image.RotateFlip(RotateFlipType.Rotate90FlipNone);
            
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