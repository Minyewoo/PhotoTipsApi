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

        [Column("video_lecture")] public List<LectureContent> VideoLecture { get; set; }

        [Column("video_lecture")] public List<LectureContent> TextLecture { get; set; }
    }
}
