using System.Threading;
using System.Threading.Tasks;
using MediatR;
using PhotoTips.Core.Repositories;
using PhotoTips.Core.Utils;

namespace PhotoTips.Backoffice.Features.User
{
    public class DeleteUserCommand : IRequest
    {
        public string Token { get; set; }
    }

    public class DeleteUserCommandHandler : AsyncRequestHandler<DeleteUserCommand>
    {
        private readonly IUserRepository _userRepository;

        public DeleteUserCommandHandler(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        protected override async Task Handle(DeleteUserCommand request, CancellationToken cancellationToken)
        {
            var user = await new JwtManager().FindUserByToken(request.Token, _userRepository, cancellationToken);
            if(user != null)
                await _userRepository.Remove(user.Id, cancellationToken);
        }
    }
}