using Microsoft.EntityFrameworkCore;
using RabbitFarmInfrastructer;
using RabbitFarmInfrastructer.AppDbProviders;
using RabbitFarmInfrastructer.SqlIteProvider;
using RabbitFarmService;
using Serilog;

var builder = WebApplication.CreateBuilder(args);
builder.Host.UseSerilog((hostContext, services, configuration) => { configuration.WriteTo.Console(); });
// Add services to the container.
builder.Services.AddInfrastructures(builder.Configuration);
IocConfiguration.RegisterAllDependencies(builder.Services, builder.Configuration);
builder.Services.AddDbContext<AppSqliteDbContext>();

builder.Services.AddCors(options => {
    options.AddPolicy("localhost", bb => {
        bb.WithOrigins(
             builder.Configuration["App:CorsOrigins"]
            .Split(",", StringSplitOptions.RemoveEmptyEntries)
            )
        .SetIsOriginAllowedToAllowWildcardSubdomains()
        .AllowAnyHeader()
        .AllowAnyMethod()
        .AllowCredentials();

    });
});

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();

    // For sqlite 
       using (var scope = app.Services.CreateScope()) {
           var context = scope.ServiceProvider.GetService<AppSqliteDbContext>();
           if (context != null) await context.Database.MigrateAsync();
        }
}
//else 
//{
//    using (var scope = app.Services.CreateScope()) {
//        var context = scope.ServiceProvider.GetService<AppSqliteDbContext>();
//        if (context != null) await context.Database.MigrateAsync();
//    }
//}

app.UseAuthorization();
app.UseCors("localhost");
app.MapControllers();
app.Run();
