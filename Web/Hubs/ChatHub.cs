using Microsoft.AspNetCore.SignalR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Web.Interfaces;
using Web.ViewModels.Hub;

namespace Web.Hubs
{
    public class ChatHub : Hub
    {
        private readonly IChatHubService hubService;

        public ChatHub(IChatHubService hubService)
        {
            this.hubService = hubService;
        }
        
        public async Task JoinPerson(string userName, string chatId)
        {
            var profile = new HubProfileViewModel(Context.ConnectionId, userName, chatId);
            await hubService.JoinPersonAsync(profile);
        }

        public async Task SendMessage(HubMessageViewModel messageRequest)
        {
            await hubService.SendMessageAsync(messageRequest);
        }

        public override Task OnConnectedAsync()
        {
            return base.OnConnectedAsync();
        }
    }
}
