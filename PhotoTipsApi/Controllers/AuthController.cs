using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using Microsoft.AspNetCore.Mvc;
using PhotoTipsApi.Models;
using PhotoTipsApi.Repositories;
using JetBrains.Annotations;
using JWT;
using JWT.Algorithms;
using JWT.Serializers;
using PhotoTipsApi.Helpers;

namespace PhotoTipsApi.Controllers
{
    [Route("api/auth")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly UserRepository _userRepository;

        public AuthController(UserRepository repository)
        {
            _userRepository = repository;
        }

        [HttpPost("register")]
        public IActionResult Register([FromBody] RegisterRequest request)
        {
            if (!ValidateEmail(request.Email))
                return BadRequest("Invalid email");

            var uniqueError = CheckUnique(request.Email);
            if (uniqueError != null)
                return BadRequest(uniqueError);
            var user = new User
            {
                Email = request.Email.Trim(), PasswordHash = EncryptPassword(request.Password),
                Name = request.Name, RegistrationDate = DateTime.Now,
            };
            return Ok(new {user = _userRepository.Create(user)});
            //return CreatedAtRoute("Login", new {email = user.Email, password = request.Password}, _userRepository.Create(user));
        }

        [HttpGet("login", Name = "Login")]
        public IActionResult Login([CanBeNull] [FromQuery] string email, [CanBeNull] [FromQuery] string phoneNumber,
            [FromQuery] string password)
        {
            const string secret = "43dcd4fa564342456b373347b5f68389ccc3e04c638a1e8735ab1c4cc5a6eea4";
            IJwtAlgorithm algorithm = new HMACSHA256Algorithm(); // symmetric
            IJsonSerializer serializer = new JsonNetSerializer();
            IBase64UrlEncoder urlEncoder = new JwtBase64UrlEncoder();
            IJwtEncoder encoder = new JwtEncoder(algorithm, serializer, urlEncoder);

            var passwordHash = EncryptPassword(password);

            if (string.IsNullOrEmpty(email))
                if (string.IsNullOrEmpty(phoneNumber))
                    return BadRequest("Email or phone number required");
                else
                {
                    if (_userRepository.FindByPhoneNumberAndPassword(phoneNumber, passwordHash) == null)
                        return BadRequest("Wrong phoneNumber or password");
                    return Ok(new
                    {
                        token = new JwtManager().Encode(new Dictionary<string, string>
                            {{"phoneNumber", phoneNumber}, {"passwordHash", EncryptPassword(password)}})
                    });
                }

            if (_userRepository.FindByEmailAndPassword(email, passwordHash) == null)
                return BadRequest("Wrong email or password");
            return Ok(new
            {
                token = new JwtManager().Encode(
                    new Dictionary<string, string>
                        {{"email", email}, {"passwordHash", EncryptPassword(password)}})
            });
        }

        private bool ValidateEmail(string email)
        {
            return email.Contains("@");
        }

        private string EncryptPassword(string password)
        {
            var bytes = new SHA256Managed().ComputeHash(Encoding.ASCII.GetBytes(password));
            return string.Join("", bytes.Select(x => x.ToString("X2")));
        }

        private string CheckUnique(string email)
        {
            if (_userRepository.FindByEmail(email) != null)
                return $"User with email = '{email}' already exists";

            //if (_userRepository.FindByPhoneNumber(phoneNumber) != null)
            //    return $"User with phone number = '{phoneNumber}' already exists";

            return null;
        }
    }
}
