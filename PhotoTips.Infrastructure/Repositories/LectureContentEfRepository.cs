using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using PhotoTips.Core.Models;
using PhotoTips.Core.Repositories;

namespace PhotoTips.Infrastructure.Repositories
{
    public class LectureContentEfRepository : ILectureContentRepository
    {
        private readonly PhotoTipsDbContext _context;

        public LectureContentEfRepository(PhotoTipsDbContext context)
        {
            _context = context;
        }

        public async Task<IReadOnlyCollection<LectureContent>> Get(CancellationToken cancellationToken)
        {
            return await _context.LectureContents.ToListAsync(cancellationToken: cancellationToken);
        }

        public async Task<IReadOnlyCollection<LectureContent>> Get(int? skip, int? count, CancellationToken cancellationToken)
        {
            return skip.HasValue && count.HasValue
                ? await _context.LectureContents.Skip(skip.Value).Take(count.Value).ToListAsync(cancellationToken)
                : await Get(cancellationToken);
        }

        public async Task<LectureContent> Get(long id, CancellationToken cancellationToken)
        {
            return await _context.LectureContents.FirstOrDefaultAsync(x => x.Id == id, cancellationToken);
        }

        public async Task Create(LectureContent lectureContent, CancellationToken cancellationToken)
        {
            await _context.LectureContents.AddAsync(lectureContent, cancellationToken);
            await _context.SaveChangesAsync(cancellationToken);
        }

        public async Task Update(LectureContent lectureContent, CancellationToken cancellationToken)
        {
            _context.LectureContents.Update(lectureContent);
            await _context.SaveChangesAsync(cancellationToken);
        }

        public async Task Remove(LectureContent lectureContent, CancellationToken cancellationToken)
        {
            _context.LectureContents.Remove(lectureContent);
            await _context.SaveChangesAsync(cancellationToken);
        }

        public async Task Remove(long id, CancellationToken cancellationToken)
        {
            var lectureContent = await Get(id, cancellationToken);
            if (lectureContent != null)
            {
                _context.LectureContents.Remove(lectureContent);
                await _context.SaveChangesAsync(cancellationToken);
            }
        }
    }
}