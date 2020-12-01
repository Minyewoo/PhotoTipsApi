using System.Threading;
using System.Threading.Tasks;
using MediatR;
using PhotoTips.Core.Repositories;

namespace PhotoTips.Backoffice.Features.ModuleEntry
{
    public class AddToTextLectureCommand : IRequest
    {
        public long ModuleEntryId { get; set; }
        public long LectureContentId { get; set; }
    }

    public class AddToTextLectureCommandHandler : AsyncRequestHandler<AddToTextLectureCommand>
    {
        private readonly IModuleEntryRepository _moduleEntryRepository;
        private readonly ILectureContentRepository _lectureContentRepository;

        public AddToTextLectureCommandHandler(IModuleEntryRepository moduleEntryRepository,
            ILectureContentRepository lectureContentRepository)
        {
            _moduleEntryRepository = moduleEntryRepository;
            _lectureContentRepository = lectureContentRepository;
        }

        protected override async Task Handle(AddToTextLectureCommand request, CancellationToken cancellationToken)
        {
            var lectureContent = await _lectureContentRepository.Get(request.LectureContentId, cancellationToken);
            var moduleEntry = await _moduleEntryRepository.Get(request.ModuleEntryId, cancellationToken);

            moduleEntry.TextLecture.Add(lectureContent);
            await _moduleEntryRepository.Update(moduleEntry, cancellationToken);
        }
    }
}