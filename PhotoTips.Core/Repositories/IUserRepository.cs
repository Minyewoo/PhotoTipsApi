using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using PhotoTips.Core.Models;

namespace PhotoTips.Core.Repositories
{
    public interface IUserRepository
    {
        public Task<User> FindByEmail(string email, CancellationToken cancellationToken);

        public Task<User> FindByEmailAndPassword(string email, string passwordHash,
            CancellationToken cancellationToken);

        public Task<User> FindByPhoneNumber(string phoneNumber, CancellationToken cancellationToken);

        public Task<User> FindByPhoneNumberAndPassword(string phoneNumber, string passwordHash,
            CancellationToken cancellationToken);

        public Task<IReadOnlyCollection<User>> Get(CancellationToken cancellationToken);

        public Task<IReadOnlyCollection<User>> Get(int? skip, int? count, CancellationToken cancellationToken);
        
        public Task<User> Get(long id, CancellationToken cancellationToken);

        public Task Create(User user, CancellationToken cancellationToken);

        public Task Update(User user, CancellationToken cancellationToken);

        public Task Remove(User user, CancellationToken cancellationToken);

        public Task Remove(long id, CancellationToken cancellationToken);
    }
}