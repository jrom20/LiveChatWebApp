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

namespace WebApi
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
            services.AddIdentity<ApplicationUser, IdentityRole>(cfg =>
            {
                cfg.User.RequireUniqueEmail = true;
            }).AddEntityFrameworkStores<AppIdentityDbContext>();


            services.AddDbContext<AppIdentityDbContext>(cfg =>
            {
                cfg.UseSqlServer(_config.GetConnectionString("IdentityConnectionString"));
            });

            services.AddAuthentication()
                .AddCookie();

            //Dependency Injection Service
            services.AddTransient<AppIdentityDbContextSeed>();
            services.AddSwaggerGen(s => {
                s.SwaggerDoc("LibraryOpenAPISpecification", new Microsoft.OpenApi.Models.OpenApiInfo()
                {
                    Title = "Chat API",
                    Version = "1"
                });
            });

            services.AddControllers();
            services.AddCors(options =>
            {
                options.AddPolicy("AllowMyOrigin",

                builder =>
                {

                    builder
                        .AllowAnyHeader()
                        .AllowAnyMethod()
                        .AllowCredentials()
                        .Build();
                });
            });
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

            app.UseSwagger();
            app.UseSwaggerUI(setupAction => {
                setupAction.SwaggerEndpoint("/swagger/LibraryOpenAPISpecification/swagger.json", "Library API");
            });

            app.UseRouting();
            app.UseAuthentication();
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
