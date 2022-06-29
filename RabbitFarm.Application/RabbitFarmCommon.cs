using Microsoft.EntityFrameworkCore;
using RabbitFarm.Domain;
using RabbitFarm.Domain.Entities;
using RabbitFarmInfrastructer.AppDbProviders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Timers;

namespace RabbitFarm.Application
{
    public static class RabbitFarmCommon
    {
        public async static void StartFarmSimulations(this IAppDbContext ctx)
        {
            var cfg = await ctx.GetAppDinesionAsync();
            ctx.Animal.RemoveRange(await ctx.Animal.AsNoTracking().ToListAsync());
            await ctx.SaveChangesAsync();
            await ctx.Animal.AddRangeAsync(new List<Animals>()
            {
                new Animals { 
                    AnimalSexType=AnimalSexType.Male , 
                    AnimalType=cfg.AnimalType?? "",  
                    AnimalStatus= AnimalStatus.Adult
                },
                new Animals { AnimalSexType=AnimalSexType.FeMale ,AnimalType=cfg.AnimalType??"",  AnimalStatus= AnimalStatus.Adult}
            });

            await ctx.SaveChangesAsync();

            cfg.AppStatus = Domain.AppStatus.Started;
            ctx.AppDimension.RemoveRange(ctx.AppDimension.ToList());
            await ctx.SaveChangesAsync();
            cfg.DimensionId = 0;
            ctx.AppDimension.Add(cfg);
            await ctx.SaveChangesAsync();


        }

      
        public static string GetEnumDesc(this AppStatus tp)
        {

            switch (tp)
            {
                case AppStatus.Started: return "Başlatıldı";
                case AppStatus.Stopped: return "Durduruldu";
                default: return "";
            }

        }

        public static string GetEnumDesc(this AnimalSexType tp)
        {

            switch (tp)
            {
                case AnimalSexType.Male: return "Erkek";
                case AnimalSexType.FeMale: return "Dişi";
                default: return "";
            }

        }

        public static string GetEnumDesc(this AnimalStatus tp)
        {
            switch (tp)
            {
                case AnimalStatus.Adult: return "Yetişkin";
                case AnimalStatus.Pospartum: return "Lohusa";
                case AnimalStatus.Death: return "Olü";
                case AnimalStatus.Baby: return "Yavru";
                case AnimalStatus.Pregnant: return "Gebe";
                default: return "";
            }

        }

        public async static Task DoInseminationAsync(this IAppDbContext ctx)
        {
            var cfg = await ctx.GetAppDinesionAsync();
            var femaleAnimals = await ctx.Animal.Where(x => x.AnimalSexType == AnimalSexType.FeMale && x.AnimalStatus == AnimalStatus.Adult && x.PregnantCount < cfg.MaxDoBornCount).ToListAsync();
            bool hasmaleAnimal = await ctx.Animal.Where(x => x.AnimalSexType == AnimalSexType.FeMale && x.AnimalStatus == AnimalStatus.Adult).AnyAsync();
            if (!hasmaleAnimal)
                return;

            foreach (var femaleAnimal in femaleAnimals)
            {
                femaleAnimal.AnimalStatus = AnimalStatus.Pregnant;
                femaleAnimal.InseminationDate = DateTime.Now;
                femaleAnimal.PregnantCount++;
                ctx.Animal.Update(femaleAnimal);
                await ctx.SaveChangesAsync();
            }

        }

        public async static Task DoCheckBornAsync(this IAppDbContext ctx)
        {
            var PregnantAnimals = await ctx.Animal.Where(x => x.AnimalSexType == AnimalSexType.FeMale && x.AnimalStatus == AnimalStatus.Pregnant).ToListAsync();
            var cfg = await ctx.GetAppDinesionAsync();
            foreach (var f in PregnantAnimals)
            {
                var calcpregnant = (DateTime.Now - f.InseminationDate)?.TotalMinutes;
                if (calcpregnant >= cfg?.Pregnancyduration.TotalMinutes)
                {
                    f.AnimalStatus = AnimalStatus.Pospartum;
                    f.InseminationDate = null;
                    ctx.Animal.Update(f);
                    await ctx.SaveChangesAsync();
                    ChildCreateAsync(ctx);
                }



            }
        }

        public async static Task ChildCreateAsync(IAppDbContext ctx)
        {
            var cfg = await ctx.GetAppDinesionAsync();
            var babycount = new Random().Next(1, cfg.MaxDoPerBornCount);
            AnimalSexType[] childUnix = { AnimalSexType.Male, AnimalSexType.FeMale };
            var newAnimals = new List<Animals>();
            for (int i = 0; i < babycount; i++)
            {
                AnimalSexType coiceunix = childUnix[new Random().Next(1, 2)];
                newAnimals.Add( new Animals()
                {
                    AnimalSexType = coiceunix,
                    AnimalStatus = AnimalStatus.Baby,
                    AnimalType = cfg.AnimalType??"",
                    PregnantCount = 0,

                });

            }
            ctx.Animal.AddRange(newAnimals);
            await ctx.SaveChangesAsync();

        }

        public async static Task DoCheckAdultAsync(this IAppDbContext ctx)
        {
            var cfg = await ctx.GetAppDinesionAsync();
            var BabyAnimals = await ctx.Animal.Where(x =>  x.AnimalStatus == AnimalStatus.Baby).ToListAsync();
            foreach (var baby in BabyAnimals) 
            {
                var babyage = (DateTime.Now - baby.CreatedDate).TotalMinutes;
                if (cfg.WaitMinutes >= babyage) { 
                   baby.AnimalStatus = AnimalStatus.Adult;
                    ctx.Animal.Update(baby);
                   await ctx.SaveChangesAsync();
                }
               
            }
        }

        public async static Task DoCheckDieAsync(this IAppDbContext ctx)
        {
            var cfg = await ctx.GetAppDinesionAsync();
            var AllAnimals = await ctx.Animal.Where(x => x.AnimalStatus !=AnimalStatus.Death).ToListAsync();
            foreach (var a in AllAnimals)
            {
                var age = (DateTime.Now - a.CreatedDate).TotalMinutes;
                if (cfg.MaxLifeMinutes <= age)
                {
                    a.AnimalStatus = AnimalStatus.Death;
                    ctx.Animal.Update(a);
                    await ctx.SaveChangesAsync();
                }

            }
        }

        public async static Task<AppDimensions> GetAppDinesionAsync(this IAppDbContext ctx)
        {

            var dimesion = await ctx.AppDimension.SingleOrDefaultAsync();
            return dimesion ?? new AppDimensions()
            {
                AppLifeTime = new TimeSpan(1, 0, 0),
                MaxDoBornCount = 5,
                MaxDoPerBornCount = 15,
                MaxLifeMinutes = 30,
                WaitMinutes = 3,
                Pregnancyduration = new TimeSpan(0, 3, 0),
                AnimalType ="Tavşan"
         

            };

        }

    }
}
