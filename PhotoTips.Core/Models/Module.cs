using System.Collections.Generic;

namespace PhotoTips.Core.Models
{
    public class Module
    {
        public long Id { get; set; }

        public int IndexNumber { get; set; }
        
        public string Name { get; set; }

        public string Description { get; set; }
        
        public virtual ICollection<ModuleEntry> Entries { get; set; }
    }
}