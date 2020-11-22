using System;
using System.Collections.Generic;
using System.Linq;
using JetBrains.Annotations;
using PhotoTipsApi.Models;

namespace PhotoTipsApi.Repositories
{
    public class PhotoRepository
    {
        public List<Photo> Get()
        {
            using var context = new PhotoTipsDbContext();
            return context.Photos.ToList();
        }

        [CanBeNull]
        public Photo Get([NotNull] string id)
        {
            using var context = new PhotoTipsDbContext();
            return context.Photos.Find(id);
        }

        [CanBeNull]
        public Photo Create([NotNull] Photo photo)
        {
            using var context = new PhotoTipsDbContext();
            photo.Id = Guid.NewGuid().ToString();
            var createdPhoto = context.Photos.Add(photo);
            context.SaveChanges();
            return createdPhoto?.Entity;
        }

        [CanBeNull]
        public Photo Update([NotNull] Photo photo)
        {
            using var context = new PhotoTipsDbContext();

            var updatedPhoto = context.Photos.Update(photo);
            context.SaveChanges();

            return updatedPhoto?.Entity;
        }

        public void Remove([NotNull] Photo photo)
        {
            using var context = new PhotoTipsDbContext();
            context.Photos.Remove(photo);
        }

        public void Remove([NotNull] string id)
        {
            using var context = new PhotoTipsDbContext();
            context.Photos.Remove(context.Photos.Find(id));
        }
    }
}