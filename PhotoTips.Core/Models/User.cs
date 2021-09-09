using System;
using System.Collections.Generic;

namespace PhotoTips.Core.Models
{
    public class User
    {
        public long Id { get; set; }

        public bool IsAdmin { get; set; }

        public string Name { get; set; }

        public string Surname { get; set; }

        public string Email { get; set; }

        public string PhoneNumber { get; set; }

        public DateTime RegistrationDate { get; set; }

        public virtual City ResidenceCity { get; set; }

        public string PasswordHash { get; set; }

        public virtual ICollection<Photo> Photos { get; set; }
    }
}