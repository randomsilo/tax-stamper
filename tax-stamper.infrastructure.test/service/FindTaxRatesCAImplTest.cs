using System;
using System.Collections.Generic;
using Xunit;
using Bogus;
using Serilog;
using tax_stamper.domain.entity;
using tax_stamper.domain.model;
using tax_stamper.domain.repository;
using tax_stamper.infrastructure.repository;
using tax_stamper.infrastructure.service;


namespace tax_stamper.infrastructure.test.service
{
    public class FindTaxRatesCAImplTest
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

        private ITaxRatesRepositoryCA GetUseTaxRatesRepositoryCA(ILogger logger, string name, string instancePath)
        {
            ITaxRatesRepositoryCA repository = new SqliteTaxRatesRepositoryCA(logger, name, instancePath);

            var record = new TaxRateCA() {
                ForwardStationArea = "K8N"
                , LocalDeliveryUnit = "5W6"
                , EffectiveDate = new DateTime(1960, 2, 10)
                , TaxRateGST = 0.07
                , TaxRatePST = 0.05
                , TaxRateHST = 0.03
            };

            repository.Create(record);

            return repository;
        }

        private ITaxRatesRepositoryCA GetSalesTaxRatesRepositoryCA(ILogger logger, string name, string instancePath)
        {
            ITaxRatesRepositoryCA repository = new SqliteTaxRatesRepositoryCA(logger, name, instancePath);

            var record = new TaxRateCA() {
                ForwardStationArea = "K8N"
                , LocalDeliveryUnit = "5W6"
                , EffectiveDate = new DateTime(1960, 2, 10)
                , TaxRateGST = 0.08
                , TaxRatePST = 0.06
                , TaxRateHST = 0.04
            };

            repository.Create(record);

            return repository;
        }
        
        [Fact]
        public void FindTaxRatesCAService()
        {
            TaxSearchCA search = null;
            TaxResultsCA results = null;

            var instancePath = "xunit_tests/tax-stamper/TaxRatesServiceCA";
            var name = "TaxRatesServiceCA";
            var logger = GetLogger(instancePath, name);

            var service = new FindTaxRatesCAImpl(
                logger
                , GetUseTaxRatesRepositoryCA(logger, name+"_UseTax", instancePath)
                , GetSalesTaxRatesRepositoryCA(logger, name+"_SalesTax", instancePath)
            );

            // find record - Sales Tax
            search = new TaxSearchCA()
            {
                ForwardStationArea = "K8N"
                , LocalDeliveryUnit = "5W6"
                , OnDate = DateTime.Now
            };
            results = service.FindSalesTaxRates(search);

            Assert.Equal(0.08, results.TaxRateGST);
            Assert.Equal(0.06, results.TaxRatePST);
            Assert.Equal(0.04, results.TaxRateHST);

            // find record - Use Tax
            search = new TaxSearchCA()
            {
                ForwardStationArea = "K8N"
                , LocalDeliveryUnit = "5W6"
                , OnDate = DateTime.Now
            };
            results = service.FindUseTaxRates(search);

            Assert.Equal(0.07, results.TaxRateGST);
            Assert.Equal(0.05, results.TaxRatePST);
            Assert.Equal(0.03, results.TaxRateHST);
            
            // bad data - ForwardStationArea
            search = new TaxSearchCA()
            {
                ForwardStationArea = "AAA"
                , LocalDeliveryUnit = "5W6"
                , OnDate = DateTime.Now
            };
            Assert.Throws<ArgumentException>(() => results = service.FindSalesTaxRates(search));

            // bad data - LocalDeliveryUnit
            search = new TaxSearchCA()
            {
                ForwardStationArea = "K8N"
                , LocalDeliveryUnit = "123"
                , OnDate = DateTime.Now
            };
            Assert.Throws<ArgumentException>(() => results = service.FindSalesTaxRates(search));

            // bad data - OnDate is ancient
            search = new TaxSearchCA()
            {
                ForwardStationArea = "K8N"
                , LocalDeliveryUnit = "5W6"
                , OnDate = new DateTime(1800, 1, 1)
            };
            Assert.Throws<ArgumentException>(() => results = service.FindSalesTaxRates(search));

            // bad data - OnDate too future
            search = new TaxSearchCA()
            {
                ForwardStationArea = "K8N"
                , LocalDeliveryUnit = "5W6"
                , OnDate = new DateTime(2100, 1, 1)
            };
            Assert.Throws<ArgumentException>(() => results = service.FindSalesTaxRates(search));

            // no record
            search = new TaxSearchCA()
            {
                ForwardStationArea = "Z0Z"
                , LocalDeliveryUnit = "0Z0"
                , OnDate = DateTime.Now
            };
            Assert.Throws<KeyNotFoundException>(() => results = service.FindSalesTaxRates(search));
        }
    }
}
