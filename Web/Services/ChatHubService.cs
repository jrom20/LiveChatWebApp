using ApplicationCore.Interfaces;
using Microsoft.AspNetCore.SignalR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Web.Hubs;
using Web.Interfaces;
using Web.ViewModels.Hub;

namespace Web.Services
{
    public class ChatHubService : IChatHubService
    {
        private readonly IHubContext<ChatHub> hubContext;

        public ChatHubService(IHubContext<ChatHub> hubContext)
        {
            this.hubContext = hubContext;
        }

        public async Task JoinPersonAsync(HubProfileViewModel profile)
        {
            await hubContext.Clients.Group(profile.Room).SendAsync("joinedPerson", profile.UserName);
            await hubContext.Groups.AddToGroupAsync(profile.ConnectionId, profile.Room);
        }

        public async Task RemovePersonAsync(HubProfileViewModel profile)
        {
            await hubContext.Groups.AddToGroupAsync(profile.ConnectionId, profile.Room);
            await hubContext.Clients.Group(profile.Room).SendAsync("removedPerson", profile.UserName);
        }

        public async Task SendMessageAsync(HubMessageViewModel messageRequest)
        {
            var channelName = string.Format($"channel-{messageRequest.ChatId}");
            messageRequest.ReceiveTime = DateTime.Now;
            await hubContext.Clients.Group(channelName).SendAsync("receivedMessage", messageRequest);
        }
    }
}
