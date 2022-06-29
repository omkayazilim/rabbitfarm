using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;
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
        public AppSqliteDbContext(DbContextOptions<AppSqliteDbContext> options) : base(options) { }


        public DbSet<Animals> Animal { get; set; }
        public DbSet<AppDimensions> AppDimension { get; set; }
    
        public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = new CancellationToken())
        {
            return base.SaveChangesAsync();
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Animals>(b =>
            {
                b.HasKey(e => e.AnimalId);
                b.Property(e => e.AnimalId).ValueGeneratedOnAdd();
            });

            modelBuilder.Entity<AppDimensions>(b =>
            {
                b.HasKey(e => e.DimensionId);
                b.Property(e => e.DimensionId).ValueGeneratedOnAdd();
            });
        }
     }

    public class DesignTimeDbContextFactory : IDesignTimeDbContextFactory<AppSqliteDbContext>
    {
        public AppSqliteDbContext CreateDbContext(string[] args)
        {
            var optionsBuilder = new DbContextOptionsBuilder<AppSqliteDbContext>();
            var config = new ConfigurationBuilder()
                .AddJsonFile(Path.Combine(Directory.GetCurrentDirectory().Replace("RabbitFarmInfrastructer", "RabbitFarmService"), "appsettings.json"))
                .Build();
            var builder = new DbContextOptionsBuilder<AppSqliteDbContext>();
            string connectionString = config[$"ConnectionStrings:Default"];
            builder.UseSqlite(connectionString);
            return new AppSqliteDbContext();
        }
    }
}
