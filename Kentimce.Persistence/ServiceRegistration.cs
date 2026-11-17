using Kentimce.Application.Abstractions;
using Kentimce.Persistence.DataContext;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kentimce.Persistence
{
    public static class ServiceRegistration
    {
        
        public static void AddPersistenceServices(this IServiceCollection services, IConfiguration configuration)
        {
            
            var conn = configuration.GetConnectionString("DefaultConnection");

            services.AddDbContext<AppDbContext>(options =>
                options.UseNpgsql(conn, o => o.UseNetTopologySuite()));

           
            services.AddScoped<IAppDbContext>(provider =>
            {
                return provider.GetRequiredService<AppDbContext>();
            });

           
        }
    }
}