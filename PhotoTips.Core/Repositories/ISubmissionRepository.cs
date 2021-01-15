using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using PhotoTips.Core.Models;

namespace PhotoTips.Core.Repositories
{
    public interface ISubmissionRepository
    {
        public Task<IReadOnlyCollection<Submission>> Get(CancellationToken cancellationToken);

        public Task<IReadOnlyCollection<Submission>> Get(int? skip, int? count, CancellationToken cancellationToken);
        
        public Task<Submission> Get(string id, CancellationToken cancellationToken);
        
        public Task<IReadOnlyCollection<Submission>> Get(User user, CancellationToken cancellationToken);

        public Task<Submission> Create(Submission submission, CancellationToken cancellationToken);

        public Task Update(Submission submission, CancellationToken cancellationToken);

        public Task Remove(Submission submission, CancellationToken cancellationToken);

        public Task Remove(string id, CancellationToken cancellationToken);
    }
}