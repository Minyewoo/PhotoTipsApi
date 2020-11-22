using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PhotoTipsApi.Models
{
    [Table("lecture_contents")]
    public class LectureContent
    {
        public enum ContentType
        {
            Video,
            Image,
            Text
        }

        [Key] [Column("id")] public string Id { get; set; }

        [Column("index_number")] public int IndexNumber { get; set; }

        [Column("type")] public ContentType Type { get; set; }
        [Column("content")] public string Content { get; set; }
    }
}