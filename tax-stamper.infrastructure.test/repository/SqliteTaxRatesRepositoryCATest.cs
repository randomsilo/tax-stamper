using System;
using Xunit;
using Bogus;
using Serilog;
using tax_stamper.domain.entity;
using tax_stamper.domain.repository;
using tax_stamper.infrastructure.repository;

namespace tax_stamper.infrastructure.test.repository
{
    public class SqliteTaxRatesRepositoryCATest
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
            var instancePath = "xunit_tests/tax-stamper/TaxRatesRepoCA";
            var logger = GetLogger(instancePath, "CreateTest");

            ITaxRatesRepositoryCA repository = new SqliteTaxRatesRepositoryCA(logger, "CreateTest_TaxRateRepo", instancePath);

            var record = new TaxRateCA() {
                ForwardStationArea = "K8N"
                , LocalDeliveryUnit = "5W6"
                , EffectiveDate = new DateTime(1941, 1, 1)
                , TaxRateGST = 0.07
                , TaxRatePST = 0.05
                , TaxRateHST = 0.03
            };

            long id = repository.Create(record);

            Assert.True(id > 0); 
        }

        [Fact]
        public void CreateFetchUpdateDeleteTest()
        {
            var instancePath = "xunit_tests/tax-stamper/TaxRatesRepoCA";
            var logger = GetLogger(instancePath, "CreateFetchUpdateDeleteTest");

            ITaxRatesRepositoryCA repository = new SqliteTaxRatesRepositoryCA(logger, "CFUD_TaxRateRepo", instancePath);

            var record = new TaxRateCA() {
                ForwardStationArea = "K8N"
                , LocalDeliveryUnit = "5W6"
                , EffectiveDate = new DateTime(1941, 1, 1)
                , TaxRateGST = 0.07
                , TaxRatePST = 0.05
                , TaxRateHST = 0.03
            };

            // create record
            long id = repository.Create(record);
            Assert.True(id > 0); 

            // fetch record
            var fetchedRecord = repository.FetchById(id);
            Assert.Equal(id, fetchedRecord.Id);

            // update object
            fetchedRecord.TaxRateGST = 0.077;
            fetchedRecord.TaxRatePST = 0.055;

            // update record
            var updatedRecords = repository.Update(fetchedRecord);
            Assert.Equal(1, updatedRecords);

            // confirm update
            var updatedRecord = repository.FetchById(id);
            Assert.Equal(fetchedRecord.TaxRateGST, updatedRecord.TaxRateGST);
            Assert.Equal(fetchedRecord.TaxRatePST, updatedRecord.TaxRatePST);

            // delete record
            var deletedRecords = repository.Delete(updatedRecord);
            Assert.Equal(1, deletedRecords);

            // confirm deletion
            var deletedRecord = repository.FetchById(id);
            Assert.Null(deletedRecord);
        }

        public void FindByZipcodeTest()
        {
            var instancePath = "xunit_tests/tax-stamper/TaxRatesRepoCA";
            var logger = GetLogger(instancePath, "CreateFetchUpdateDeleteTest");

            ITaxRatesRepositoryCA repository = new SqliteTaxRatesRepositoryCA(logger, "FindByZipcode_TaxRateRepo", instancePath);

            var record = new TaxRateCA() {
                ForwardStationArea = "K8N"
                , LocalDeliveryUnit = "5W6"
                , EffectiveDate = new DateTime(1960, 2, 10)
                , TaxRateGST = 0.07
                , TaxRatePST = 0.05
                , TaxRateHST = 0.03
            };

            // create record
            long id = repository.Create(record);
            Assert.True(id > 0); 

            // fetch record
            var fetchedRecord = repository.FetchByZipcode("K8N", "5W6", new DateTime(1960, 2, 20));
            Assert.Equal(id, fetchedRecord.Id);

            // fetch record
            var fetchNullRecord = repository.FetchByZipcode("K8N", "5W6", new DateTime(1960, 1, 20));
            Assert.Null(fetchNullRecord);

        }
    }
}
