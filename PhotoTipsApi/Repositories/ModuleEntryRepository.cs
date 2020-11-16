using System;
using System.Collections.Generic;
using System.Linq;
using JetBrains.Annotations;
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
        public ModuleEntry Update([NotNull] ModuleEntry moduleEntry)
        {
            using var context = new PhotoTipsDbContext();

            var updatedModuleEntry = context.ModuleEntries.Update(moduleEntry);
            context.SaveChanges();

            return updatedModuleEntry?.Entity;
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