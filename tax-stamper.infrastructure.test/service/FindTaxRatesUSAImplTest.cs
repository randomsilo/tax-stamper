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
    public class FindTaxRatesUSAImplTest
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

        private ITaxRatesRepositoryUSA GetUseTaxRatesRepositoryUSA(ILogger logger, string name, string instancePath)
        {
            ITaxRatesRepositoryUSA repository = new SqliteTaxRatesRepositoryUSA(logger, name, instancePath);

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

            repository.Create(record);

            return repository;
        }

        private ITaxRatesRepositoryUSA GetSalesTaxRatesRepositoryUSA(ILogger logger, string name, string instancePath)
        {
            ITaxRatesRepositoryUSA repository = new SqliteTaxRatesRepositoryUSA(logger, name, instancePath);

            var record = new TaxRateUSA() {
                Zipcode = 68136
                , ZipPlus4StartRange = 0
                , ZipPlus4EndRange = 9999
                , EffectiveDate = new DateTime(1941, 1, 1)
                , TaxRateState = 0.08
                , TaxRateCounty = 0.06
                , TaxRateCity = 0.04
                , TaxRateLocal1 = 0.03
                , TaxRateLocal2 = 0.02
            };

            repository.Create(record);

            return repository;
        }
        
        [Fact]
        public void FindTaxRatesUSAService()
        {
            TaxSearchUSA search = null;
            TaxResultsUSA results = null;

            var instancePath = "xunit_tests/tax-stamper/TaxRatesServiceUSA";
            var name = "TaxRatesServiceUSA";
            var logger = GetLogger(instancePath, name);

            var service = new FindTaxRatesUSAImpl(
                logger
                , GetUseTaxRatesRepositoryUSA(logger, name+"_UseTax", instancePath)
                , GetSalesTaxRatesRepositoryUSA(logger, name+"_SalesTax", instancePath)
            );

            // find record
            search = new TaxSearchUSA()
            {
                Zipcode = "68136"
                , ZipPlus4 = "2121"
                , OnDate = DateTime.Now
            };
            results = service.FindSalesTaxRates(search);

            Assert.Equal(0.08, results.TaxRateState);
            Assert.Equal(0.06, results.TaxRateCounty);
            Assert.Equal(0.04, results.TaxRateCity);
            Assert.Equal(0.03, results.TaxRateLocal1);
            Assert.Equal(0.02, results.TaxRateLocal2);
            
            // bad data - Zipcode
            search = new TaxSearchUSA()
            {
                Zipcode = "ABCDE"
                , ZipPlus4 = "1234"
                , OnDate = DateTime.Now
            };
            Assert.Throws<ArgumentException>(() => results = service.FindSalesTaxRates(search));

            // bad data - ZipPlus4
            search = new TaxSearchUSA()
            {
                Zipcode = "68136"
                , ZipPlus4 = "ABCD"
                , OnDate = DateTime.Now
            };
            Assert.Throws<ArgumentException>(() => results = service.FindSalesTaxRates(search));

            // bad data - OnDate is ancient
            search = new TaxSearchUSA()
            {
                Zipcode = "68136"
                , ZipPlus4 = "1234"
                , OnDate = new DateTime(1800, 1, 1)
            };
            Assert.Throws<ArgumentException>(() => results = service.FindSalesTaxRates(search));

            // bad data - OnDate too future
            search = new TaxSearchUSA()
            {
                Zipcode = "68136"
                , ZipPlus4 = "1234"
                , OnDate = new DateTime(2100, 1, 1)
            };
            Assert.Throws<ArgumentException>(() => results = service.FindSalesTaxRates(search));

            // no record
            search = new TaxSearchUSA()
            {
                Zipcode = "88888"
                , ZipPlus4 = "1234"
                , OnDate = DateTime.Now
            };
            Assert.Throws<KeyNotFoundException>(() => results = service.FindSalesTaxRates(search));
        }
    }
}
