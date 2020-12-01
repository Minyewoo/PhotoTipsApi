using System;
using System.Collections.Generic;
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
    [Route("api/photo")]
    [ApiController]
    public class PhotoController : ControllerBase
    {
        private readonly PhotoRepository _photoRepository;
        private readonly UserRepository _userRepository;
        private readonly string _storageDirectory;
        
        public PhotoController(PhotoRepository photoRepository, UserRepository userRepository)
        {
            _photoRepository = photoRepository;
            _userRepository = userRepository;
            _storageDirectory = "images";
        }

        /// <summary>
        /// Получить весь созданный контент
        /// </summary>
        /// <returns>Список с контентом</returns>
        [HttpGet]
        public ActionResult<List<Photo>> Get([FromQuery] string token)
        {
            var user = new JwtManager().CheckUser(_userRepository, token);

            if (user == null)
            {
                return NotFound("User not found");
            }

            return user.Photos.ToList();
        }


        /// <summary>
        /// Получить конкретный блок с контентом по его id
        /// </summary>
        /// <param name="id">Идентификатор контента</param>
        /// <param name="token"></param>
        /// <returns>Объект контента</returns>
        [HttpGet("{id}")]
        public ActionResult<Photo> Get(string id, [FromQuery] string token)
        {
            var user = new JwtManager().CheckUser(_userRepository, token);

            if (user == null)
            {
                return NotFound("User not found");
            }

            return user.Photos.FirstOrDefault(photo => photo.Id == id);
        }

        /// <summary>
        /// Создать контент
        /// Он бывает трёх типов: Text, Image, Video
        /// Тексту в поле Content следует положить текст,
        /// а видео или картинке - ссылку на файл
        /// </summary>
        /// <param name="request"></param>
        /// <returns>Созданный контент</returns>
        [HttpPost]
        public async Task<ActionResult<Photo>> Create([FromForm] UploadRequest request)
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
        
        /// <summary>
        /// Обновить информацию о контенте урока/дз
        /// </summary>
        /// <param name="entity">Изменённый объект модуля (с id)</param>
        /// <returns></returns>
        [HttpPut]
        public IActionResult Update(Photo entity)
        {
            var entityToUpdate = _photoRepository.Get(entity.Id);

            if (entityToUpdate == null)
            {
                return NotFound();
            }

            _photoRepository.Update(entity);

            return NoContent();
        }


        /// <summary>
        /// Удалить контент по его идентификатору
        /// </summary>
        /// <param name="id">Идентификатор контента</param>
        /// <returns></returns>
        [HttpDelete("{id}")]
        public IActionResult Delete(string id, [FromQuery] string token)
        {
            var user = new JwtManager().CheckUser(_userRepository, token);

            if (user == null)
            {
                return NotFound("User not found");
            }

            var entity = _photoRepository.Get(id);

            if (entity == null)
            {
                return NotFound();
            }

            _photoRepository.Remove(entity.Id);

            return NoContent();
        }
    }
}