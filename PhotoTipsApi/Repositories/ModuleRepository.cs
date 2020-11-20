using System;
using System.Collections.Generic;
using System.Linq;
using JetBrains.Annotations;
using Microsoft.EntityFrameworkCore;
using PhotoTipsApi.Models;
using Module = PhotoTipsApi.Models.Module;

namespace PhotoTipsApi.Repositories
{
    public class ModuleRepository
    {
        public List<Module> Get()
        {
            using var context = new PhotoTipsDbContext();
            return context.Modules.Include(module => module.Entries).ThenInclude(entry => entry.TextLecture)
                .Include(module => module.Entries).ThenInclude(entry => entry.VideoLecture).ToList();
        }

        [CanBeNull]
        public Module Get([NotNull] string id)
        {
            using var context = new PhotoTipsDbContext();
            return context.Modules.Include(module => module.Entries).ThenInclude(entry => entry.TextLecture)
                .Include(module => module.Entries).ThenInclude(entry => entry.VideoLecture).FirstOrDefault(x=>x.Id==id);
        }

        [CanBeNull]
        public Module Create([NotNull] Module module)
        {
            using var context = new PhotoTipsDbContext();
            module.Id = Guid.NewGuid().ToString();
            var createdModule = context.Modules.Add(module);
            context.SaveChanges();
            return createdModule.Entity;
        }

        [CanBeNull]
        public Module Update([NotNull] Module module)
        {
            using var context = new PhotoTipsDbContext();
            context.Modules.Update(module);
            context.SaveChanges();
            return context.Modules.Find(module.Id);
        }

        [CanBeNull]
        public Module AddModuleEntry([NotNull] string moduleId, [NotNull] string moduleEntryId)
        {
            using var context = new PhotoTipsDbContext();
            var updatableModule = context.Modules.FirstOrDefault(x => x.Id == moduleId);

            if (updatableModule.Entries != null)
                updatableModule.Entries.Add(context.ModuleEntries.FirstOrDefault(x => x.Id == moduleEntryId));
            else
                updatableModule.Entries = new List<ModuleEntry>
                    {context.ModuleEntries.FirstOrDefault(x => x.Id == moduleEntryId)};

            context.Modules.Update(updatableModule);
            context.SaveChanges();
            return updatableModule;
        }

        public void Remove([NotNull] Module module)
        {
            using var context = new PhotoTipsDbContext();
            context.Modules.Remove(module);
        }

        public void Remove([NotNull] string id)
        {
            using var context = new PhotoTipsDbContext();
            context.Modules.Remove(context.Modules.Find(id));
        }
    }
}