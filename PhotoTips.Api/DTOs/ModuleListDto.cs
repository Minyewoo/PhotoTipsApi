namespace PhotoTips.Api.DTOs
{
    public class ModuleListDto
    {
        public ModuleListItemDto[] Items { get; set; } = new ModuleListItemDto[0];
        public class ModuleListItemDto
        {
            public long Id { get; set; }

            public int IndexNumber { get; set; }
        
            public string Name { get; set; }

            public string Description { get; set; }
        }
    }
}