using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using PhotoTips.Core.Models;
using PhotoTips.Core.Repositories;

namespace PhotoTips.Infrastructure.Repositories
{
    public class ModuleEntryEfRepository : IModuleEntryRepository
    {
        private readonly PhotoTipsDbContext _context;

        public ModuleEntryEfRepository(PhotoTipsDbContext context)
        {
            _context = context;
        }

        public async Task<IReadOnlyCollection<ModuleEntry>> Get(CancellationToken cancellationToken)
        {
            return await _context.ModuleEntries.Include(x => x.TextLecture)
                .Include(x => x.VideoLecture).ToListAsync(cancellationToken);
        }

        public async Task<IReadOnlyCollection<ModuleEntry>> Get(int? skip, int? count, CancellationToken cancellationToken)
        {
            return skip.HasValue && count.HasValue
                ? await _context.ModuleEntries.Skip(skip.Value).Take(count.Value).ToListAsync(cancellationToken)
                : await Get(cancellationToken);
        }

        public async Task<ModuleEntry> Get(long id, CancellationToken cancellationToken)
        {
            return await _context.ModuleEntries.Include(x => x.TextLecture)
                .Include(x => x.VideoLecture)
                .SingleOrDefaultAsync(x => x.Id == id, cancellationToken: cancellationToken);
        }

        public async Task Create(ModuleEntry moduleEntry, CancellationToken cancellationToken)
        {
            await _context.ModuleEntries.AddAsync(moduleEntry, cancellationToken);
            await _context.SaveChangesAsync(cancellationToken);
        }

        public async Task Update(ModuleEntry moduleEntry, CancellationToken cancellationToken)
        {
            _context.ModuleEntries.Update(moduleEntry);
            await _context.SaveChangesAsync(cancellationToken);
        }

        public async Task AddToVideoLecture(long moduleEntryId, long lectureContentId,
            CancellationToken cancellationToken)
        {
            var updatableModuleEntry =
                await _context.ModuleEntries.FirstOrDefaultAsync(x => x.Id == moduleEntryId,
                    cancellationToken);

            if (updatableModuleEntry == null) return;

            if (updatableModuleEntry.VideoLecture != null)
                updatableModuleEntry.VideoLecture.Add(
                    await _context.LectureContents.FirstOrDefaultAsync(x => x.Id == lectureContentId,
                        cancellationToken));
            else
                updatableModuleEntry.VideoLecture = new List<LectureContent>
                {
                    await _context.LectureContents.FirstOrDefaultAsync(x => x.Id == lectureContentId,
                        cancellationToken)
                };

            await Update(updatableModuleEntry, cancellationToken);
        }

        public async Task AddToTextLecture(long moduleEntryId, long lectureContentId, CancellationToken cancellationToken)
        {
            var updatableModuleEntry =
                await _context.ModuleEntries.FirstOrDefaultAsync(x => x.Id == moduleEntryId,
                    cancellationToken);

            if (updatableModuleEntry == null) return;

            if (updatableModuleEntry.TextLecture != null)
                updatableModuleEntry.TextLecture.Add(
                    await _context.LectureContents.FirstOrDefaultAsync(x => x.Id == lectureContentId,
                        cancellationToken));
            else
                updatableModuleEntry.TextLecture = new List<LectureContent>
                {
                    await _context.LectureContents.FirstOrDefaultAsync(x => x.Id == lectureContentId,
                        cancellationToken)
                };

            await Update(updatableModuleEntry, cancellationToken);
        }

        public async Task Remove(ModuleEntry moduleEntry, CancellationToken cancellationToken)
        {
            _context.ModuleEntries.Remove(moduleEntry);
            await _context.SaveChangesAsync(cancellationToken);
        }

        public async Task Remove(long id, CancellationToken cancellationToken)
        {
            var moduleEntry = await Get(id, cancellationToken);
            if (moduleEntry != null)
            {
                _context.ModuleEntries.Remove(moduleEntry);
                await _context.SaveChangesAsync(cancellationToken);
            }
        }
    }
}