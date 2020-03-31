using BotStock.StandAlone.Interfaces;
using EventBus.Interfaces;
using EventBus.Requests;
using EventBusRabbitMQ.Interfaces;
using Newtonsoft.Json;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace BotStock.StandAlone
{
    public class ListenerEventBusRabbitMQ : IEventBus, IDisposable
    {
        const string BROKER_NAME = "stockbot_bus";
        const string QUESTION_QUEUE = "chating.signalrhub.question";
        const string ANSWER_QUEUE = "chating.signalrhub.answer";

        private readonly IRabbitMQPersistentConnection _persistentConnection;
        private readonly IStockService _stockService;

        private IModel _consumerChannel;

        public ListenerEventBusRabbitMQ(IRabbitMQPersistentConnection persistentConnection, IStockService stockService)
        {
            _persistentConnection = persistentConnection;
            _consumerChannel = CreateConsumerChannel();
            _stockService = stockService;
        }

        private IModel CreateConsumerChannel()
        {
            var channel = _persistentConnection.CreateModel();

            channel.ExchangeDeclare(exchange: BROKER_NAME,
                                    type: "direct");

            channel.QueueDeclare(queue: QUESTION_QUEUE,
                                 durable: false,
                                 exclusive: false,
                                 autoDelete: false,
                                 arguments: null);

            return channel;
        }

        public void Publish(EventReply @event)
        {
            using (var newChannel = _persistentConnection.CreateModel())
            {
                newChannel.QueueDeclare(queue: ANSWER_QUEUE,
                                 durable: false,
                                 exclusive: false,
                                 autoDelete: false,
                                 arguments: null);

                newChannel.QueueBind(queue: ANSWER_QUEUE,
                                  exchange: BROKER_NAME,
                                  routingKey: ANSWER_QUEUE);

                newChannel.ExchangeDeclare(exchange: BROKER_NAME, type: "direct");

                var message = JsonConvert.SerializeObject(@event);
                var body = Encoding.UTF8.GetBytes(message);

                newChannel.BasicPublish(
                    exchange: BROKER_NAME,
                    routingKey: ANSWER_QUEUE,
                    basicProperties: null,
                    body: body);
            }
        }

        public void Dispose()
        {
            if (_consumerChannel != null)
            {
                _consumerChannel.Dispose();
            }
        }


        public void Subscribe()
        {
            if (_consumerChannel != null)
            {
                var consumer = new AsyncEventingBasicConsumer(_consumerChannel);
                consumer.Received += Consumer_Received;

                _consumerChannel.BasicConsume(
                    queue: QUESTION_QUEUE,
                    autoAck: true,
                    consumer: consumer);
            }
        }


        private async Task Consumer_Received(object sender, BasicDeliverEventArgs eventArgs)
        {
            var message = Encoding.UTF8.GetString(eventArgs.Body);
            var replyMessage = JsonConvert.DeserializeObject<EventReply>(message);
            try
            {
                string stockQuote = await _stockService.GetStockQuote(replyMessage.Message);
                replyMessage.Message = stockQuote;
                Publish(replyMessage);
            }
            catch (Exception ex)
            {
                throw;
            }
        }
    }
}
