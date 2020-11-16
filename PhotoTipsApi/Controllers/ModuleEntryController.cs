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
    [Route("api/module_entry")]
    [ApiController]
    public class ModuleEntryController : ControllerBase
    {
        private readonly ModuleRepository _moduleRepository;
        private readonly ModuleEntryRepository _moduleEntryRepository;
        private readonly LectureContentRepository _lectureContentRepository;
        private readonly string _storageDirectory;

        public ModuleEntryController(ModuleRepository moduleRepository, ModuleEntryRepository moduleEntryRepository,
            LectureContentRepository lectureContentRepository)
        {
            _moduleRepository = moduleRepository;
            _moduleEntryRepository = moduleEntryRepository;
            _lectureContentRepository = lectureContentRepository;
        }

        [HttpGet]
        public ActionResult<List<ModuleEntry>> Get() =>
            _moduleEntryRepository.Get();

        [HttpGet("{id}")]
        public ActionResult<ModuleEntry> Get(string id)
        {
            var entity = _moduleEntryRepository.Get(id);

            if (entity == null)
            {
                return NotFound();
            }

            return entity;
        }

        [HttpPut]
        public IActionResult Update(ModuleEntry entity)
        {
            var entityToUpdate = _moduleEntryRepository.Get(entity.Id);

            if (entityToUpdate == null)
            {
                return NotFound();
            }

            _moduleEntryRepository.Update(entity);

            return NoContent();
        }

        [HttpDelete("{id}")]
        public IActionResult Delete(string id)
        {
            var entity = _moduleEntryRepository.Get(id);

            if (entity == null)
            {
                return NotFound();
            }

            _moduleEntryRepository.Remove(entity.Id);

            return NoContent();
        }

        [HttpPost]
        public ActionResult<ModuleEntry> Create(ModuleEntry entity)
        {
            return Ok(new {module_entry = _moduleEntryRepository.Create(entity)});
        }

        [HttpGet("{id}/addToModule")]
        public ActionResult<ModuleEntry> AddToModule(string id, [FromQuery] string moduleId)
        {
            var module = _moduleRepository.Get(moduleId);
            if (module == null)
                NotFound("Module not found");

            var moduleEntry = _moduleEntryRepository.Get(id);
            if (moduleEntry == null)
                NotFound("Module entry not found");

            module.Entries.Add(moduleEntry);

            return Ok(new {module = _moduleRepository.Update(module)});
        }

        [HttpGet("{id}/addContentToVideoLecture")]
        public ActionResult<ModuleEntry> AddContentToVideoLecture(string id, [FromQuery] string lectureContentId)
        {
            var moduleEntry = _moduleEntryRepository.Get(id);
            if (moduleEntry == null)
                NotFound("Module entry not found");
            
            var lectureContent = _lectureContentRepository.Get(lectureContentId);
            if (lectureContent == null)
                NotFound("Module not found");

            moduleEntry.VideoLecture.Add(lectureContent);

            return Ok(new {module = _moduleEntryRepository.Update(moduleEntry)});
        }
        
        [HttpGet("{id}/addContentToTextLecture")]
        public ActionResult<ModuleEntry> AddContentToTextLecture(string id, [FromQuery] string lectureContentId)
        {
            var moduleEntry = _moduleEntryRepository.Get(id);
            if (moduleEntry == null)
                NotFound("Module entry not found");
            
            var lectureContent = _lectureContentRepository.Get(lectureContentId);
            if (lectureContent == null)
                NotFound("Module not found");

            moduleEntry.TextLecture.Add(lectureContent);

            return Ok(new {module = _moduleEntryRepository.Update(moduleEntry)});
        }
    }
}