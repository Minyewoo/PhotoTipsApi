using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using PhotoTipsApi.Models;
using PhotoTipsApi.Repositories;

namespace PhotoTipsApi.Controllers
{
    [Route("api/module")]
    [ApiController]
    public class ModuleController : ControllerBase
    {
        private readonly ModuleRepository _moduleRepository;
        private readonly ModuleEntryRepository _moduleEntryRepository;

        public ModuleController(ModuleRepository moduleRepository, ModuleEntryRepository moduleEntryRepository)
        {
            _moduleRepository = moduleRepository;
            _moduleEntryRepository = moduleEntryRepository;
        }

        /// <summary>
        /// Получить все модули и их содержание
        /// </summary>
        /// <returns>Список модулей, уроков внутри модулей и контентом внутри уроков</returns>
        [HttpGet]
        public ActionResult<List<Module>> Get() =>
            _moduleRepository.Get();

        [HttpGet("{id}")]
        public ActionResult<Module> Get(string id)
        {
            var entity = _moduleRepository.Get(id);

            if (entity == null)
            {
                return NotFound();
            }

            return entity;
        }

        /// <summary>
        /// Обновить информацию о модуле
        /// </summary>
        /// <param name="entity">Изменённый объект модуля (с id)</param>
        /// <returns></returns>
        [HttpPut]
        public IActionResult Update(Module entity)
        {
            var entityToUpdate = _moduleRepository.Get(entity.Id);

            if (entityToUpdate == null)
            {
                return NotFound();
            }

            _moduleRepository.Update(entity);

            return NoContent();
        }

        /// <summary>
        /// Удалить модуль по его идентификатору
        /// </summary>
        /// <param name="id">Идентификатор модуля</param>
        /// <returns></returns>
        [HttpDelete("{id}")]
        public IActionResult Delete(string id)
        {
            var entity = _moduleRepository.Get(id);

            if (entity == null)
            {
                return NotFound();
            }

            _moduleRepository.Remove(entity.Id);

            return NoContent();
        }

        /// <summary>
        /// Создать модуль
        /// </summary>
        /// <param name="entity">Объект модуля без id и всяких списков</param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult<Module> Create(Module entity)
        {
            return Ok(new {module = _moduleRepository.Create(entity)});
        }

        /// <summary>
        /// Добавить урок/дз в модуль
        /// </summary>
        /// <param name="id">Идентификатор модуля</param>
        /// <param name="moduleEntryId">Идентификатор урока/дз</param>
        /// <returns>Обновлённый модуль</returns>
        [HttpGet("{id}/addModuleEntry")]
        public ActionResult<Module> AddLesson(string id, [FromQuery] string moduleEntryId)
        {
            var module = _moduleRepository.Get(id);
            if (module == null)
                NotFound("Module not found");

            var moduleEntry = _moduleEntryRepository.Get(moduleEntryId);
            if (moduleEntry == null)
                NotFound("Module entry not found");

            return Ok(new {module = _moduleRepository.AddModuleEntry(id, moduleEntryId)});
        }
    }
}