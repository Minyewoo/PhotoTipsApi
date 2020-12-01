using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using PhotoTips.Core.Models;

namespace PhotoTips.Core.Repositories
{
    public interface IPhotoRepository
    {
        public Task<IReadOnlyCollection<Photo>> Get(CancellationToken cancellationToken);

        public Task<IReadOnlyCollection<Photo>> Get(int? skip, int? count, CancellationToken cancellationToken);
        
        public Task<Photo> Get(string id, CancellationToken cancellationToken);

        public Task<Photo> Create(Photo photo, CancellationToken cancellationToken);

        public Task Update(Photo photo, CancellationToken cancellationToken);

        public Task Remove(Photo photo, CancellationToken cancellationToken);

        public Task Remove(string id, CancellationToken cancellationToken);
    }
}