using System.Text.Json.Serialization;
using Microsoft.AspNetCore.Http;

namespace PhotoTipsApi.Models
{
    public class RegisterRequest
    {
        [JsonPropertyName("name")]
        public string Name { get; set; }
        
        [JsonPropertyName("email")]
        public string Email { get; set; }
        
        [JsonPropertyName("password")]
        public string Password { get; set; }
    }

    public class UploadRequest
    {
        [JsonPropertyName("userToken")]
        public string UserToken { get; set; }
        
        [JsonPropertyName("file")]
        public IFormFile File { get; set; }
    }
}
