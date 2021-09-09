using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using PhotoTips.Core.Models;
using PhotoTips.Core.Repositories;

namespace PhotoTips.Infrastructure.Repositories
{
    public class ModuleEfRepository : IModuleRepository
    {
        private readonly PhotoTipsDbContext _context;

        public ModuleEfRepository(PhotoTipsDbContext context)
        {
            _context = context;
        }

        public async Task<IReadOnlyCollection<Module>> Get(CancellationToken cancellationToken)
        {
            return await _context.Modules.Include(module => module.Entries).ThenInclude(entry => entry.TextLecture)
                .Include(module => module.Entries).ThenInclude(entry => entry.VideoLecture)
                .ToListAsync(cancellationToken);
        }

        public async Task<IReadOnlyCollection<Module>> Get(int? skip, int? count, CancellationToken cancellationToken)
        {
            return skip.HasValue && count.HasValue
                ? await _context.Modules.Include(module => module.Entries).ThenInclude(entry => entry.TextLecture)
                    .Include(module => module.Entries).ThenInclude(entry => entry.VideoLecture).Skip(skip.Value).Take(count.Value).ToListAsync(cancellationToken)
                : await Get(cancellationToken);
        }

        public async Task<Module> Get(long id, CancellationToken cancellationToken)
        {
            return await _context.Modules.Include(module => module.Entries).ThenInclude(entry => entry.TextLecture)
                .Include(module => module.Entries).ThenInclude(entry => entry.VideoLecture)
                .FirstOrDefaultAsync(x => x.Id == id, cancellationToken);
        }

        public async Task Create(Module module, CancellationToken cancellationToken)
        {
            await _context.Modules.AddAsync(module, cancellationToken);
            await _context.SaveChangesAsync(cancellationToken);
        }

        public async Task Update(Module module, CancellationToken cancellationToken)
        {
            _context.Modules.Update(module);
            await _context.SaveChangesAsync(cancellationToken);
        }

        public async Task AddModuleEntry(long moduleId, long moduleEntryId, CancellationToken cancellationToken)
        {
            var updatableModule = await _context.Modules.FirstOrDefaultAsync(x => x.Id == moduleId, cancellationToken);

            if (updatableModule == null) return;

            if (updatableModule.Entries != null)
                updatableModule.Entries.Add(
                    await _context.ModuleEntries.FirstOrDefaultAsync(x => x.Id == moduleEntryId, cancellationToken));
            else
                updatableModule.Entries = new List<ModuleEntry>
                {
                    await _context.ModuleEntries.FirstOrDefaultAsync(x => x.Id == moduleEntryId, cancellationToken)
                };

            await Update(updatableModule, cancellationToken);
        }

        public async Task Remove(Module module, CancellationToken cancellationToken)
        {
            _context.Modules.Remove(module);
            await _context.SaveChangesAsync(cancellationToken);
        }

        public async Task Remove(long id, CancellationToken cancellationToken)
        {
            var module = await Get(id, cancellationToken);
            if (module != null)
            {
                _context.Modules.Remove(module);
                await _context.SaveChangesAsync(cancellationToken);
            }
        }
    }
}