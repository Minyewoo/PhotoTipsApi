using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using PhotoTips.Core.Models;

namespace PhotoTips.Core.Repositories
{
    public interface ICityRepository
    {
        public Task<IReadOnlyCollection<City>> Get(CancellationToken cancellationToken);

        public Task<IReadOnlyCollection<City>> Get(int? skip, int? count, CancellationToken cancellationToken);
        
        public Task<City> Get(long id, CancellationToken cancellationToken);
        
        public Task Create(City city, CancellationToken cancellationToken);

        public Task Update(City city, CancellationToken cancellationToken);

        public Task Remove(City city, CancellationToken cancellationToken);

        public Task Remove(long id, CancellationToken cancellationToken);
    }
}