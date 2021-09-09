using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using PhotoTips.Core.Models;

namespace PhotoTips.Core.Repositories
{
    public interface IModuleEntryRepository
    {
        public Task<IReadOnlyCollection<ModuleEntry>> Get(CancellationToken cancellationToken);

        public Task<IReadOnlyCollection<ModuleEntry>> Get(int? skip, int? count, CancellationToken cancellationToken);
        
        public Task<ModuleEntry> Get(long id, CancellationToken cancellationToken);

        public Task Create(ModuleEntry moduleEntry, CancellationToken cancellationToken);

        public Task Update(ModuleEntry moduleEntry, CancellationToken cancellationToken);

        public Task AddToVideoLecture(long moduleEntryId, long lectureContentId, CancellationToken cancellationToken);

        public Task AddToTextLecture(long moduleEntryId, long lectureContentId, CancellationToken cancellationToken);

        public Task Remove(ModuleEntry moduleEntry, CancellationToken cancellationToken);

        public Task Remove(long id, CancellationToken cancellationToken);
    }
}