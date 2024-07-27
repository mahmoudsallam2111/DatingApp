using DatingApp.Application.Dtos;
using DatingApp.Application.Helpers;
using DatingApp.Domain.Aggregates.AppUser.Entities;

namespace DatingApp.Application.Interfaces
{
    public interface IMessagesAppService
    {
        Task<MessageDto> CreateMessageAsync(CreateMessageDto createMessageDto , string SenderName);
        Task DeleteMessageAsync();

        Task<PagesList<MessageDto>> GetMessageForUser(MessageParams messageParams);
        Task<List<MessageDto>> GetAllMessagesAsync();

        Task<IEnumerable<MessageDto>> GetMessageThead(string currentUserName, string receivedName);

    }
}
