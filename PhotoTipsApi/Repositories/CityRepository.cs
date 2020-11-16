using System;
using System.Collections.Generic;
using System.Linq;
using JetBrains.Annotations;
using PhotoTipsApi.Models;

namespace PhotoTipsApi.Repositories
{
    public class CityRepository
    {
        public List<City> Get()
        {
            using var context = new PhotoTipsDbContext();
            return context.Cities.ToList();
        }

        [CanBeNull]
        public City Get([NotNull] string id)
        {
            using var context = new PhotoTipsDbContext();
            return context.Cities.Find(id);
        }

        [CanBeNull]
        public City Create([NotNull] City city)
        {
            using var context = new PhotoTipsDbContext();

            var createdCity = context.Cities.Add(city);
            context.SaveChanges();
            return createdCity.Entity;
        }

        [CanBeNull]
        public City Update([NotNull] City city)
        {
            using var context = new PhotoTipsDbContext();

            var updatedCity = context.Cities.Update(city);
            context.SaveChanges();

            return updatedCity?.Entity;
        }

        public void Remove([NotNull] City city)
        {
            using var context = new PhotoTipsDbContext();
            context.Cities.Remove(city);
        }

        public void Remove([NotNull] string id)
        {
            using var context = new PhotoTipsDbContext();
            context.Cities.Remove(context.Cities.Find(id));
        }
    }
}