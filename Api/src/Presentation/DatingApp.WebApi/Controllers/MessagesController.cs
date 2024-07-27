using DatingApp.Application.Dtos;
using DatingApp.Application.Helpers;
using DatingApp.Application.Interfaces;
using DatingApp.WebApi.Infrastracture.Extensions;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace DatingApp.WebApi.Controllers
{

    public class MessagesController : BaseApiController
    {
        private readonly IMessagesAppService _messagesAppService;

        public MessagesController(IMessagesAppService messagesAppService)
        {
            _messagesAppService = messagesAppService;
        }


        [HttpPost]
        public async Task<ActionResult<MessageDto>> CreateMessage(CreateMessageDto createMessageDto)
        {
            var UserName = User.GetUserName();
            if (createMessageDto.ReceiverName.ToLower() == UserName.ToLower()) { return BadRequest("You can send a message to yourself"); }

            var result = await _messagesAppService.CreateMessageAsync(createMessageDto, UserName);

            return Ok(result);

        }


        [HttpGet(nameof(GetMessagesForUser))]
        public async Task<ActionResult<MessageDto>> GetMessagesForUser([FromQuery] MessageParams messageParams)
        {
            messageParams.UserName = User.GetUserName();

            var result = await _messagesAppService.GetMessageForUser(messageParams);

            Response.AddPaginationHeader(new PaginationHeader(result.CurrentPage, result.PageSize, result.TotalCount, result.TotalPages));

            return Ok(result);
        }

        [HttpGet("GetMessageThead/{userName}")]
        public async Task<ActionResult<MessageDto>> GetMessageThead(string userName)
        {
            var currentUserName = User.GetUserName();

            var result = await _messagesAppService.GetMessageThead(currentUserName, userName);
           
            return Ok(result);
        }


    }
}
