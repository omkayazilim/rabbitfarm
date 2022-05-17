
using Microsoft.EntityFrameworkCore;
using RabbitFarm.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RabbitFarmInfrastructer.AppDbProviders
{
    public interface IAppDbContext
    {
        DbSet<Animals> Animal { get; set; }      
        DbSet<AppDimensions> AppDimension { get; set; }   
        Task<int> SaveChangesAsync(CancellationToken cancellationToken = new CancellationToken());
    }
}
