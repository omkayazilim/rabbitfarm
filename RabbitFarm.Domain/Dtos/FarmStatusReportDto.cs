using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RabbitFarm.Domain.Dtos
{
    public class FarmStatusReportDto
    {
        public string AnimalTypeName { get; set; }
        public long AnimalCount { get; set; }
        public long MaleCount { get; set; }
        public long BabyAnimalCount { get; set; }
        public long FeMaleCount { get; set; }
        public long PregnantCount { get; set; }
        public long PostpartumCount { get; set; }
        public string SimulationStatus { get; set; }
    }
}
