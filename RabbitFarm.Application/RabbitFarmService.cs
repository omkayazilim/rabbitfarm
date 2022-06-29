using Microsoft.EntityFrameworkCore;
using RabbitFarm.Domain;
using RabbitFarm.Domain.Dtos;
using RabbitFarm.Domain.Entities;
using RabbitFarm.Domain.Interfaces;
using RabbitFarmInfrastructer.AppDbProviders;
using Serilog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Timers;

namespace RabbitFarm.Application
{
    public interface IRabbitFarmService: IScopedDependency
    {
        Task<Boolean> SetAppDinemsions(AppDimensionsInput? config);
        Task<AppDimensionsDto> GetAppDinemsions();
        Task StartRabbitFarmSimulation();
        Task<FarmStatusReportDto> StopRabbitFarmSimulation();
        Task<FarmStatusReportDto> GetRabbitFarmSimulationStatusReport();
    }

    public class RabbitFarmService : IRabbitFarmService
    {
        private  ILogger _logger;
        private readonly IAppDbContext _context;
        private readonly System.Timers.Timer _timer;
        private static string TimerToken="Stop";
        public RabbitFarmService(IAppDbContext context)
        {
            _context = context;
            _timer=  new System.Timers.Timer(10000);
            _logger= Log.ForContext<RabbitFarmService>();
        }

       

        public async Task<AppDimensionsDto> GetAppDinemsions()
        {
            var query = await _context.AppDimension.Select(x => new AppDimensionsDto
            {
                AppLifeTime = x.AppLifeTime,
                MaxDoBornCount = x.MaxDoBornCount,
                MaxDoPerBornCount = x.MaxDoPerBornCount,
                MaxLifeMinutes = x.MaxLifeMinutes,
                Pregnancyduration = x.Pregnancyduration,
                WaitMinutes = x.WaitMinutes,
                AnimalType = x.AnimalType??"",
                AppStatus = x.AppStatus,   

            }).ToListAsync();

            return query.FirstOrDefault() ?? new AppDimensionsDto()
            {
                AppLifeTime = new TimeSpan(1, 0, 0),
                MaxDoBornCount = 5,
                MaxDoPerBornCount = 15,
                MaxLifeMinutes = 30,
                WaitMinutes = 3,
                Pregnancyduration = new TimeSpan(0, 3, 0),
            }; ;

        }


        public async Task<bool> SetAppDinemsions(AppDimensionsInput? config)
        {
            config = config ?? new AppDimensionsDto()
            {
                AppLifeTime = new TimeSpan(1, 0, 0),
                MaxDoBornCount = 5,
                MaxDoPerBornCount = 15,
                MaxLifeMinutes = 30,
                WaitMinutes = 3,
                Pregnancyduration = new TimeSpan(0, 3, 0),
            };
            _context.AppDimension.RemoveRange(_context.AppDimension);
            var removeresult = await _context.SaveChangesAsync();
            _context.AppDimension.Add(new AppDimensions
            {
                AppLifeTime = config.AppLifeTime,
                AppStatus = Domain.AppStatus.Ready,
                MaxDoBornCount = config.MaxDoBornCount,
                MaxDoPerBornCount = config.MaxDoPerBornCount,
                MaxLifeMinutes = config.MaxLifeMinutes,
                Pregnancyduration = config.Pregnancyduration
            });

          await _context.SaveChangesAsync();
            return true ;
        }

        public  async Task StartRabbitFarmSimulation() 
        {
         
            TimerToken ="Start";
            _context.StartFarmSimulations();
            _logger.Warning($"Simulation Started");
            _timer.Elapsed +=  async (e,arg) => 
            {

              await  _context.DoInseminationAsync();
              await  _context.DoCheckBornAsync(); 
              await  _context.DoCheckAdultAsync();
              await  _context.DoCheckDieAsync();

                _logger.Warning($"{arg.SignalTime}");
                if (TimerToken.Equals("Stop")) 
                {
                    _timer.Close();
                    _timer.Dispose();
                }
            };  
            _timer.Start();
        }

        public async Task<FarmStatusReportDto> StopRabbitFarmSimulation()
        {
            _logger.Warning($"Simulation Stopped");
             TimerToken = "Stop";
            var cfg = await _context.AppDimension.ToListAsync();
            var config = cfg.FirstOrDefault();
            config.AppStatus = Domain.AppStatus.Stopped;
            _context.AppDimension.Update(config);
            await _context.SaveChangesAsync();

            return await GetRabbitFarmSimulationStatusReport();
        }

        public async Task<FarmStatusReportDto> GetRabbitFarmSimulationStatusReport()
        {

            AnimalStatus[] ansall = { AnimalStatus.Baby, AnimalStatus.Adult, AnimalStatus.Pregnant, AnimalStatus.Pospartum };
            AnimalStatus[] ansfemale = { AnimalStatus.Adult, AnimalStatus.Pregnant, AnimalStatus.Pospartum };
            AnimalStatus[] ansmale = { AnimalStatus.Adult};
            var dimesion = await _context.GetAppDinesionAsync();
            var FarmStatusReport = new FarmStatusReportDto
            {
                AnimalCount = await _context.Animal.Where(x => ansall.Contains(x.AnimalStatus)).CountAsync(),
                AnimalTypeName = dimesion.AnimalType??"",
                BabyAnimalCount = await _context.Animal.Where(x => x.AnimalStatus == AnimalStatus.Baby && x.AnimalStatus != AnimalStatus.Death).CountAsync(),
                FeMaleCount = await _context.Animal.Where(x => ansfemale.Contains(x.AnimalStatus) && x.AnimalSexType == AnimalSexType.FeMale).CountAsync(),
                MaleCount = await _context.Animal.Where(x => ansmale.Contains(x.AnimalStatus) && x.AnimalSexType == AnimalSexType.Male).CountAsync(),
                PregnantCount = await _context.Animal.Where(x => x.AnimalStatus == AnimalStatus.Pregnant).CountAsync(),
                PostpartumCount = await _context.Animal.Where(x => x.AnimalStatus == AnimalStatus.Pospartum).CountAsync(),
                SimulationStatus = dimesion.AppStatus.GetEnumDesc()
            };

            return FarmStatusReport;
        }
    }

}
