using AutoMapper;
using BuildingBlocks.Exceptions;
using DatingApp.Application.Dtos;
using DatingApp.Application.Helpers;
using DatingApp.Application.Interfaces;
using DatingApp.Application.Interfaces.Repositories;
using DatingApp.Domain.Aggregates.AppUser.Entities;

namespace DatingApp.Application.Services
{
    public class MessagesAppService : IMessagesAppService
    {
        private readonly IMessageRepository _messageRepository;
        private readonly IUserRepository _userRepository;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        public MessagesAppService(IMessageRepository messageRepository ,
            IUserRepository userRepository,
            IUnitOfWork unitOfWork,
            IMapper mapper)
        {
            _messageRepository = messageRepository;
            _userRepository = userRepository;
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }
        public async Task<MessageDto> CreateMessageAsync(CreateMessageDto createMessageDto , string SenderName)
        {
            Message message = await InitializeMessageObject(createMessageDto, SenderName);

            var messageCreated = await _messageRepository.AddAsync(message);

            await _unitOfWork.SaveChangesAsync();

            return _mapper.Map<MessageDto>(messageCreated);

        }

        private async Task<Message> InitializeMessageObject(CreateMessageDto createMessageDto, string SenderName)
        {
            var message = (Message)Activator.CreateInstance(typeof(Message));

            var sender = await _userRepository.FindByUserName(SenderName);
            var receiver = await _userRepository.FindByUserName(createMessageDto.ReceiverName);

            if (receiver == null) throw new NotFoundException("The user whom try to send a message is not exist");


            message.Sender = sender;
            message.SenderName = sender.UserName;
            message.Receiver = receiver;
            message.ReceiverName = receiver.UserName;
            message.Content = createMessageDto.Content;

            return message;
        }

        public Task DeleteMessageAsync()
        {
            throw new NotImplementedException();
        }

        public Task<List<MessageDto>> GetAllMessagesAsync()
        {
            throw new NotImplementedException();
        }

        public async Task<PagesList<MessageDto>> GetMessageForUser(MessageParams messageParams)
        {
            var messages = await _messageRepository.GetMessageForUserAsync(messageParams);
            return _mapper.Map<PagesList<MessageDto>>(messages);
        }

        public async Task<IEnumerable<MessageDto>> GetMessageThead(string currentUserName, string receivedName)
        {
            var messages = await _messageRepository.GetMessageTheadAsync(currentUserName, receivedName);

            return _mapper.Map<IEnumerable<MessageDto>>(messages);

        }
    }
}
