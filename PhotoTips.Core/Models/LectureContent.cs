namespace PhotoTips.Core.Models
{
    public class LectureContent
    {
        public enum ContentType
        {
            Video,
            Image,
            Text
        }

        public long Id { get; set; }
        public int IndexNumber { get; set; }
        public ContentType Type { get; set; }
        public string Content { get; set; }
    }
}