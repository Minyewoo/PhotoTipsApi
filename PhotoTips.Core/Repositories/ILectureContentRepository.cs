using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using PhotoTips.Core.Models;

namespace PhotoTips.Core.Repositories
{
    public interface ILectureContentRepository
    {
        public Task<IReadOnlyCollection<LectureContent>> Get(CancellationToken cancellationToken);

        public Task<IReadOnlyCollection<LectureContent>> Get(int? skip, int? count, CancellationToken cancellationToken);
        
        public Task<LectureContent> Get(long id, CancellationToken cancellationToken);

        public Task Create(LectureContent lectureContent, CancellationToken cancellationToken);

        public Task Update(LectureContent lectureContent, CancellationToken cancellationToken);

        public Task Remove(LectureContent lectureContent, CancellationToken cancellationToken);

        public Task Remove(long id, CancellationToken cancellationToken);
    }
}