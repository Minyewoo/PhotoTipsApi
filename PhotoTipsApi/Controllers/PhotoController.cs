using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
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
        
        public PhotoController(PhotoRepository photoRepository, UserRepository userRepository)
        {
            _photoRepository = photoRepository;
            _userRepository = userRepository;
        }
        
         /// <summary>
        /// Получить весь созданный контент
        /// </summary>
        /// <returns>Список с контентом</returns>
        [HttpGet]
        public ActionResult<List<Photo>> Get([FromQuery] string token) =>
            _photoRepository.Get();

         /// <summary>
         /// Получить конкретный блок с контентом по его id
         /// </summary>
         /// <param name="id">Идентификатор контента</param>
         /// <param name="token"></param>
         /// <returns>Объект контента</returns>
         [HttpGet("{id}")]
        public ActionResult<Photo> Get(string id,[FromQuery] string token)
        {
            var entity = _photoRepository.Get(id);

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
        public ActionResult<Photo> Create(Photo entity)
        {
            return Ok(new {lecture_content = _photoRepository.Create(entity)});
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
        public IActionResult Delete(string id)
        {
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