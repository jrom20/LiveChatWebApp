using ApplicationCore.Interfaces;
using Microsoft.AspNetCore.SignalR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Web.Hubs;
using Web.Interfaces;
using Web.ViewModels.Hub;
using ApplicationCore.Constants;

namespace Web.Services
{
    public class ChatHubService : IChatHubService
    {
        private readonly IHubContext<ChatHub> _hubContext;

        public ChatHubService(IHubContext<ChatHub> hubContext)
        {
            this._hubContext = hubContext;
        }

        public async Task JoinPersonAsync(HubProfileViewModel profile)
        {
            await _hubContext.Clients.Group(profile.Room).SendAsync("joinedPerson", profile.UserName);
            await _hubContext.Groups.AddToGroupAsync(profile.ConnectionId, profile.Room);
        }

        public async Task RemovePersonAsync(HubProfileViewModel profile)
        {
            await _hubContext.Groups.AddToGroupAsync(profile.ConnectionId, profile.Room);
            await _hubContext.Clients.Group(profile.Room).SendAsync("removedPerson", profile.UserName);
        }

        public async Task SendMessageAsync(HubMessageViewModel messageRequest)
        {
            var channelName = string.Format($"channel-{messageRequest.ChatId}");
            messageRequest.ReceiveTime = DateTime.Now;
            await _hubContext.Clients.Group(channelName).SendAsync("receivedMessage", messageRequest);
        }

        public async Task SendSystemMessageByConnectionIdAsync(HubMessageViewModel messageRequest, string connectionId)
        {
            messageRequest.UserName = ChatConstants.BotName;
            await _hubContext.Clients.Client(connectionId).SendAsync("receivedMessage", messageRequest);
        }
    }
}
