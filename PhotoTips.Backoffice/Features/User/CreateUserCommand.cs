using System;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using PhotoTips.Core.Repositories;

namespace PhotoTips.Backoffice.Features.User
{
    public class CreateUserCommand : IRequest<string>
    {
        public string Name { get; set; }
        public string Surname { get; set; }
        public string Email { get; set; }
        public string PhoneNumber { get; set; }
        [Required] public string Password { get; set; }
    }

    public class CreateUserCommandHandler : IRequestHandler<CreateUserCommand, string>
    {
        private readonly IUserRepository _userRepository;

        public CreateUserCommandHandler(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        public async Task<string> Handle(CreateUserCommand request, CancellationToken cancellationToken)
        {
            var validationResult = await ValidateRequest(request, cancellationToken);
            if (!string.IsNullOrEmpty(validationResult)) return validationResult;

            var user = new Core.Models.User
            {
                Name = request.Name,
                Surname = request.Surname,
                Email = request.Email,
                PhoneNumber = request.PhoneNumber,
                PasswordHash = EncryptPassword(request.Password),
                RegistrationDate = DateTime.UtcNow,
            };

            await _userRepository.Create(user, cancellationToken);

            return null;
        }

        private string EncryptPassword(string password)
        {
            var bytes = new SHA256Managed().ComputeHash(Encoding.ASCII.GetBytes(password));
            return string.Join("", bytes.Select(x => x.ToString("X2")));
        }

        private async Task<string> ValidateRequest(CreateUserCommand request, CancellationToken cancellationToken)
        {
            if (request.Email != null)
            {
                if (!ValidateEmail(request.Email))
                    return "Invalid email";

                var uniqueEmailError = await CheckIfEmailUnique(request.Email, cancellationToken);
                if (uniqueEmailError != null)
                    return uniqueEmailError;
            }
            else if (request.PhoneNumber != null)
            {
                if (!ValidatePhoneNumber(request.PhoneNumber))
                    return "Invalid phone number";

                var uniquePhoneNumberError = await CheckIfPhoneNumberUnique(request.PhoneNumber, cancellationToken);
                if (uniquePhoneNumberError != null)
                    return uniquePhoneNumberError;
            }
            else return "Email or Phone Number requested";

            return null;
        }

        private bool ValidateEmail(string email)
        {
            return Regex.IsMatch(email, @"^[a-zA-Z0-9.]+@[a-zA-Z0-9]+\.[a-zA-Z]+");
        }

        private bool ValidatePhoneNumber(string phoneNumber)
        {
            return Regex.IsMatch(phoneNumber, @"[0-9]{11}");
        }

        private async Task<string> CheckIfEmailUnique(string email, CancellationToken cancellationToken)
        {
            var user = await _userRepository.FindByEmail(email, cancellationToken);
            return user != null
                ? $"User with email = '{email}' already exists"
                : null;
        }

        private async Task<string> CheckIfPhoneNumberUnique(string phoneNumber, CancellationToken cancellationToken)
        {
            var user = await _userRepository.FindByPhoneNumber(phoneNumber, cancellationToken);
            return user != null ? $"User with phoneNumber = '{phoneNumber}' already exists" : null;
        }
    }
}