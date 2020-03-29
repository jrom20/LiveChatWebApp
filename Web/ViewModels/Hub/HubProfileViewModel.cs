using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Web.ViewModels.Hub
{
    public class HubProfileViewModel
    {
        public string ConnectionId { get; set; }
        public string UserName { get; set; }
        
        private string room;
        public string Room
        {
            get { return $"channel-{room}"; }
            set { room = value; }
        }
        public HubProfileViewModel(string connectionId, string userName, string room)
        {
            ConnectionId = connectionId;
            UserName = userName;
            Room = room;
        }
    }
}
