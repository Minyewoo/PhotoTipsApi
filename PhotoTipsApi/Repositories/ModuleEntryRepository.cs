using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using JetBrains.Annotations;
using Microsoft.EntityFrameworkCore;
using PhotoTipsApi.Models;

namespace PhotoTipsApi.Repositories
{
    public class ModuleEntryRepository
    {
        public List<ModuleEntry> Get()
        {
            using var context = new PhotoTipsDbContext();
            return context.ModuleEntries.ToList();
        }

        [CanBeNull]
        public ModuleEntry Get([NotNull] string id)
        {
            using var context = new PhotoTipsDbContext();
            return context.ModuleEntries.Find(id);
        }

        [CanBeNull]
        public ModuleEntry Create([NotNull] ModuleEntry moduleEntry)
        {
            using var context = new PhotoTipsDbContext();
            moduleEntry.Id = Guid.NewGuid().ToString();
            var createdModuleEntry = context.ModuleEntries.Add(moduleEntry);
            context.SaveChanges();
            return createdModuleEntry.Entity;
        }

        [CanBeNull]
        public async Task<ModuleEntry> Update([NotNull] ModuleEntry moduleEntry)
        {
            using var context = new PhotoTipsDbContext();

            context.Entry(await context.ModuleEntries.FirstOrDefaultAsync(x => x.Id == moduleEntry.Id)).CurrentValues.SetValues(moduleEntry);
            await context.SaveChangesAsync();
    
            return await context.ModuleEntries.FindAsync(moduleEntry.Id);
        }

        public void Remove([NotNull] ModuleEntry moduleEntry)
        {
            using var context = new PhotoTipsDbContext();
            context.ModuleEntries.Remove(moduleEntry);
        }

        public void Remove([NotNull] string id)
        {
            using var context = new PhotoTipsDbContext();
            context.ModuleEntries.Remove(context.ModuleEntries.Find(id));
        }
    }
}
