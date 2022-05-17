


using RabbitFarm.Application;

namespace RabbitFarmService
{
    public static class IocConfiguration
    {
        public static void RegisterAllDependencies(IServiceCollection services, IConfiguration config)
        {
            services.AddScoped<IRabbitFarmBussiness, RabbitFarmBussiness>();
   
        }




    }


}
