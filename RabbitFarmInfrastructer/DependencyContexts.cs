using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using RabbitFarmInfrastructer.AppDbProviders;
using RabbitFarmInfrastructer.SqlIteProvider;

namespace RabbitFarmInfrastructer
{
  public static  class DependencyContexts
    {
        public static IServiceCollection AddInfrastructures(this IServiceCollection services, IConfiguration config) 
        {
            services.AddSingleton<IAppDbContext,AppSqliteDbContext>();
            return services;
        
        }

        public static void SetSqlServerOptions(this DbContextOptionsBuilder builder, IConfiguration conf)
        {
            string connectionString = conf[$"ConnectionStrings:Default"];
            builder.UseSqlite(connectionString);

        }



    }
}
