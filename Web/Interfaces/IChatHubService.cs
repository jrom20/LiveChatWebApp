﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Web.ViewModels.Hub;

namespace Web.Interfaces
{
    public interface IChatHubService
    {
        Task SendMessageAsync(HubMessageViewModel messageRequest);
        Task JoinPersonAsync(HubProfileViewModel profile);
        Task RemovePersonAsync(HubProfileViewModel profile);
    }
}
