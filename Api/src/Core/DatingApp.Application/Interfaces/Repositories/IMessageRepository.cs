using DatingApp.Application.Helpers;
using DatingApp.Domain.Aggregates.AppUser.Entities;
using DatingApp.Domain.Aggregates.Group.Entities;
using System.Text.RegularExpressions;

namespace DatingApp.Application.Interfaces.Repositories
{
    public interface IMessageRepository : IGenericRepository<Message>
    {
        Task<IEnumerable<Message>> GetMessageTheadAsync(string currentUserName, string receivedName);

        Task<PagesList<Message>> GetMessageForUserAsync(MessageParams messageParams);

        void AddGroup(Domain.Aggregates.Group.Entities.Group group);
        void RemoveConnection(Connection connection);

        Task<Connection> GetConnection(string connectionid);

        Task<Domain.Aggregates.Group.Entities.Group?> GetMessageGroup(string groupName);
        Task<Domain.Aggregates.Group.Entities.Group?> GetGroupForConnection(string connectionId);

    }
}
