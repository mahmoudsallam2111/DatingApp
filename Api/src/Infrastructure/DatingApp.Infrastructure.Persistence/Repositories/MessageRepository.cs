using DatingApp.Application.Helpers;
using DatingApp.Application.Interfaces;
using DatingApp.Application.Interfaces.Repositories;
using DatingApp.Domain.Aggregates.AppUser.Entities;
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


            var unReadMessages = messages
                .Where(m=>m.ReceiverName == currentUserName)
                .ToList();

                unReadMessages?.ForEach(m => m.DateRead = DateTime.UtcNow);  // mark then as read  
                await _unitOfWork.SaveChangesAsync();

            return unReadMessages;
           
        }
    }
}
