using System.Threading;
using System.Threading.Tasks;
using MediatR;
using PhotoTips.Core.Repositories;

namespace PhotoTips.Backoffice.Features.ModuleEntry
{
    public class AddToVideoLectureCommand : IRequest
    {
        public long ModuleEntryId { get; set; }
        public long LectureContentId { get; set; }
    }

    public class AddToVideoLectureCommandHandler : AsyncRequestHandler<AddToVideoLectureCommand>
    {
        private readonly IModuleEntryRepository _moduleEntryRepository;
        private readonly ILectureContentRepository _lectureContentRepository;

        public AddToVideoLectureCommandHandler(IModuleEntryRepository moduleEntryRepository,
            ILectureContentRepository lectureContentRepository)
        {
            _moduleEntryRepository = moduleEntryRepository;
            _lectureContentRepository = lectureContentRepository;
        }

        protected override async Task Handle(AddToVideoLectureCommand request, CancellationToken cancellationToken)
        {
            var lectureContent = await _lectureContentRepository.Get(request.LectureContentId, cancellationToken);
            var moduleEntry = await _moduleEntryRepository.Get(request.ModuleEntryId, cancellationToken);

            moduleEntry.VideoLecture.Add(lectureContent);
            await _moduleEntryRepository.Update(moduleEntry, cancellationToken);
        }
    }
}