using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PhotoTipsApi.Models
{
    [Table("cities")]
    public class City
    {
        [Key] [Column("name")] public string Name { get; set; }

        public List<User> Users { get; set; }
    }
}