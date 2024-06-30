using DatingApp.Application.Dtos;
using DatingApp.Application.Interfaces.Users;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace DatingApp.WebApi.Controllers.User
{
    public class UserController : BaseApiController
    {
        private readonly IUserAppService _user;

        public UserController(IUserAppService user)
        {
            _user = user;
        }

        [HttpGet("{id}")]
        //[Authorize]
        public async Task<ActionResult<GetUserDto>> GetUser(int id)
        {
            return  await _user.GetUserById(id);
        }

        [Authorize]
        [HttpGet("GetByUserName/{userName}")]
        public async Task<ActionResult<GetUserDto>> GetUserByName(string userName)
        {
            return await _user.GetUserByName(userName);
        }

        [HttpGet("getAllUers")]
        public async Task<ActionResult<IReadOnlyList<GetUserDto>>> GetUsers()
        {
            var users =  await _user.GetUsers();
            return Ok(users);
        }

        [HttpPut]
        public async Task<ActionResult> UpdateUser(UserUpdateDto userUpdateDto)
        {
            await _user.UpdateUser(userUpdateDto);

            return NoContent();
        }
    }
}
