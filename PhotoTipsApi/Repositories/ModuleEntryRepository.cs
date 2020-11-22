using System;
using System.Collections.Generic;
using System.Linq;
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
            return context.ModuleEntries.Include(x=>x.TextLecture).Include(x=>x.VideoLecture).ToList();
        }

        [CanBeNull]
        public ModuleEntry Get([NotNull] string id)
        {
            using var context = new PhotoTipsDbContext();
            return context.ModuleEntries.Include(x=>x.TextLecture).Include(x=>x.VideoLecture).SingleOrDefault(x=>x.Id==id);
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

        // [CanBeNull]
        // public async Task<ModuleEntry> Update([NotNull] ModuleEntry moduleEntry)
        // {
        //     using var context = new PhotoTipsDbContext();
        //
        //     context.Entry(await context.ModuleEntries.FirstOrDefaultAsync(x => x.Id == moduleEntry.Id)).CurrentValues.SetValues(moduleEntry);
        //     await context.SaveChangesAsync();
        //
        //     return await context.ModuleEntries.FindAsync(moduleEntry.Id);
        // }

        [CanBeNull]
        public ModuleEntry Update([NotNull] ModuleEntry moduleEntry)
        {
            using var context = new PhotoTipsDbContext();

            context.ModuleEntries.Update(moduleEntry);

            context.SaveChanges();

            return context.ModuleEntries.Find(moduleEntry.Id);
        }

        [CanBeNull]
        public ModuleEntry AddToVideoLecture([NotNull] string moduleEntryId, [NotNull] string lectureContentId)
        {
            using var context = new PhotoTipsDbContext();

            var updatableModuleEntry = context.ModuleEntries.FirstOrDefault(x => x.Id == moduleEntryId);

            if (updatableModuleEntry.VideoLecture != null)
                updatableModuleEntry.VideoLecture.Add(
                    context.LectureContents.FirstOrDefault(x => x.Id == lectureContentId));
            else
                updatableModuleEntry.VideoLecture = new List<LectureContent>
                    {context.LectureContents.FirstOrDefault(x => x.Id == lectureContentId)};

            context.SaveChanges();

            return updatableModuleEntry;
        }

        [CanBeNull]
        public ModuleEntry AddToTextLecture([NotNull] string moduleEntryId, [NotNull] string lectureContentId)
        {
            using var context = new PhotoTipsDbContext();

            var updatableModuleEntry = context.ModuleEntries.FirstOrDefault(x => x.Id == moduleEntryId);

            if (updatableModuleEntry.TextLecture != null)
                updatableModuleEntry.TextLecture.Add(context.LectureContents.FirstOrDefault(x => x.Id == lectureContentId));
            else
                updatableModuleEntry.TextLecture = new List<LectureContent>
                    {context.LectureContents.FirstOrDefault(x => x.Id == lectureContentId)};

            context.ModuleEntries.Update(updatableModuleEntry);
  
            context.SaveChanges();

            return updatableModuleEntry;
        }

        public void Remove([NotNull] ModuleEntry moduleEntry)
        {
            using var context = new PhotoTipsDbContext();
            context.ModuleEntries.Remove(moduleEntry);
            context.SaveChanges();
        }

        public void Remove([NotNull] string id)
        {
            using var context = new PhotoTipsDbContext();
            context.ModuleEntries.Remove(context.ModuleEntries.Find(id));
            context.SaveChanges();
        }
    }
}