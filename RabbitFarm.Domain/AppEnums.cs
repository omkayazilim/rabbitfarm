using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RabbitFarm.Domain
{
    public enum AnimalSexType { 
       Male=1,
       FeMale=2
    }

    public enum AnimalStatus
    {
        Baby = 1,
        Adult = 2,
        Pregnant=3,
        Pospartum =4,
        Death=5,



    }
    public enum AppStatus
    {
        Ready,
        Started,
        Paused,
        Stopped,
        Finish
    }

}
