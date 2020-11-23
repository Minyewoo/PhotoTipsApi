using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
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

                using (var photoStream = System.IO.File.Create(photoPath))
                {
                    await request.File.CopyToAsync(photoStream);
                    photoStream.Position = 0;
                    GetReducedImage(200,200, photoStream)?.Save(thumbnailPath, ImageFormat.Jpeg);
                }

                var photo = new Photo {FileUrl = $"{_storageDirectory}/{photoName}", ThumbnailUrl = $"{_storageDirectory}/{thumbnailName}"};
                
                user.Photos.Add(_photoRepository.Create(photo));
                _userRepository.Update(user);
                
                return Ok(new {photo = photo});
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }

        private Image GetReducedImage(int width, int height, Stream resourceImage)
        {
            try
            {
                Image image = Image.FromStream(resourceImage);
                Image thumb = image.GetThumbnailImage(width, height, ()=> false, IntPtr.Zero);
                return thumb;
            }
            catch (Exception e)
            {
                return null;
            }
        }
    }
}