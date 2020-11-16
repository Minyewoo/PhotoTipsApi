using System;
using System.Collections.Generic;
using System.Linq;
using JetBrains.Annotations;
using PhotoTipsApi.Models;

namespace PhotoTipsApi.Repositories
{
    public class UserRepository
    {
        [CanBeNull]
        public User FindByEmail([NotNull] string email)
        {
            using var context = new PhotoTipsDbContext();
            return context.Users.SingleOrDefault(user => user.Email == email);
        }

        [CanBeNull]
        public User FindByEmailAndPassword([NotNull] string email, [NotNull] string passwordHash)
        {
            using var context = new PhotoTipsDbContext();
            return context.Users.SingleOrDefault(user => user.Email == email && user.PasswordHash == passwordHash);
        }

        [CanBeNull]
        public User FindByPhoneNumber([NotNull] string phoneNumber)
        {
            using var context = new PhotoTipsDbContext();
            return context.Users.SingleOrDefault(user => user.PhoneNumber == phoneNumber);
        }

        [CanBeNull]
        public User FindByPhoneNumberAndPassword(string phoneNumber, string passwordHash)
        {
            using var context = new PhotoTipsDbContext();
            return context.Users.SingleOrDefault(user =>
                user.PhoneNumber == phoneNumber && user.PasswordHash == passwordHash);
        }

        public List<User> Get()
        {
            using var context = new PhotoTipsDbContext();
            return context.Users.ToList();
        }

        [CanBeNull]
        public User Get([NotNull] string id)
        {
            using var context = new PhotoTipsDbContext();
            return context.Users.Find(id);
        }

        [NotNull]
        public User Create([NotNull] User user)
        {
            user.Id = Guid.NewGuid().ToString();
            using var context = new PhotoTipsDbContext();
            var createdUser = context.Users.Add(user);
            context.SaveChanges();
            return createdUser.Entity;
        }

        [CanBeNull]
        public User Update([NotNull] User user)
        {
            using var context = new PhotoTipsDbContext();

            var updatedUser = context.Users.Update(user);
            context.SaveChanges();
            return updatedUser?.Entity;
        }

        public void Remove([NotNull] User user)
        {
            using var context = new PhotoTipsDbContext();
            context.Users.Remove(user);
        }

        public void Remove([NotNull] string id)
        {
            using var context = new PhotoTipsDbContext();

            context.Users.Remove(context.Users.Find(id));
            context.SaveChanges();
        }
    }
}