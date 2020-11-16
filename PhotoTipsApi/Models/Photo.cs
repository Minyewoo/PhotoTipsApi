using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PhotoTipsApi.Models
{
    public class Photo
    {
        [Key] [Column("id")] public string Id { get; set; }

        [ForeignKey("owner_id")] public User Owner;

        [Column("name")] public string Name { get; set; }

        [Column("file_url")] public string FileUrl { get; set; }

        [Column("thumbnail_url")] public string ThumbnailUrl { get; set; }
    }
}