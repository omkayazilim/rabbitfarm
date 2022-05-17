
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;


namespace RabbitFarm.Domain.Entities
{
    [Table("T_AppDimensions")]
    public class AppDimensions:EntityBase
    {
        [Key]
        public long DimensionId { get; set; }
        [StringLength(20)]
        public string? AnimalType { get; set; }
        public int MaxDoPerBornCount { get; set; }
        public int MaxDoBornCount { get; set; }
        public int MaxLifeMinutes { get; set; }
        public TimeSpan Pregnancyduration { get; set; }
        public AppStatus AppStatus { get; set; }
        public TimeSpan AppLifeTime { get; set; }
        public int WaitMinutes { get; set; }


    }
}
