using DatingApp.Domain.Aggregates.AppUser.ValueObjects;
using System.ComponentModel.DataAnnotations;

namespace DatingApp.Application.Dtos
{
    public class UserUpdateDto
    {
        public int Id { get; set; }
        public string Introduction { get; set; }
        public string LookingFor { get; set; }
        public string Interests { get; set; }
        public string City { get; set; }
        public string Country { get; set; }
    }
}
