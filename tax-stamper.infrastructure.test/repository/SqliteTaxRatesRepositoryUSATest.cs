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
        private ILogger GetLogger(string instancePath, string name)
        {
            string LOGPATH = Environment.GetEnvironmentVariable("LOGPATH") ?? "/opt/data";
            string directory = $"{LOGPATH}/{instancePath}";

            System.IO.Directory.CreateDirectory(directory);

            return new LoggerConfiguration()
                .MinimumLevel.Verbose()
                .WriteTo.Console()
                .WriteTo.File($"{directory}/{name}.log", rollingInterval: RollingInterval.Day)
                .CreateLogger();
        }
        
        [Fact]
        public void CreateTest()
        {
            var instancePath = "xunit_tests/tax-stamper/TaxRatesRepoUSA";
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

        [Fact]
        public void CreateFetchUpdateDeleteTest()
        {
            var instancePath = "xunit_tests/tax-stamper/TaxRatesRepoUSA";
            var logger = GetLogger(instancePath, "CreateFetchUpdateDeleteTest");

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

            // create record
            long id = repository.Create(record);
            Assert.True(id > 0); 

            // fetch record
            var fetchedRecord = repository.FetchById(id);
            Assert.Equal(id, fetchedRecord.Id);

            // update object
            fetchedRecord.TaxRateState = 0.077;
            fetchedRecord.TaxRateCounty = 0.055;

            // update record
            var updatedRecords = repository.Update(fetchedRecord);
            Assert.Equal(1, updatedRecords);

            // confirm update
            var updatedRecord = repository.FetchById(id);
            Assert.Equal(fetchedRecord.TaxRateState, updatedRecord.TaxRateState);
            Assert.Equal(fetchedRecord.TaxRateCounty, updatedRecord.TaxRateCounty);

            // delete record
            var deletedRecords = repository.Delete(updatedRecord);
            Assert.Equal(1, deletedRecords);

            // confirm deletion
            var deletedRecord = repository.FetchById(id);
            Assert.Null(deletedRecord);
        }
    }
}
