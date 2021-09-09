using System.Threading;
using System.Threading.Tasks;
using MediatR;
using PhotoTips.Core.Repositories;

namespace PhotoTips.Backoffice.Features.Photo
{
    public class DeletePhotoCommand : IRequest
    {
        public string PhotoId { get; set; }
    }
    
    public class DeletePhotoCommandHandler : AsyncRequestHandler<DeletePhotoCommand>
    {
        private readonly IPhotoRepository _photoRepository;

        public DeletePhotoCommandHandler(IPhotoRepository photoRepository)
        {
            _photoRepository = photoRepository;
        }

        protected override async Task Handle(DeletePhotoCommand request, CancellationToken cancellationToken)
        {
            await _photoRepository.Remove(request.PhotoId, cancellationToken);
        }
    }
}