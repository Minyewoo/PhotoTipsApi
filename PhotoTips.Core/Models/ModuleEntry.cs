using System.Collections.Generic;

namespace PhotoTips.Core.Models
{
    public class ModuleEntry
    {
        public enum ModuleEntryType
        {
            Lesson,
            Homework
        }

        public long Id { get; set; }

        public int IndexNumber { get; set; }

        public ModuleEntryType Type { get; set; }

        public string Name { get; set; }

        public string Description { get; set; }

        public string AdditionalInfo { get; set; }

        public virtual ICollection<LectureContent> VideoLecture { get; set; }

        public virtual ICollection<LectureContent> TextLecture { get; set; }
    }
}