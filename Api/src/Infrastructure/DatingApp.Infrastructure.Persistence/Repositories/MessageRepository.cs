using DatingApp.Application.Helpers;
using DatingApp.Application.Interfaces;
using DatingApp.Application.Interfaces.Repositories;
using DatingApp.Domain.Aggregates.AppUser.Entities;
using DatingApp.Domain.Aggregates.Group.Entities;
using DatingApp.Infrastructure.Persistence.Context;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace DatingApp.Infrastructure.Persistence.Repositories
{
    public class MessageRepository : GenericRepository<Message>, IMessageRepository
    {
        private readonly ApplicationDbContext _dbContext;
        private readonly IUnitOfWork _unitOfWork;

        public MessageRepository(ApplicationDbContext dbContext , IUnitOfWork unitOfWork) : base(dbContext)
        {
            _dbContext = dbContext;
            _unitOfWork = unitOfWork;
        }

        public async Task<PagesList<Message>> GetMessageForUserAsync(MessageParams messageParams)
        {
            var query = _dbContext.Messages
                            .AsNoTracking()
                            .OrderByDescending(m => m.DateSent)
                            .Include(m => m.Sender)
                               .ThenInclude(u=>u.Photos)
                            .Include(m=>m.Receiver)
                               .ThenInclude(u=>u.Photos)
                            .AsQueryable();

            query = messageParams.Container switch
            {
                "Inbox" => query.Where(m => m.ReceiverName == messageParams.UserName),
                "Outbox" => query.Where(m => m.SenderName == messageParams.UserName),
                _ => query.Where(m => m.ReceiverName == messageParams.UserName && m.DateRead == null)
            };

            return await PagesList<Message>.CreateAsync(query, messageParams.PageNumber, messageParams.PageSize);
        }
        public async Task<IEnumerable<Message>> GetMessageTheadAsync(string currentUserName, string receivedName)
        {
            Expression<Func<Message, bool>> predicate =
                m => m.ReceiverName == currentUserName &&
                m.SenderName == receivedName
                ||
                m.ReceiverName == receivedName
                && m.SenderName == currentUserName;


            var messages = await _dbContext.Messages
                            .OrderByDescending(m => m.DateSent)
                            .Include(m => m.Sender)
                               .ThenInclude(u => u.Photos)
                            .Include(m => m.Receiver)
                               .ThenInclude(u => u.Photos)
                            .Where(predicate)
                            .OrderByDescending(m=>m.DateSent)
                            .ToListAsync();


            //var unReadMessages = messages
            //    .Where(m=>m.ReceiverName == currentUserName)
            //    .ToList();

            if (messages.Count != 0)
            {
                messages.ForEach(x => x.DateRead = DateTime.UtcNow);
                await _unitOfWork.SaveChangesAsync();
            }

            return messages;
           
        }

        public void RemoveConnection(Connection connection)
        {
           _dbContext.Connections.Remove(connection);
        }
        public void AddGroup(Group group)
        {
            _dbContext.Groups.Add(group);   
        }

        public async Task<Connection> GetConnection(string connectionid)
        {
            return await _dbContext.Connections.FindAsync(connectionid);
        }

        public async Task<Group?> GetMessageGroup(string groupName)
        {
            return await _dbContext.Groups
                .Include(x => x.Connections)
                .FirstOrDefaultAsync(x => x.Name == groupName);
        }

        public async Task<Group?> GetGroupForConnection(string connectionId)
        {
            return await _dbContext.Groups
                .Include(x => x.Connections)
                .Where(g => g.Connections.Any(c=>c.ConnectionId==connectionId))
                .FirstOrDefaultAsync();
        }
    }
}
