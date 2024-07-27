using DatingApp.Application.Helpers;
using DatingApp.Domain.Aggregates.AppUser.Entities;

namespace DatingApp.Application.Interfaces.Repositories
{
    public interface IMessageRepository : IGenericRepository<Message>
    {
        Task<IEnumerable<Message>> GetMessageTheadAsync(string currentUserName, string receivedName);

        Task<PagesList<Message>> GetMessageForUserAsync(MessageParams messageParams);

    }
}
