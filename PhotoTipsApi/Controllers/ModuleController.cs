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

        [HttpPost]
        public ActionResult<Module> Create(Module entity)
        {
            return Ok(new {module = _moduleRepository.Create(entity)});
        }

        [HttpGet("{id}/addModuleEntry")]
        public ActionResult<Module> AddLesson(string id, [FromQuery] string moduleEntryId)
        {
            var module = _moduleRepository.Get(id);
            if (module == null)
                NotFound("Module not found");

            var moduleEntry = _moduleEntryRepository.Get(moduleEntryId);
            if (moduleEntry == null)
                NotFound("Module entry not found");

            module.Entries.Add(moduleEntry);

            return Ok(new {module = _moduleRepository.Update(module)});
        }
    }
}