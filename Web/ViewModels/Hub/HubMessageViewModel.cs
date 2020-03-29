using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Web.ViewModels.Hub
{
    public class HubMessageViewModel
    {
        public string Message { get; set; }
        public string MessageFrom { get; set; }
        public DateTime? ReceiveTime { get; set; }
        public int ChatId { get; set; }
        public string UserName { get; set; }

    }
}
