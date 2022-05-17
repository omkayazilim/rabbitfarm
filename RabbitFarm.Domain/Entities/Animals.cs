using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RabbitFarm.Domain.Entities
{
    [Table("T_Animals")]
    public class Animals:EntityBase
    {
        [Key]
        public long AnimalId { get; set; }  
        public AnimalSexType AnimalSexType { get; set; }

        [StringLength(20)]
        public string AnimalType { get; set; }
        public AnimalStatus AnimalStatus { get; set; }
        public DateTime? InseminationDate { get; set; }
        public int PregnantCount { get; set; }
    }
}
