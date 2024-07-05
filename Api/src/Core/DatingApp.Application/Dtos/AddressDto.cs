using System.ComponentModel.DataAnnotations;

namespace DatingApp.Application.Dtos
{
    public class AddressDto
    {
        [Required]
        public string City { get; set; } 
        [Required]
        public string Country { get; set; } 
    }
}
