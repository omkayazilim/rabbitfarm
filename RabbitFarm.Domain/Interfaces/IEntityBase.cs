using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RabbitFarm.Domain.Interfaces
{
    public interface IEntityBase
    {
        DateTime CreatedDate { get; set; }
        [StringLength(20)]
        string CreatedUser { get; set; }
        bool IsActive { get; set; }
        bool IsRemoved { get; set; }
        DateTime? UpdatetdDate { get; set; }
        [StringLength(20)]
        string? UpdatetdUser { get; set; }
    }
}
