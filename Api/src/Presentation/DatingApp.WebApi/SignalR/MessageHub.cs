using AutoMapper;
using DatingApp.Application.Dtos;
using DatingApp.Application.Interfaces;
using DatingApp.Application.Interfaces.Repositories;
using DatingApp.Domain.Aggregates.AppUser.Entities;
using DatingApp.Domain.Aggregates.Group.Entities;
using DatingApp.WebApi.Infrastracture.Extensions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SignalR;
using System.Text.RegularExpressions;

namespace DatingApp.WebApi.SignalR
{
    [Authorize]
    public class MessageHub : Hub
    {
        private readonly IMessagesAppService _messagesAppService;
        private readonly IMessageRepository _messageRepository;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IUserRepository _userRepository;
        private readonly IMapper _mapper;
        private readonly IHubContext<PresenceHub> _presenceHubContext;

        public MessageHub(IMessagesAppService messagesAppService , 
            IMessageRepository messageRepository , 
            IUnitOfWork unitOfWork,
            IUserRepository userRepository,
            IMapper mapper,
            IHubContext<PresenceHub> presenceHubContext)
        {
            _messagesAppService = messagesAppService;
            _messageRepository = messageRepository;
            _unitOfWork = unitOfWork;
            _userRepository = userRepository;
            _mapper = mapper;
            _presenceHubContext = presenceHubContext;
        }

        public override async Task OnConnectedAsync()
        {
           var httpContext = Context.GetHttpContext();
           var otherUser = httpContext.Request.Query["user"];
           var groupName = GetGroupName(Context.User.GetUserName(), otherUser);
           await Groups.AddToGroupAsync(Context.ConnectionId, groupName);

           await AddToGroup(groupName);  // to presist the data

            var messages = await _messagesAppService.GetMessageThead(Context.User.GetUserName() ,otherUser);

            await Clients.Group(groupName).SendAsync("RecieveMessageThread" , messages);
        }

        public async Task SendMessage(CreateMessageDto createMessageDto)
        {
            var sender = await _userRepository.FindByUserName(Context.User.GetUserName());
            var reciver = await _userRepository.FindByUserName(createMessageDto.ReceiverName);

            if (createMessageDto.ReceiverName.ToLower() == sender.UserName.ToLower()) { throw new HubException("You can send a message to yourself"); }

            try
            {
                var message = new Message
                {
                    Sender = sender,
                    Receiver = reciver,
                    SenderName = sender.UserName,
                    ReceiverName = reciver.UserName,
                    Content = createMessageDto.Content
                };

                var groupName = GetGroupName(sender.UserName, createMessageDto.ReceiverName);

                var group = await _messageRepository.GetMessageGroup(groupName);

                if (group.Connections.Any(x=>x.UserName == createMessageDto.ReceiverName) )  // if the user in the same group 
                {
                    message.DateRead = DateTime.UtcNow;
                }
                else
                {
                    var connectins = await PresenceTracker.GetConnectionsForUser(reciver.UserName);
                    if(connectins is not null) // is it not in the message group , the user will recive a notification
                    {
                        await _presenceHubContext.Clients.Clients(connectins).SendAsync("NewMessageRecieved" , 
                            new { userName = sender.UserName , knownAs = sender.KnownAs });
                    }

                }

                await _messageRepository.AddAsync(message);

                if (await _unitOfWork.SaveChangesAsync())    // if successfully saved send the message to the group
                   await Clients.Group(groupName).SendAsync("NewMessage" , _mapper.Map<MessageDto>(message));

            }
            catch (Exception e)
            {

                throw;
            }

        }

        public override async Task OnDisconnectedAsync(Exception? exception)
        {
            await RemoveFromMessageGroup();
            await base.OnDisconnectedAsync(exception);
        }
          private string GetGroupName(string caller , string others)
          {
               var stringComparer = string.CompareOrdinal(caller, others) < 0;

                return stringComparer ? $"{caller}--{others}" : $"{others}--{caller}";
          }

        private async Task<bool> AddToGroup(string groupName)
        {
            var group = await _messageRepository.GetMessageGroup(groupName); 

            var connection = new Connection(Context.ConnectionId , Context.User.GetUserName());    
            
            if (group == null)
            {
               group = new Domain.Aggregates.Group.Entities.Group(groupName);
               _messageRepository.AddGroup(group);
            }

            group.Connections.Add(connection);

            return await _unitOfWork.SaveChangesAsync();

        }


        private async Task<bool> RemoveFromMessageGroup()
        {
            var connnection = await _messageRepository.GetConnection(Context.ConnectionId);
            _messageRepository.RemoveConnection(connnection);
            return await _unitOfWork.SaveChangesAsync();    
        }
    }
}
