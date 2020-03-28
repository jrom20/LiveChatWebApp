using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace Infrastructure.Identity
{
    public class AppIdentityDbContext : IdentityDbContext<ApplicationUser>
    {
        public AppIdentityDbContext(DbContextOptions<AppIdentityDbContext> options)
            : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
            // Customize the ASP.NET Identity model and override the defaults if needed.
            // For example, you can rename the ASP.NET Identity table names and more.
            // Add your customizations after calling base.OnModelCreating(builder);

            //dotnet ef migrations add InitialIdentityModel --context AppIdentityDbContext -p "C:\Users\helsi\Desktop\Jobsity\jrom20\Infrastructure\Infrastructure.csproj" -s "C:\Users\helsi\Desktop\Jobsity\jrom20\WebApi\WebApi.csproj" -o Identity/Migrations
        }
    }
}
