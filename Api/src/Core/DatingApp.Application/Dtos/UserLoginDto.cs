namespace DatingApp.Application.Dtos
{
    public class UserLoginDto
    {
        public long Id { get; set; }
        public string Name { get; set; }
        public string Token { get; set; }
        public string PhotoUrl { get; set; }
        public string KnownAs { get; set; }
        public string Gender { get; set; }
    }
}
