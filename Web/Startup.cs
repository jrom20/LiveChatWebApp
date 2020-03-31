using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Infrastructure.Identity;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using ApplicationCore.Helpers.Extensions;
using Infrastructure.Data;
using ApplicationCore.Interfaces;
using Web.Interfaces;
using Web.Services;
using ApplicationCore.Services;
using Web.Hubs;
using System.Reflection;
using AutoMapper;
using EventBusRabbitMQ.Interfaces;
using RabbitMQ.Client;
using EventBusRabbitMQ;
using EventBus.Interfaces;
using Web.Events;
using Microsoft.AspNetCore.SignalR;

namespace Web
{
    public class Startup
    {
        private readonly IConfiguration _config;

        public Startup(IConfiguration config)
        {
            _config = config;
        }

        // This method gets called by the runtime. Use this method to add services to the container.
        // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddSignalR();
            services.AddIdentity<ApplicationUser, IdentityRole>(cfg =>
            {
                cfg.User.RequireUniqueEmail = true;
            }).AddEntityFrameworkStores<AppIdentityDbContext>();


            services.AddDbContext<AppIdentityDbContext>(cfg =>
            {
                cfg.UseSqlServer(_config.GetConnectionString("IdentityConnectionString"));
            });

            services.AddDbContext<AppDataContext>(c =>
                c.UseSqlServer(_config.GetConnectionString("DataConnectionString")));

            services.AddAuthentication()
                .AddCookie();

            services.AddControllersWithViews();

            services.AddAutoMapper(Assembly.GetEntryAssembly());
            //Dependency Injection Service
            services.AddTransient<AppIdentityDbContextSeed>();
            services.AddTransient<AppDataContextSeed>();

            //Dependency Injection
            services.AddScoped(typeof(IAsyncRepository<>), typeof(EfRepository<>));
            services.AddScoped<IChatRepository, ChatRepository>();
            services.AddScoped<IChatService, ChatService>();
            services.AddScoped<IMessageService, MessageService>();
            services.AddScoped<IChatViewModelService, ChatViewModelService>();
            services.AddScoped<IChatHubService, ChatHubService>();

            services.AddSingleton<IRabbitMQPersistentConnection>(sp =>
            {
                var factory = new ConnectionFactory()
                {
                    HostName = "localhost",
                    DispatchConsumersAsync = true
                };

                return new DefaultRabbitMQPersistentConnection(factory);
            });


            services.AddScoped<IEventBus, StockQuoteEventsHandler>();
            services.AddSingleton<IEventBus, StockQuoteEventsHandler>(sp =>
            {
                var rabbitMQPersistentConnection = sp.GetRequiredService<IRabbitMQPersistentConnection>();
                var hubContext = sp.GetRequiredService<IHubContext<ChatHub>>();

                return new StockQuoteEventsHandler(rabbitMQPersistentConnection, hubContext);
            });

            ConfigureEventBus(services);

            services.AddSwaggerGen(s => {
                s.SwaggerDoc("LibraryOpenAPISpecification", new Microsoft.OpenApi.Models.OpenApiInfo()
                {
                    Title = "Chat API",
                    Version = "1"
                });
            });

            
            services.AddCors(options =>
            {
                options.AddPolicy("AllowMyOrigin",

                builder =>
                {
                    builder 
                        // Allow add subdomains localHost: 4200,http://*.ngrok.io
                        .SetIsOriginAllowedToAllowWildcardSubdomains()
                        //"CorsOrigins": "http://localhost:4200,http://*.ngrok.io" en AppSettings.json

                        //App:CorsOrigins in appsettings.json can contain more than one address with splitted by comma.
                        .WithOrigins(_config["App:CorsOrigins"]
                        .Split(",", StringSplitOptions.RemoveEmptyEntries).Select(o => o.RemovePostFix("/")).ToArray())
                        .AllowAnyHeader()
                        .AllowAnyHeader()
                        .AllowAnyMethod()
                        .AllowCredentials()
                        .Build();
                });
            });


        }

        private void ConfigureEventBus(IServiceCollection services)
        {
            var serviceProvider = services.BuildServiceProvider();
            var eventBus = serviceProvider.GetRequiredService<IEventBus>();
            eventBus.Subscribe();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            app.UseCors("AllowMyOrigin");

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseHttpsRedirection();
            app.UseDefaultFiles();
            app.UseStaticFiles();
            app.UseNodeModules();

            app.UseSwagger();
            app.UseSwaggerUI(setupAction => {
                setupAction.SwaggerEndpoint("/swagger/LibraryOpenAPISpecification/swagger.json", "Library API");
            });

            app.UseRouting();
            app.UseAuthentication();
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {

                endpoints.MapHub<ChatHub>("hubs/LiveChat");
                endpoints.MapControllerRoute("Default", "{controller}/{action}/{id?}", new { controller = "Account", Action = "Login" });
            });
        }
    }
}
