using BotStock.StandAlone.Interfaces;
using BotStock.StandAlone.Services;
using EventBus.Interfaces;
using EventBusRabbitMQ;
using EventBusRabbitMQ.Interfaces;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System;
using System.Text;

namespace BotStock.StandAlone
{
    class Program
    {
        static void Main(string[] args)
        {
            var configuration = new ConfigurationBuilder().AddJsonFile("appsettings.json").Build();
            var services = new ServiceCollection();

            services.AddSingleton<IRabbitMQPersistentConnection>(sp =>
            {
                var factory = new ConnectionFactory()
                {
                    HostName = "localhost",
                    DispatchConsumersAsync = true
                };

                return new DefaultRabbitMQPersistentConnection(factory);
            });

            services.AddSingleton<IConfiguration>(configuration);
            services.AddScoped<IStockService, StockService>();
            services.AddScoped<IEventBus, ListenerEventBusRabbitMQ>();
            services.AddSingleton<IEventBus, ListenerEventBusRabbitMQ>(sp =>
            {
                var rabbitMQPersistentConnection = sp.GetRequiredService<IRabbitMQPersistentConnection>();
                var stockService = sp.GetRequiredService<IStockService>();
                return new ListenerEventBusRabbitMQ(rabbitMQPersistentConnection, stockService);
            });

            var serviceProvider = services.BuildServiceProvider();
            var eventBus = serviceProvider.GetRequiredService<IEventBus>();

            eventBus.Subscribe();

            Console.WriteLine("Service has been started.");
            Console.WriteLine("Waiting for messages...");
            Console.ReadKey();

        }
    }
}
