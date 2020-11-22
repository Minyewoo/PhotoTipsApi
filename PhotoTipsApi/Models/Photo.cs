using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PhotoTipsApi.Models
{
    [Table("photo")]
    public class Photo
    {
        [Key] [Column("id")] public string Id { get; set; }

        [Column("file_url")] public string FileUrl { get; set; }

        [Column("thumbnail_url")] public string ThumbnailUrl { get; set; }
    }
}