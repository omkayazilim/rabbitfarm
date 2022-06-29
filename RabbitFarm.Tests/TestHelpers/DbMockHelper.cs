using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using Moq;
using MockQueryable.Moq;
using System.Threading;

namespace RabbitFarm.Tests.TestHelpers
{
    internal class DbMockHelper
    {

        internal static DbSet<T> CreateMockDbSet<T>(List<T> entity) where T : class 
        { 
          var dbset=entity.AsQueryable().BuildMockDbSet();
            dbset.Setup(x => x.Add(It.IsAny<T>())).Callback<T>(entity.Add);
           
            dbset.Setup(x => x.AddAsync(It.IsAny<T>(),It.IsAny<CancellationToken>()))
                .Callback<T,CancellationToken>((obj,token)=>entity.Add(obj));

            dbset.Setup(x => x.AddRange(It.IsAny<IEnumerable<T>>()))
             .Callback<IEnumerable<T>>(entity.AddRange);

            dbset.Setup(x => x.AddRangeAsync(It.IsAny<IEnumerable<T>>(),It.IsAny<CancellationToken>()))
            .Callback<IEnumerable<T>,CancellationToken>((obj,token)=>entity.AddRange(obj));
            return dbset.Object;
        }
    }
}
