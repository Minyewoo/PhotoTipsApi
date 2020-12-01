using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PhotoTipsApi.Models
{
    public class Book
    {
        public string IsbnNumber { get; set; }
        
        public string Name { get; set; }
        
        public int WrittenYear { get; set; }
        
        public string[] Authors { get; set; }
    }
    
    
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