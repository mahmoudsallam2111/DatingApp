using DatingApp.Domain.Aggregates.AppUser.Entities;

namespace DatingApp.Application.Interfaces.Repositories
{
    public interface IMessageRepository : IGenericRepository<Message>
    {
        Task<IEnumerable<Message>> GetMessageThead(int senderId , int receivedId);
    }
}
