using DatingApp.Application.Dtos;
using DatingApp.Application.Interfaces;
using Microsoft.AspNetCore.Mvc;
using System.Security.Cryptography;
using System.Text;

namespace DatingApp.WebApi.Controllers
{

    public class AccountController : BaseApiController
    {
        private readonly IUserAppService _userAppService;
        private readonly ITokenService _tokenService;

        public AccountController(IUserAppService userAppService, ITokenService tokenService)
        {
            _userAppService = userAppService;
            _tokenService = tokenService;
        }
        [HttpPost("register")]
        public async Task<ActionResult<UserLoginDto>> Register(RegisterUserDto registerUserDto)
        {
            var user = await _userAppService.RegisterUser(registerUserDto);
            return Ok(user);

        }

        [HttpPost("login")]
        public async Task<ActionResult<UserLoginDto>> Login(LoginDto loginDto)
        {
            var user = await _userAppService.LoginUser(loginDto);

            if (user is null) return Unauthorized("Invalid UserName or Password");

            return Ok(user);
        }

    }
}
