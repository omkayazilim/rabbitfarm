using Microsoft.AspNetCore.Mvc;
using RabbitFarm.Application;
using RabbitFarm.Domain.Dtos;
using RabbitFarm.Domain.Entities;

namespace RabbitFarmService.Controllers
{
    public class RabbitFarmController:ControllerBase
    {
        private readonly IRabbitFarmBussiness _farmBussiness;
        public RabbitFarmController(IRabbitFarmBussiness farmBussiness) {
          _farmBussiness = farmBussiness;   

        }

        [HttpPost("SetAppDinemsions")]
        public Task<Boolean> SetAppDinemsions([FromBody] AppDimensionsInput? config)
        {
            return _farmBussiness.SetAppDinemsions(config);
        }

        [HttpGet("GetAppDinemsions")]
        public async Task<AppDimensionsInput> GetAppDinemsions()
        {
            return await _farmBussiness.GetAppDinemsions();
        }


        [HttpGet("HealthCheck")]
        public Task<Boolean> HealthCheck() 
        {
            return Task.FromResult(true);
        }

       

        [HttpGet("StartRabbitFarmSimulation")]
        public Task StartRabbitFarmSimulation()
        {
            return _farmBussiness.StartRabbitFarmSimulation();
        }

        [HttpGet("StopRabbitFarmSimulation")]
        public Task<FarmStatusReportDto> StopRabbitFarmSimulation()
        {
            return _farmBussiness.StopRabbitFarmSimulation();
        }

        [HttpGet("GetRabbitFarmSimulationStatusReport")]
        public Task<FarmStatusReportDto> GetRabbitFarmSimulationStatusReport()
        {
            return _farmBussiness.GetRabbitFarmSimulationStatusReport();
        }
    }
}
