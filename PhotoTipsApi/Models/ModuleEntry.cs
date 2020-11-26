using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;


namespace PhotoTipsApi.Models
{
    [Table("module_entries")]
    public class ModuleEntry
    {
        public enum ModuleEntryType
        {
            Lesson,
            Homework
        }

        [Key] [Column("id")] public string Id { get; set; }

        [Column("index_number")] public int IndexNumber { get; set; }

        [Column("type")] public ModuleEntryType Type { get; set; }

        [Column("name")] public string Name { get; set; }

        [Column("description")] public string Description { get; set; }
        
        [Column("additional_info")] public string AdditionalInfo { get; set; }

        [ForeignKey("video_lecture_id")]
        public virtual ICollection<LectureContent> VideoLecture { get; set; }

        [ForeignKey("text_lecture_id")]
        public virtual ICollection<LectureContent> TextLecture { get; set; }
    }
}
