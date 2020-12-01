using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using PhotoTips.Core.Repositories;
using PhotoTips.Core.Utils;

namespace PhotoTips.Backoffice.Features.User
{
    public class UpdateUserInfoQuery : IRequest<IActionResult>
    {
        [Required] public string Token { get; set; }
        public string Name { get; set; }
        public string Surname { get; set; }
        public string Email { get; set; }
        public string PhoneNumber { get; set; }
    }

    public class UpdateUserInfoCommandHandler : IRequestHandler<UpdateUserInfoQuery, IActionResult>
    {
        private readonly IUserRepository _userRepository;

        public UpdateUserInfoCommandHandler(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        public async Task<IActionResult> Handle(UpdateUserInfoQuery request, CancellationToken cancellationToken)
        {
            var user = await new JwtManager().FindUserByToken(request.Token, _userRepository, cancellationToken);

            if (user == null) return new NotFoundObjectResult("User not found");

            var validationResult = await ValidateRequest(user, request, cancellationToken);
            if (!string.IsNullOrEmpty(validationResult)) return new BadRequestObjectResult(validationResult);

            user.Name = request.Name;
            user.Surname = request.Surname;
            user.Email = request.Email;
            user.PhoneNumber = request.PhoneNumber;

            await _userRepository.Update(user, cancellationToken);

            var token = new JwtManager().GetToken(user);
            
            return new OkObjectResult(new {token});
        }

        private async Task<string> ValidateRequest(Core.Models.User user, UpdateUserInfoQuery request,
            CancellationToken cancellationToken)
        {
            if (request.Email != null)
            {
                if (!ValidateEmail(request.Email))
                    return "Invalid email";

                if (request.Email != user.Email)
                {
                    var uniqueEmailError = await CheckIfEmailUnique(request.Email, cancellationToken);
                    if (uniqueEmailError != null)
                        return uniqueEmailError;
                }
            }
            else if (request.PhoneNumber != null)
            {
                if (!ValidatePhoneNumber(request.PhoneNumber))
                    return "Invalid phone number";

                if (request.PhoneNumber != user.PhoneNumber)
                {
                    var uniquePhoneNumberError = await CheckIfPhoneNumberUnique(request.PhoneNumber, cancellationToken);
                    if (uniquePhoneNumberError != null)
                        return uniquePhoneNumberError;
                }
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