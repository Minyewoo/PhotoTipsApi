using Microsoft.EntityFrameworkCore;
using PhotoTips.Core.Models;

namespace PhotoTips.Infrastructure.Repositories
{
    public class PhotoTipsDbContext : DbContext
    {
        public DbSet<User> Users { get; set; }
        public DbSet<City> Cities { get; set; }
        public DbSet<Module> Modules { get; set; }
        public DbSet<ModuleEntry> ModuleEntries { get; set; }
        public DbSet<LectureContent> LectureContents { get; set; }
        public DbSet<Photo> Photos { get; set; }
        public DbSet<Submission> Submissions { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseNpgsql(
                "Host=localhost;Port=5432;Database=photo_tips;Username=minyewoo;Password=wefunk2020");
        }
    }
}
