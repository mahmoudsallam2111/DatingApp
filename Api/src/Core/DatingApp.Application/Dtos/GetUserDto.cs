using DatingApp.Domain.Aggregates.AppUser.Entities;
using DatingApp.Domain.Aggregates.AppUser.ValueObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DatingApp.Application.Dtos
{
    public class GetUserDto
    {
        public long Id { get; set; }
        public string Name { get; set; }
        public byte[] PasswordHash { get; set; }
        public byte[] PasswordSalt { get; set; }
        public int Age { get; set; }
        public string PhotoUrl { get; set; }
        public string KnownAs { get; set; }
        public DateTime LastActive { get; set; } 
        public string Gender { get; set; } 
        public string Introduction { get; set; } 
        public string LookingFor { get; set; }
        public string Interests { get; set; }
        public Address Address { get; set; }
        public List<UserPhotoDto> Photos { get; set; } 
    }
}
