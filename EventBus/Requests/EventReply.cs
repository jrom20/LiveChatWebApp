using EventBus.Events;
using System;
using System.Collections.Generic;
using System.Text;

namespace EventBus.Requests
{
    public class EventReply : IntegrationEvent
    {
        public int ChannelId { get; set; }
        public string ConnectionId { get; set; }
        public string Message { get; set; }

        public EventReply(int channelId, string connectionId, string message)
        {
            ChannelId = channelId;
            connectionId = ConnectionId;
            Message = message;
        }
    }
}
