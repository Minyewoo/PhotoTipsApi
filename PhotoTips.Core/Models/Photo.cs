namespace PhotoTips.Core.Models
{
    public class Photo
    {
        public string Id { get; set; }

        public string FileUrl { get; set; }

        public string ThumbnailUrl { get; set; }
        
        public virtual User Owner { get; set; }
    }
}