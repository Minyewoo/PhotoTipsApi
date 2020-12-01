using System;

namespace PhotoTips.Api.DTOs
{
    public class UserDto
    {
        public long Id { get; set; }

        public bool IsAdmin { get; set; }

        public string Name { get; set; }

        public string Surname { get; set; }

        public string Email { get; set; }

        public string PhoneNumber { get; set; }

        public DateTime RegistrationDate { get; set; }

        public CityDto ResidenceCity { get; set; }

    }
}