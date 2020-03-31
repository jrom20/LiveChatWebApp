using RabbitMQ.Client;
using System;
using System.Collections.Generic;
using System.Text;

namespace BotStockEventBusService.Interfaces
{
    public interface IRabbitMQPersistentConnection : IDisposable
    {
        bool IsConnected { get; }
        IModel CreateModel();
    }
}
