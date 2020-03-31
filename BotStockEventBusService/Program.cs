using BotStockEventBusService.EventBus;
using BotStockEventBusService.Interfaces;
using BotStockEventBusService.Services;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System;
using System.Text;

namespace BotStockEventBusService
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
            services.AddScoped<IEventBus, EventBusRabbitMQ>();
            services.AddSingleton<IEventBus, EventBusRabbitMQ>(sp =>
            {
                var rabbitMQPersistentConnection = sp.GetRequiredService<IRabbitMQPersistentConnection>();
                var stockService = sp.GetRequiredService<IStockService>();
                return new EventBusRabbitMQ(rabbitMQPersistentConnection, stockService);
            });

            var serviceProvider = services.BuildServiceProvider();
            var eventBus = serviceProvider.GetRequiredService<IEventBus>();

            eventBus.Subscribe();

            Console.WriteLine("Waiting for inputs");
            Console.ReadKey();

        }
    }
}
