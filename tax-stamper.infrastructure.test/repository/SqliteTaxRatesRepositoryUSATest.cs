using System;
using Xunit;
using Bogus;
using Serilog;
using tax_stamper.domain.entity;
using tax_stamper.domain.repository;
using tax_stamper.infrastructure.repository;

namespace tax_stamper.infrastructure.test.repository
{
    public class SqliteTaxRatesRepositoryUSATest
    {
        private ILogger GetLogger(string instance, string name)
        {
            string LOGPATH = Environment.GetEnvironmentVariable("LOGPATH") ?? "/opt/data";
            string INSTANCE_PATH = $"{LOGPATH}/{instance}";

            System.IO.Directory.CreateDirectory(INSTANCE_PATH);

            return new LoggerConfiguration()
                .MinimumLevel.Debug()
                .WriteTo.Console()
                .WriteTo.File($"{INSTANCE_PATH}/{name}.log", rollingInterval: RollingInterval.Day)
                .CreateLogger();
        }
        
        [Fact]
        public void CreateTest()
        {
            var instancePath = "SqliteTaxRatesRepositoryUSATest";
            var logger = GetLogger(instancePath, "CreateTest");

            ITaxRatesRepositoryUSA repository = new SqliteTaxRatesRepositoryUSA(logger, "CreateTest_TaxRateRepo", instancePath);

            var record = new TaxRateUSA() {
                Zipcode = 68136
                , ZipPlus4StartRange = 0
                , ZipPlus4EndRange = 9999
                , EffectiveDate = new DateTime(1941, 1, 1)
                , TaxRateState = 0.07
                , TaxRateCounty = 0.05
                , TaxRateCity = 0.03
                , TaxRateLocal1 = 0.02
                , TaxRateLocal2 = 0.01
            };

            long id = repository.Create(record);

            Assert.True(id > 0); 
        }
    }
}
