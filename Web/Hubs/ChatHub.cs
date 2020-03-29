using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SignalR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Web.Interfaces;
using Web.ViewModels.Hub;

namespace Web.Hubs
{
    [Authorize]
    public class ChatHub : Hub
    {
        private readonly IChatHubService _hubService;

        public ChatHub(IChatHubService hubService)
        {
            this._hubService = hubService;
        }

        public async Task<string> JoinPerson(string chatId)
        {
            var identity = (ClaimsIdentity)Context.User.Identity;
            if (identity.IsAuthenticated)
            {
                var profile = new HubProfileViewModel(Context.ConnectionId, identity.Name, chatId);
                await _hubService.JoinPersonAsync(profile);
            }
            else
                throw new ApplicationException("Action requires authentication.");

            return Context.ConnectionId;
        }

        public async Task SendMessage(HubMessageViewModel messageRequest)
        {
            var identity = (ClaimsIdentity)Context.User.Identity;
            messageRequest.UserName = identity.Name;
            messageRequest.MessageFrom = Context.ConnectionId;
            await _hubService.SendMessageAsync(messageRequest);
        }

        public override Task OnConnectedAsync()
        {
            return base.OnConnectedAsync();
        }
    }
}
