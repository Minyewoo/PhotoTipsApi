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

        [HttpGet]
        public ActionResult<List<LectureContent>> Get() =>
            _lectureContentRepository.Get();

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

        [HttpPost]
        public ActionResult<LectureContent> Create(LectureContent entity)
        {
            return Ok(new {lecture_content = _lectureContentRepository.Create(entity)});
        }
        
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

        [HttpPost("uploadText")]
        public ActionResult<LectureContent> UploadText(string text)
        {
            return Ok(new
            {
                lecture_content = _lectureContentRepository.Create(new LectureContent
                    {Type = LectureContent.ContentType.Text, Content = text})
            });
        }

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