using System.ComponentModel.DataAnnotations;

namespace DatingApp.Application.Dtos
{
    public class RegisterUserDto
    {
        [Required]
        public string UserName { get; set; }
        [Required]
        public string Password { get; set; }
    }
}
