using Microsoft.EntityFrameworkCore;
using PhotoTipsApi.Models;

namespace PhotoTipsApi.Repositories
{
    public class PhotoTipsDbContext : DbContext
    {
        public DbSet<User> Users { get; set; }
        public DbSet<City> Cities { get; set; }
        public DbSet<Module> Modules { get; set; }
        public DbSet<ModuleEntry> ModuleEntries { get; set; }
        public DbSet<LectureContent> LectureContents { get; set; }
        public DbSet<Photo> Photos { get; set; }

        public PhotoTipsDbContext()
        {
            Database.EnsureCreated();
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseNpgsql(
                "Host=localhost:5000;Port=5432;Database=photo_tips;Username=minyewoo;Password=wefunk2020");
        }
    }
}