using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;


namespace PhotoTipsApi.Models
{
    [Table("modules")]
    public class Module
    {
        [Key] [Column("id")] public string Id { get; set; }

        [Column("index_number")] public int IndexNumber { get; set; }
        
        [Column("name")] public string Name { get; set; }

        [Column("description")] public string Description { get; set; }

        [ForeignKey("entry_id")]
        public virtual ICollection<ModuleEntry> Entries { get; set; }
    }
}