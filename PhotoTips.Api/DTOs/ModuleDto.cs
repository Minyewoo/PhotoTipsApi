namespace PhotoTips.Api.DTOs
{
    public class ModuleDto
    {
        public long Id { get; set; }

        public int IndexNumber { get; set; }
        
        public string Name { get; set; }

        public string Description { get; set; }
        
        public virtual ModuleEntryDto[] Entries { get; set; } = new ModuleEntryDto[0];
    }
}