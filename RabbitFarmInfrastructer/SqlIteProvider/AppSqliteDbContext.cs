using Microsoft.EntityFrameworkCore;
using RabbitFarm.Domain.Entities;
using RabbitFarmInfrastructer.AppDbProviders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RabbitFarmInfrastructer.SqlIteProvider
{
    public class AppSqliteDbContext : DbContext, IAppDbContext
    {

        public string DbPath { get; }
        public DbSet<Animals> Animal { get; set; }
        public DbSet<AppDimensions> AppDimension { get; set; }
    

        public AppSqliteDbContext() {
            var folder = Environment.SpecialFolder.LocalApplicationData;
            var path = Environment.GetFolderPath(folder);
            DbPath = "AnimalFarm.db";
        }

        public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = new CancellationToken())
        {
            return base.SaveChangesAsync();
        }

        protected override void OnConfiguring(DbContextOptionsBuilder options)
       => options.UseSqlite($"Data Source={DbPath}");
    }
}
