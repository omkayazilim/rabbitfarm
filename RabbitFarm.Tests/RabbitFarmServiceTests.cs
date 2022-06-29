
using AutoFixture;
using Moq;
using RabbitFarm.Application;
using RabbitFarm.Domain.Dtos;
using RabbitFarm.Domain.Entities;
using RabbitFarm.Tests.TestHelpers;
using RabbitFarmInfrastructer.AppDbProviders;
using Shouldly;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;


namespace RabbitFarm.Tests
{
    public class RabbitFarmServiceTests
    {
        private readonly Fixture _fixture;
        private Mock<IAppDbContext> _dbMock;
        private  IRabbitFarmService _rabbitFarmService;  
        public RabbitFarmServiceTests()
        {
             _fixture = new Fixture();
             _dbMock = new Mock<IAppDbContext>();   

               
        }

        [Fact]
        public async Task GetAppDinemsions_test() {

            var dbset = DbMockHelper.CreateMockDbSet<AppDimensions>(_fixture.Build<AppDimensions>().CreateMany(3).ToList());
            _dbMock.Setup(x=>x.AppDimension).Returns(dbset);
            _rabbitFarmService = new Application.RabbitFarmService(_dbMock.Object);
            var result= await _rabbitFarmService.GetAppDinemsions();

            /// Assert
            result.ShouldNotBeNull();
            


        }

        [Fact]
        public async Task SetAppDinemsions_test()
        {

            var dbset = DbMockHelper.CreateMockDbSet<AppDimensions>(_fixture.Build<AppDimensions>().CreateMany(3).ToList());
            _dbMock.Setup(x => x.AppDimension).Returns(dbset);
            var _rabbitFarmService = new Application.RabbitFarmService(_dbMock.Object);
            var result = await _rabbitFarmService.SetAppDinemsions(_fixture.Build<AppDimensionsInput>().Create());

            /// Assert
            result.ShouldBe(true);



        }

    }
}
