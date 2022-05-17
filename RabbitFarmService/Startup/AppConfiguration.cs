using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RabbitFarmService
{
    public class AppConfiguration
    {

        public static IConfigurationRoot BuildConfiguration(string path,string envBaseName) {
            var builder = new ConfigurationBuilder().SetBasePath(path).AddJsonFile("appsettings.json", optional: true, reloadOnChange: true);

            switch (envBaseName) {
                case "Development": { builder.AddJsonFile($"appsettings.json", optional: true, reloadOnChange: true); break; }
                case "Production": { builder.AddJsonFile($"appsettings.prod.json", optional: true, reloadOnChange: true); break; }
                default: { builder.AddJsonFile($"appsettings.json", optional: true, reloadOnChange: true); break; }
            }

            return builder.Build();
        }
    }
}
