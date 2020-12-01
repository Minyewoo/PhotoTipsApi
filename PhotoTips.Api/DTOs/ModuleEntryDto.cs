using PhotoTips.Core.Models;

namespace PhotoTips.Api.DTOs
{
    public class ModuleEntryDto
    {
        public long Id { get; set; }

        public int IndexNumber { get; set; }

        public ModuleEntry.ModuleEntryType Type { get; set; }

        public string Name { get; set; }

        public string Description { get; set; }

        public string AdditionalInfo { get; set; }

        public virtual LectureContentDto[] VideoLecture { get; set; } = new LectureContentDto[0];

        public virtual LectureContentDto[] TextLecture { get; set; } = new LectureContentDto[0];
    }
}