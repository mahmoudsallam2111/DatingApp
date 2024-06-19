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
        [Authorize]
        public async Task<ActionResult<GetUserDto>> GetUser(int id)
        {
            return  await _user.GetUserById(id);
        }
    }
}
