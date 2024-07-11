using DatingApp.Application.Dtos;
using DatingApp.Application.Helpers;
using DatingApp.Application.Interfaces;
using DatingApp.Application.Interfaces.Users;
using DatingApp.WebApi.Infrastracture.Extensions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace DatingApp.WebApi.Controllers.User
{
    [Authorize]
    public class UserController : BaseApiController
    {
        private readonly IUserAppService _userAppService;
        private readonly IPhotoAppService _photoAppService;

        public UserController(IUserAppService user , IPhotoAppService photoAppService)
        {
            _userAppService = user;
            _photoAppService = photoAppService;
        }

        [HttpGet("{id}")]
        //[Authorize]
        public async Task<ActionResult<GetUserDto>> GetUser(int id)
        {
            return  await _userAppService.GetUserById(id);
        }

        [HttpGet("GetByUserName/{userName}")]
        public async Task<ActionResult<GetUserDto>> GetUserByName(string userName)
        {
            return await _userAppService.GetUserByName(userName);
        }

        [HttpGet("getAllUers")]
        public async Task<ActionResult<PagesList<GetUserDto>>> GetUsers([FromQuery] UserParams userParams)
        {
            var userName = User.GetUserName();
            userParams.CurrentUser = userName;

            var users =  await _userAppService.GetUsers(userParams);
            Response.AddPaginationHeader(new PaginationHeader(users.CurrentPage,users.PageSize,users.TotalCount,users.TotalPages));
            return Ok(users);
        }

        [HttpPut]
        public async Task<ActionResult> UpdateUser(UserUpdateDto userUpdateDto)
        {
            await _userAppService.UpdateUser(userUpdateDto);

            return NoContent();
        }

        [HttpPost("add-photo")]
        public async Task<ActionResult<UserPhotoDto>> AddPhoto(IFormFile file)
        {
            var userName = User.GetUserName(); // to get the name if the user is authorized

            var result =  await _photoAppService.AddPhoto(file , userName);

            if(result.Error != null)  return BadRequest(result.Error);

            var userPhotoDto = new UserPhotoDto
            {
                Url = result.SecureUrl.AbsoluteUri,
                PublicId = result.PublicId,
            };
          
           return CreatedAtAction(nameof(GetUserByName), new {userName = userName},userPhotoDto);

        }

        [HttpPut("set-main-photo/{photoId}")]
        public async Task<ActionResult> SetMainPhoto(int photoId)
        {
            await _photoAppService.SetUserMainPhoto(photoId, User.GetUserName());
            return NoContent();
        }

        [HttpDelete("delete-photo/{photoId}")]
        public async Task<ActionResult> DeletePhoto(int photoId)
        {
            await _photoAppService.DeletePhotoAsync(photoId, User.GetUserName());
            return Ok();
        }
    }
}
