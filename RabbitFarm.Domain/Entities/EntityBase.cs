using RabbitFarm.Domain.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RabbitFarm.Domain.Entities
{
    public abstract class EntityBase : IEntityBase
    {
        public DateTime CreatedDate { get; set; }=DateTime.Now; 
        public string? CreatedUser { get; set; } = "SYS";
        public bool IsActive { get; set; } = true;
        public bool IsRemoved { get; set; } = false;
        public DateTime? UpdatetdDate { get; set; }
        public string? UpdatetdUser { get; set; }
    
    }
}
