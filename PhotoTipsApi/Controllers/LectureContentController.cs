using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PhotoTipsApi.Helpers;
using PhotoTipsApi.Models;
using PhotoTipsApi.Repositories;

namespace PhotoTipsApi.Controllers
{
    [Route("api/lecture_content")]
    [ApiController]
    public class LectureContentController : ControllerBase
    {
        private readonly LectureContentRepository _lectureContentRepository;
        private readonly UserRepository _userRepository;
        private readonly string _storageDirectory;

        public LectureContentController(LectureContentRepository lectureContentRepository,
            UserRepository userRepository)
        {
            _lectureContentRepository = lectureContentRepository;
            _userRepository = userRepository;
            _storageDirectory = "images";
        }

        /// <summary>
        /// Получить весь созданный контент
        /// </summary>
        /// <returns>Список с контентом</returns>
        [HttpGet]
        public ActionResult<List<LectureContent>> Get() =>
            _lectureContentRepository.Get();

        /// <summary>
        /// Получить конкретный блок с контентом по его id
        /// </summary>
        /// <param name="id">Идентификатор контента</param>
        /// <returns>Объект контента</returns>
        [HttpGet("{id}")]
        public ActionResult<LectureContent> Get(string id)
        {
            var entity = _lectureContentRepository.Get(id);

            if (entity == null)
            {
                return NotFound();
            }

            return entity;
        }

        /// <summary>
        /// Создать контент
        /// Он бывает трёх типов: Text, Image, Video
        /// Тексту в поле Content следует положить текст,
        /// а видео или картинке - ссылку на файл
        /// </summary>
        /// <param name="entity">Объект контента (без id)</param>
        /// <returns>Созданный контент</returns>
        [HttpPost]
        public ActionResult<LectureContent> Create(LectureContent entity)
        {
            return Ok(new {lecture_content = _lectureContentRepository.Create(entity)});
        }
        
        /// <summary>
        /// Обновить информацию о контенте урока/дз
        /// </summary>
        /// <param name="entity">Изменённый объект модуля (с id)</param>
        /// <returns></returns>
        [HttpPut]
        public IActionResult Update(LectureContent entity)
        {
            var entityToUpdate = _lectureContentRepository.Get(entity.Id);

            if (entityToUpdate == null)
            {
                return NotFound();
            }

            _lectureContentRepository.Update(entity);

            return NoContent();
        }

        
        /// <summary>
        /// Удалить контент по его идентификатору
        /// </summary>
        /// <param name="id">Идентификатор контента</param>
        /// <returns></returns>
        [HttpDelete("{id}")]
        public IActionResult Delete(string id)
        {
            var entity = _lectureContentRepository.Get(id);

            if (entity == null)
            {
                return NotFound();
            }

            _lectureContentRepository.Remove(entity.Id);

            return NoContent();
        }

        /// <summary>
        /// Создать блок контента с текстом
        /// </summary>
        /// <param name="text">Содержание блока (строка с текстом)</param>
        /// <returns></returns>
        [HttpPost("uploadText")]
        public ActionResult<LectureContent> UploadText(string text)
        {
            return Ok(new
            {
                lecture_content = _lectureContentRepository.Create(new LectureContent
                    {Type = LectureContent.ContentType.Text, Content = text})
            });
        }

        /// <summary>
        /// Создать блок контента с видео
        /// </summary>
        /// <param name="request">JWT-токен пользователя и видео-файл</param>
        /// <returns>Созданный блок контента с видео</returns>
        [HttpPost("uploadVideo")]
        public async Task<IActionResult> UploadVideo([FromForm] UploadRequest request)
        {
            var user = new JwtManager().CheckUser(_userRepository, request.UserToken);

            if (user == null)
            {
                return NotFound("User not found");
            }

            if (!user.IsAdmin)
            {
                return BadRequest("Only Admin allowed");
            }

            try
            {
                var fileName = request.File.Name;
                var filePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", _storageDirectory,
                    Path.GetRandomFileName());
                
                using (var stream = System.IO.File.Create(filePath))
                {
                    await request.File.CopyToAsync(stream);
                }
                
                var video = new LectureContent {Type = LectureContent.ContentType.Video, Content = $"{_storageDirectory}/{fileName}"};
                
                return Ok(new {video = video});
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }
        
        /// <summary>
        /// Создать блок контента с изображением
        /// </summary>
        /// <param name="request">JWT-токен пользователя и файл с изображением</param>
        /// <returns>Созданный блок контента с изображением</returns>
        [HttpPost("uploadImage")]
        public async Task<IActionResult> UploadImage([FromForm] UploadRequest request)
        {
            var user = new JwtManager().CheckUser(_userRepository, request.UserToken);

            if (user == null)
            {
                return NotFound("User not found");
            }

            if (!user.IsAdmin)
            {
                return BadRequest("Only Admin allowed");
            }

            try
            {
                var fileName = request.File.Name;
                var filePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", _storageDirectory,
                    Path.GetRandomFileName());
                
                using (var stream = System.IO.File.Create(filePath))
                {
                    await request.File.CopyToAsync(stream);
                }
                
                var image = new LectureContent {Type = LectureContent.ContentType.Image, Content = $"{_storageDirectory}/{fileName}"};
                
                return Ok(new {image = image});
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }
    }
}