using System.ComponentModel.DataAnnotations;

namespace DatingApp.Application.Dtos
{
    public class RegisterUserDto
    {
        [Required]
        public string Name { get; set; }
        [Required]
        public string Password { get; set; }
    }
}
