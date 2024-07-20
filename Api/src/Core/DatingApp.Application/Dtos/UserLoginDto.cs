﻿namespace DatingApp.Application.Dtos
{
    public class UserLoginDto
    {
        public int Id { get; set; }
        public string UserName { get; set; }
        public string Token { get; set; }
        public string PhotoUrl { get; set; }
        public string KnownAs { get; set; }
        public string Gender { get; set; }
    }
}
