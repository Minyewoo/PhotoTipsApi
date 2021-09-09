using PhotoTips.Core.Models;

namespace PhotoTips.Api.DTOs
{
    public class LectureContentDto
    {
        public long Id { get; set; }
        public int IndexNumber { get; set; }
        public LectureContent.ContentType Type { get; set; }
        public string Content { get; set; }

        public LectureContent ToEntity()
        {
            return new LectureContent
            {
                Id = Id,
                IndexNumber = IndexNumber,
                Type = Type,
                Content = Content,
            };
        }
    }
}