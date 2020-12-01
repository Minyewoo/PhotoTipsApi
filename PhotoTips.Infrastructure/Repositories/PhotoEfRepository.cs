using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using PhotoTips.Core.Models;
using PhotoTips.Core.Repositories;

namespace PhotoTips.Infrastructure.Repositories
{
    public class PhotoEfRepository : IPhotoRepository
    {
        private readonly PhotoTipsDbContext _context;

        public PhotoEfRepository(PhotoTipsDbContext context)
        {
            _context = context;
        }

        public async Task<IReadOnlyCollection<Photo>> Get(CancellationToken cancellationToken)
        {
            return await _context.Photos.ToListAsync(cancellationToken: cancellationToken);
        }

        public async Task<IReadOnlyCollection<Photo>> Get(int? skip, int? count, CancellationToken cancellationToken)
        {
            return skip.HasValue && count.HasValue
                ? await _context.Photos.Skip(skip.Value).Take(count.Value).ToListAsync(cancellationToken)
                : await Get(cancellationToken);
        }

        public async Task<Photo> Get(string id, CancellationToken cancellationToken)
        {
            return await _context.Photos.FirstOrDefaultAsync(x => x.Id == id, cancellationToken);
        }

        public async Task<Photo> Create(Photo photo, CancellationToken cancellationToken)
        {
            photo.Id = Guid.NewGuid().ToString();
            var createdPhoto = await _context.Photos.AddAsync(photo, cancellationToken);
            await _context.SaveChangesAsync(cancellationToken);
            return createdPhoto?.Entity;
        }

        public async Task Update(Photo photo, CancellationToken cancellationToken)
        {
            _context.Photos.Update(photo);
            await _context.SaveChangesAsync(cancellationToken);
        }

        public async Task Remove(Photo photo, CancellationToken cancellationToken)
        {
            _context.Photos.Remove(photo);
            await _context.SaveChangesAsync(cancellationToken);
        }

        public async Task Remove(string id, CancellationToken cancellationToken)
        {
            var photo = await Get(id, cancellationToken);
            if (photo != null)
            {
                _context.Photos.Remove(photo);
                await _context.SaveChangesAsync(cancellationToken);
            }
        }
    }
}