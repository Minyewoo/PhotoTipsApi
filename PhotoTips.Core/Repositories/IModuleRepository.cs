using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using PhotoTips.Core.Models;

namespace PhotoTips.Core.Repositories
{
    public interface IModuleRepository
    {
        public Task<IReadOnlyCollection<Module>> Get(CancellationToken cancellationToken);

        public Task<IReadOnlyCollection<Module>> Get(int? skip, int? count, CancellationToken cancellationToken);
        
        
        public Task<Module> Get(long id, CancellationToken cancellationToken);

        public Task Create(Module module, CancellationToken cancellationToken);

        public Task Update(Module module, CancellationToken cancellationToken);

        public Task AddModuleEntry(long moduleId, long moduleEntryId, CancellationToken cancellationToken);

        public Task Remove(Module module, CancellationToken cancellationToken);

        public Task Remove(long id, CancellationToken cancellationToken);
    }
}