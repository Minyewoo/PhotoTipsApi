using System;
using System.Collections.Generic;
using System.Linq;
using JetBrains.Annotations;
using Microsoft.EntityFrameworkCore;
using PhotoTipsApi.Models;

namespace PhotoTipsApi.Repositories
{
    public class LectureContentRepository
    {
        public List<LectureContent> Get()
        {
            using var context = new PhotoTipsDbContext();
            return context.LectureContents.ToList();
        }

        [CanBeNull]
        public LectureContent Get([NotNull] string id)
        {
            using var context = new PhotoTipsDbContext();
            return context.LectureContents.Find(id);
        }

        [CanBeNull]
        public LectureContent Create([NotNull] LectureContent lectureContent)
        {
            using var context = new PhotoTipsDbContext();
            lectureContent.Id = Guid.NewGuid().ToString();
            var createdLectureContent = context.LectureContents.Add(lectureContent);
            context.SaveChanges();
            return createdLectureContent.Entity;
        }

        [CanBeNull]
        public LectureContent Update([NotNull] LectureContent lectureContent)
        {
            using var context = new PhotoTipsDbContext();

            var updatedLectureContent = context.LectureContents.Update(lectureContent);
            context.SaveChanges();

            return updatedLectureContent?.Entity;
        }

        public void Remove([NotNull] LectureContent lectureContent)
        {
            using var context = new PhotoTipsDbContext();
            context.LectureContents.Remove(lectureContent);
            context.SaveChanges();
        }

        public void Remove([NotNull] string id)
        {
            using var context = new PhotoTipsDbContext();
            context.LectureContents.Remove(context.LectureContents.Find(id));
            context.SaveChanges();
        }
    }
}