using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using PhotoTipsApi.Helpers;
using PhotoTipsApi.Models;
using PhotoTipsApi.Repositories;

namespace PhotoTipsApi.Controllers
{
    [Route("api/user")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly UserRepository _userRepository;
        private readonly PhotoRepository _photoRepository;
        private readonly string _storageDirectory;

        public UserController(UserRepository userRepository, PhotoRepository photoRepository)
        {
            _userRepository = userRepository;
            _photoRepository = photoRepository;
            _storageDirectory = "images";
        }

        /// <summary>
        /// Получить информацию о пользователе по его JWT-токену
        /// </summary>
        /// <param name="token">JWT-токен (выдаётся при логине)</param>
        /// <returns>Объект с информацией о пользователе</returns>
        [HttpGet("{token}")]
        public ActionResult<User> Get(string token)
        {
            var user = new JwtManager().CheckUser(_userRepository, token);

            if (user == null)
            {
                return NotFound("User not found");
            }

            return user;
        }

        /// <summary>
        /// Обновить информацию о пользователе по его JWT-токену
        /// </summary>
        /// <param name="token">JWT-токен (выдаётся при логине)</param>
        /// <param name="updatedUser">Изменённый объект пользователя</param>
        /// <returns>Объект с изменённой информацией о пользователе</returns>
        [HttpPut("{token}")]
        public ActionResult<User> Update(string token, [FromBody] User updatedUser)
        {
            var user = new JwtManager().CheckUser(_userRepository, token);

            if (user == null)
                return NotFound("User not found");

            if (user.Id != updatedUser.Id || !user.IsAdmin)
                return BadRequest("Only Admin or Owner allowed");


            return _userRepository.Update(updatedUser);
        }

        /// <summary>
        /// Добавить фотографию в галерею пользователя
        /// </summary>
        /// <param name="request">Объект с токеном пользователя и файлом</param>
        /// <returns>Добавленное фото</returns>
        [HttpPost("addPhoto")]
        public async Task<IActionResult> AddPhoto([FromForm] UploadRequest request)
        {
            var user = new JwtManager().CheckUser(_userRepository, request.UserToken);

            if (user == null)
            {
                return NotFound("User not found");
            }

            try
            {
                var name = Guid.NewGuid();
                var photoName = $"{name}.png";
                var thumbnailName = $"{name}_thumb.jpg";
                var photoPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", _storageDirectory,
                    photoName);
                var thumbnailPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", _storageDirectory,
                    thumbnailName);

                await using (var photoStream = new MemoryStream())
                {
                    await request.File.CopyToAsync(photoStream);
                    photoStream.Seek(0, SeekOrigin.Begin);
                    
                    SaveImageWithThumbnail(photoStream, photoPath, thumbnailPath);
                }

                var photo = new Photo
                {
                    FileUrl = $"{_storageDirectory}/{photoName}", ThumbnailUrl = $"{_storageDirectory}/{thumbnailName}"
                };

                user.Photos.Add(_photoRepository.Create(photo));
                _userRepository.Update(user);

                return Ok(new {photo = photo});
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }

        private void SaveImageWithThumbnail(Stream resourceImage, string imagePath, string thumbnailPath)
        {
            using var image = Image.FromStream(resourceImage);
            image.RotateFlip(RotateFlipType.Rotate90FlipNone);
            
            var size = image.Size;
            var aspectRatio = size.Width / (double)size.Height;
            const int boxSize = 360;
            var scaleFactor = boxSize / (double) (1 > aspectRatio ? size.Height : size.Width);

            using var thumb = image.GetThumbnailImage((int) (image.Width * scaleFactor), (int) (image.Height * scaleFactor),
                () => false, IntPtr.Zero);

            thumb.Save(thumbnailPath, ImageFormat.Jpeg);
            image.Save(imagePath, ImageFormat.Png);
            
        }

        private ImageCodecInfo GetEncoder(ImageFormat format)
        {
            var codecs = ImageCodecInfo.GetImageEncoders();

            return codecs.FirstOrDefault(codec => codec.FormatID == format.Guid);
        }
    }
}