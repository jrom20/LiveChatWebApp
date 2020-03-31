using BotStockEventBusService.EventBus;
using System;
using System.Collections.Generic;
using System.Text;

namespace BotStockEventBusService.Interfaces
{
    public interface IEventBus
    {
        void PublishStock(EventReply @event);
        void Subscribe();

    }
}
