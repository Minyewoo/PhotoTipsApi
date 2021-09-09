using PhotoTips.Core.Models;

namespace PhotoTips.Api.DTOs
{
    public class ModuleEntryListDto
    {
        public ModuleEntryListItemDto[] Items { get; set; } = new ModuleEntryListItemDto[0];

        public class ModuleEntryListItemDto
        {
            public long Id { get; set; }
            public int IndexNumber { get; set; }
            public ModuleEntry.ModuleEntryType Type { get; set; }
            public string Name { get; set; }
            public string Description { get; set; }
            public string AdditionalInfo { get; set; }

            public ModuleEntry ToEntity() => new ModuleEntry
            {
                Id = Id,
                IndexNumber = IndexNumber,
                Type = Type,
                Name = Name,
                Description = Description,
                AdditionalInfo = AdditionalInfo
            };
            
        }
    }
}