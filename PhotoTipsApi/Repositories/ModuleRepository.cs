﻿using System;
using System.Collections.Generic;
using System.Linq;
using JetBrains.Annotations;
using PhotoTipsApi.Models;

namespace PhotoTipsApi.Repositories
{
    public class ModuleRepository
    {
        public List<Module> Get()
        {
            using var context = new PhotoTipsDbContext();
            return context.Modules.ToList();
        }

        [CanBeNull]
        public Module Get([NotNull] string id)
        {
            using var context = new PhotoTipsDbContext();
            return context.Modules.Find(id);
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

            var updatedModule = context.Modules.Update(module);
            context.SaveChanges();

            return updatedModule?.Entity;
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