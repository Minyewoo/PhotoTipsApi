using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using PhotoTips.Core.Repositories;
using PhotoTips.Core.Utils;

namespace PhotoTips.Backoffice.Features.User
{
    public class UpdatePasswordQuery : IRequest<IActionResult>
    {
        [Required] public string Token { get; set; }
        [Required] public string OldPassword { get; set; }
        [Required] public string NewPassword { get; set; }
    }

    public class UpdatePasswordQueryHandler : IRequestHandler<UpdatePasswordQuery, IActionResult>
    {
        private readonly IUserRepository _userRepository;

        public UpdatePasswordQueryHandler(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        public async Task<IActionResult> Handle(UpdatePasswordQuery request, CancellationToken cancellationToken)
        {
            var user = await new JwtManager().FindUserByToken(request.Token, _userRepository, cancellationToken);

            if (user == null) return new NotFoundObjectResult("User not found");

            if (user.PasswordHash != EncryptPassword(request.OldPassword))
                return new BadRequestObjectResult("Wrong old password");

            user.PasswordHash = EncryptPassword(request.NewPassword);

            await _userRepository.Update(user, cancellationToken);

            var token = new JwtManager().GetToken(user);

            return new OkObjectResult(new {token});
        }

        private string EncryptPassword(string password)
        {
            var bytes = new SHA256Managed().ComputeHash(Encoding.ASCII.GetBytes(password));
            return string.Join("", bytes.Select(x => x.ToString("X2")));
        }
    }
}