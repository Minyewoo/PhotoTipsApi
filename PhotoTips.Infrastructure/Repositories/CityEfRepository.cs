using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using PhotoTips.Core.Models;
using PhotoTips.Core.Repositories;

namespace PhotoTips.Infrastructure.Repositories
{
    public class CityEfRepository : ICityRepository
    {
        private readonly PhotoTipsDbContext _context;

        public CityEfRepository(PhotoTipsDbContext context)
        {
            _context = context;
        }

        public async Task<IReadOnlyCollection<City>> Get(CancellationToken cancellationToken)
        {
            return await _context.Cities.ToListAsync(cancellationToken);
        }

        public async Task<IReadOnlyCollection<City>> Get(int? skip, int? count, CancellationToken cancellationToken)
        {
            return skip.HasValue && count.HasValue
                ? await _context.Cities.Skip(skip.Value).Take(count.Value).ToListAsync(cancellationToken)
                : await Get(cancellationToken);
        }

        public async Task<City> Get(long id, CancellationToken cancellationToken)
        {
            return await _context.Cities.FirstOrDefaultAsync(x => x.Id == id, cancellationToken);
        }

        public async Task Create(City city, CancellationToken cancellationToken)
        {
            await _context.Cities.AddAsync(city, cancellationToken);
            await _context.SaveChangesAsync(cancellationToken);
        }

        public async Task Update(City city, CancellationToken cancellationToken)
        {
            _context.Cities.Update(city);
            await _context.SaveChangesAsync(cancellationToken);
        }

        public async Task Remove(City city, CancellationToken cancellationToken)
        {
            _context.Cities.Remove(city);
            await _context.SaveChangesAsync(cancellationToken);
        }

        public async Task Remove(long id, CancellationToken cancellationToken)
        {
            var city = await Get(id, cancellationToken);
            if (city != null)
            {
                _context.Cities.Remove(city);
                await _context.SaveChangesAsync(cancellationToken);
            }
        }
    }
}