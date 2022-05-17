using RabbitFarm.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RabbitFarm.Domain.Dtos
{
    public class AppDimensionsInput
    {
        public int MaxDoPerBornCount { get; set; }
        public int MaxDoBornCount { get; set; }
        public int MaxLifeMinutes { get; set; }
        public int WaitMinutes { get; set; }
        public TimeSpan Pregnancyduration { get; set; }
        public TimeSpan AppLifeTime { get; set; }
  
    }

    public class AppDimensionsDto : AppDimensionsInput {
        public string AnimalType { get; set; }     
        public AppStatus AppStatus { get; set; }
    }
}
