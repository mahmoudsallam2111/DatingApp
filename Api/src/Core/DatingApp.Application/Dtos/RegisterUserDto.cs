using System.ComponentModel.DataAnnotations;

namespace DatingApp.Application.Dtos
{
    public class RegisterUserDto
    {
        [Required]
        public string Name { get; set; }
        [Required]
        public string KnowAs { get; set; }
        [Required]
        public string Gender { get; set; }
        [Required]
        public DateOnly DateOfBirth { get; set; }
        [Required]
        public AddressDto Address { get; set; }

        [Required]
        [StringLength(8 , MinimumLength =4)]
        public string Password { get; set; }
    }
}
