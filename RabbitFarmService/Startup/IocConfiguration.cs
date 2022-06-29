
using RabbitFarm.Application;
using RabbitFarm.Domain.Interfaces;
using RabbitFarmInfrastructer.AppDbProviders;
using RabbitFarmInfrastructer.SqlIteProvider;

namespace RabbitFarmService
{
    public static class IocConfiguration
    {
        public static void RegisterAllDependencies(IServiceCollection services, IConfiguration config)
        {
            services.Scan(scan => {scan.FromApplicationDependencies(x=>x.GetName().Name.StartsWith("RabbitFarm"))
                .AddClasses(classess=>classess.AssignableTo<IScopedDependency>())
                .AsSelfWithInterfaces()
                .WithScopedLifetime()
                    .AddClasses(classes => classes.AssignableTo<ITansientDependency>())
                    .AsSelfWithInterfaces()
                    .WithTransientLifetime()
                .AddClasses(classes => classes.AssignableTo<ISingletonDependency>())
                    .AsSelfWithInterfaces()
                    .WithSingletonLifetime();

            });
        }




    }


}
