using System.Collections.Generic;
using Microsoft.AspNetCore.Http;

namespace PhotoTipsApi.Models
{
    public class RegisterRequest
    {
        public string Name { get; set; }
        public string Surname { get; set; }
        public string Email { get; set; }
        public string PhoneNumber { get; set; }
        public string Password { get; set; }
    }

    public class UploadRequest
    {
        public string UserToken { get; set; }
        public List<IFormFile> Files { get; set; }
    }
}