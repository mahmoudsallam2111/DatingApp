using DatingApp.Application.Dtos;
using DatingApp.Application.Helpers;
using DatingApp.Application.Interfaces;
using DatingApp.WebApi.Infrastracture.Extensions;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;

namespace DatingApp.WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LikesController : BaseApiController
    {
        private readonly ILikeAppService _likeAppService;

        public LikesController(ILikeAppService likeAppService)
        {
            _likeAppService = likeAppService;
        }

        [HttpPost("{likedUserName}")]
        public async Task<ActionResult> AddLike(string likedUserName)
        {
            var sourceUserId = User.GetUserId();
            await _likeAppService.AddLikeAsync(long.Parse(sourceUserId), likedUserName);
            return Ok();
            
        }

        [HttpGet]
        public async Task<ActionResult<PagesList<LikeDto>>> GetUserLike (string predicate)
        {
            var result = await _likeAppService.GetUserLikeAsync(predicate , long.Parse(User.GetUserId()));
            return Ok(result);
        }

    }
}
