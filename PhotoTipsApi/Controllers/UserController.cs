using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
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
        private readonly string _storageDirectory;

        public UserController(UserRepository repository)
        {
            _userRepository = repository;
            _storageDirectory = "images";
        }

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

        [HttpPost("{token}/addPhoto")]
        public async Task<IActionResult> AddPhoto(string token, [FromBody] List<IFormFile> files)
        {
            var user = new JwtManager().CheckUser(_userRepository, token);

            if (user == null)
            {
                return NotFound("User not found");
            }

            var result = new List<Photo>();

            foreach (var formFile in files)
            {
                if (formFile.Length > 0)
                {
                    var fileName = $@"{Guid.NewGuid()}";
                    var filePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", _storageDirectory,
                        Path.GetRandomFileName());

                    using (var stream = System.IO.File.Create(filePath))
                    {
                        await formFile.CopyToAsync(stream);
                    }

                    var photo = new Photo {FileUrl = $"~/{_storageDirectory}/{fileName}"};
                    var photos = user.Photos.ToList();
                    photos.Add(photo);
                    user.Photos=photos.ToArray();
                    result.Add(photo);
                }
            }

            return Ok(new {addedPhotos = result});
        }
    }
}