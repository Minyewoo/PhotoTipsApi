using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using PhotoTips.Core.Models;
using PhotoTips.Core.Repositories;

namespace PhotoTips.Infrastructure.Repositories
{
    public class UserEfRepository : IUserRepository
    {
        private readonly PhotoTipsDbContext _context;

        public UserEfRepository(PhotoTipsDbContext context)
        {
            _context = context;
        }

        public async Task<User> FindByEmail(string email, CancellationToken cancellationToken)
        {
            return await _context.Users.Include(x => x.Photos).Include(x => x.ResidenceCity)
                .SingleOrDefaultAsync(user => user.Email == email, cancellationToken);
        }

        public async Task<User> FindByEmailAndPassword(string email, string passwordHash,
            CancellationToken cancellationToken)
        {
            return await _context.Users.Include(x => x.Photos).Include(x => x.ResidenceCity)
                .SingleOrDefaultAsync(user => user.Email == email && user.PasswordHash == passwordHash,
                    cancellationToken);
        }

        public async Task<User> FindByPhoneNumber(string phoneNumber, CancellationToken cancellationToken)
        {
            return await _context.Users.Include(x => x.Photos).Include(x => x.ResidenceCity)
                .SingleOrDefaultAsync(user => user.PhoneNumber == phoneNumber, cancellationToken);
        }

        public async Task<User> FindByPhoneNumberAndPassword(string phoneNumber, string passwordHash,
            CancellationToken cancellationToken)
        {
            return await _context.Users.Include(x => x.Photos).Include(x => x.ResidenceCity).SingleOrDefaultAsync(
                user =>
                    user.PhoneNumber == phoneNumber && user.PasswordHash == passwordHash, cancellationToken);
        }

        public async Task<IReadOnlyCollection<User>> Get(CancellationToken cancellationToken)
        {
            return await _context.Users.Include(x => x.Photos).Include(x => x.ResidenceCity)
                .ToListAsync(cancellationToken);
        }

        public async Task<IReadOnlyCollection<User>> Get(int? skip, int? count, CancellationToken cancellationToken)
        {
            return skip.HasValue && count.HasValue
                ? await _context.Users.Skip(skip.Value).Take(count.Value).ToListAsync(cancellationToken)
                : await Get(cancellationToken);
        }

        public async Task<User> Get(long id, CancellationToken cancellationToken)
        {
            return await _context.Users.Include(x => x.Photos).Include(x => x.ResidenceCity)
                .SingleOrDefaultAsync(x => x.Id == id, cancellationToken);
        }

        public async Task Create(User user, CancellationToken cancellationToken)
        {
            await _context.Users.AddAsync(user, cancellationToken);
            await _context.SaveChangesAsync(cancellationToken);
        }

        public async Task Update(User user, CancellationToken cancellationToken)
        {
            _context.Users.Update(user);
            await _context.SaveChangesAsync(cancellationToken);
        }

        public async Task Remove(User user, CancellationToken cancellationToken)
        {
            _context.Users.Remove(user);
            await _context.SaveChangesAsync(cancellationToken);
        }

        public async Task Remove(long id, CancellationToken cancellationToken)
        {
            var user = await Get(id, cancellationToken);
            if (user != null)
            {
                _context.Users.Remove(user);
                await _context.SaveChangesAsync(cancellationToken);
            }
        }
    }
}