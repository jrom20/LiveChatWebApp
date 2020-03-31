using System;
using System.Collections.Generic;
using System.Text;

namespace BotStockEventBusService.EventBus
{
    public class EventReply: IntegrationEvent
    {
        public int ChannelId { get; set; }
        public string ConnectionId { get; set; }
        public string Message { get; set; }
    }
}
