using ApplicationCore.Interfaces;
using Microsoft.AspNetCore.SignalR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Web.Hubs;
using Web.Interfaces;
using Web.ViewModels.Hub;
using ApplicationCore.Helpers.Extensions;
using ApplicationCore.Constants;
using EventBus.Interfaces;

namespace Web.Services
{
    public class ChatHubService : IChatHubService
    {
        private readonly IHubContext<ChatHub> _hubContext;
        private readonly IChatService _chatService;
        private readonly IMessageService _messageService;
        private readonly IEventBus _eventBus;

        public ChatHubService(IHubContext<ChatHub> hubContext, IChatService chatService, IMessageService messageService, IEventBus eventBus)
        {
            _hubContext = hubContext;
            _chatService = chatService;
            _messageService = messageService;
            _eventBus = eventBus;
        }

        public async Task JoinPersonAsync(HubProfileViewModel profile)
        {
            await _chatService.AddPersonToChat(profile.UserName, profile.RoomId);

            await _hubContext.Clients.Group(profile.Room).SendAsync("joinedPerson", profile.UserName);
            await _hubContext.Groups.AddToGroupAsync(profile.ConnectionId, profile.Room);

            List<HubMessageViewModel> messagesList = new List<HubMessageViewModel>();
            var Messages = await _messageService.GetChatMessagesById(profile.RoomId);

            foreach (var msg in Messages.OrderBy(c=>c.Id))
            {
                messagesList.Add(new HubMessageViewModel
                {
                    ChatId = msg.ChatId,
                    Message = msg.TextMessage,
                    MessageFrom = msg.Person.IdentityGuid == profile.UserName ? profile.ConnectionId : string.Empty,
                    ReceiveTime = msg.CapturedDate,
                    UserName = msg.Person.IdentityGuid
                });
            }

            await _hubContext.Clients.Client(profile.ConnectionId).SendAsync("loadMessages", messagesList, profile.ConnectionId);
        }

        public async Task RemovePersonAsync(HubProfileViewModel profile)
        {
            await _hubContext.Groups.AddToGroupAsync(profile.ConnectionId, profile.Room);
            await _hubContext.Clients.Group(profile.Room).SendAsync("removedPerson", profile.UserName);
        }

        public async Task SendMessageAsync(HubMessageViewModel messageRequest)
        {
            messageRequest.ReceiveTime = DateTime.Now;

            if (messageRequest.Message.IsStockCommand(out string stockCode))
            {
                _eventBus.Publish(new EventBus.Requests.EventReply(messageRequest.ChatId, messageRequest.MessageFrom, messageRequest.Message));
            }
            else
            {
                var profile = new HubProfileViewModel(messageRequest.ChatId);
                await _hubContext.Clients.Group(profile.Room).SendAsync("receivedMessage", messageRequest);
                await _messageService.AddNewMessage(messageRequest.ChatId, messageRequest.UserName, messageRequest.Message, messageRequest.ReceiveTime.Value);
            }
        }

        public async Task SendSystemMessageByConnectionIdAsync(HubMessageViewModel messageRequest, string connectionId)
        {
            messageRequest.UserName = ChatConstants.BotName;
            await _hubContext.Clients.Client(connectionId).SendAsync("receivedMessage", messageRequest);
        }
    }
}
