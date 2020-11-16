using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PhotoTipsApi.Models
{
    [Table("users")]
    public class User

    {
        [Key] [Column("id")] public string Id { get; set; }

        [Column("is_admin")] public bool IsAdmin { get; set; }

        [Column("name")] public string Name { get; set; }

        [Column("surname")] public string Surname { get; set; }

        [Column("email")] public string Email { get; set; }

        [Column("phone_number")] public string PhoneNumber { get; set; }

        [Column("registration_date")] public DateTime RegistrationDate { get; set; }

        [ForeignKey("residence_city_id")] public City ResidenceCity { get; set; }

        [Column("password_hash")] public string PasswordHash { get; set; }

        public List<Photo> Photos { get; set; }
    }
}