using DatingApp.Application.Dtos;
using DatingApp.Application.Interfaces;
using DatingApp.Application.Interfaces.Users;
using Microsoft.AspNetCore.Mvc;
using System.Security.Cryptography;
using System.Text;

namespace DatingApp.WebApi.Controllers.Accounts
{

    public class AccountController : BaseApiController
    {
        private readonly IUserAppService _userAppService;
        private readonly ITokenService _tokenService;

        public AccountController(IUserAppService userAppService , ITokenService tokenService)
        {
            _userAppService = userAppService;
            _tokenService = tokenService;
        }
        [HttpPost("register")]
        public async Task<ActionResult<UserLoginDto>> Register(RegisterUserDto registerUserDto)
        {
            using var hmac = new HMACSHA512(); 
            var passwordHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(registerUserDto.Password));
            var passwordSalt = hmac.Key;

          var user =  await _userAppService.RegisterUser(registerUserDto, passwordHash, passwordSalt);
          return Ok(user);

        }

        [HttpPost("login")]
        public async Task<ActionResult<UserLoginDto>> Login(LoginDto loginDto)
        {
            var user =await _userAppService.LoginUser(loginDto);
            if (user is null) return Unauthorized("Invalid User");

            using var hmac = new HMACSHA512(user.PasswordSalt);
            var computedHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(loginDto.Password));
            for (int i = 0; i < computedHash.Length; i++)
            {
                if (computedHash[i] != user.PasswordHash[i])
                {
                    return Unauthorized("Invalid Password");
                }
            }
            UserLoginDto userLoginDto = new UserLoginDto
            {
                Id = user.Id,
                Name = user.Name,
                Token = _tokenService.CreateTokent(user.Name),
                PhotoUrl = user?.Photos?.FirstOrDefault(p=>p.IsMain)?.Url,
                Gender = user.Gender
            };
            return Ok(userLoginDto);
        }

    }
}
