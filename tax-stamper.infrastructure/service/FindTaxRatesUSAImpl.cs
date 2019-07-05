using System;
using System.Collections.Generic;
using Serilog;
using tax_stamper.domain.entity;
using tax_stamper.domain.model;
using tax_stamper.domain.repository;
using tax_stamper.domain.service;


namespace tax_stamper.infrastructure.service
{
    public class FindTaxRatesUSAImpl : IFindTaxRatesUSA
    {
        private ILogger _logger;
        private ITaxRatesRepositoryUSA _useTaxRatesRepositoryUSA;
        private ITaxRatesRepositoryUSA _salesTaxRatesRepositoryUSA;

        public FindTaxRatesUSAImpl(ILogger logger, ITaxRatesRepositoryUSA useTaxRatesRepositoryUSA, ITaxRatesRepositoryUSA salesTaxRatesRepositoryUSA)
        {
            // logger
            _logger = logger;
            _logger.Verbose($"{this.GetType().Name} IN Constructor");

            // repos
            _useTaxRatesRepositoryUSA = useTaxRatesRepositoryUSA;
            _salesTaxRatesRepositoryUSA = salesTaxRatesRepositoryUSA;
        }

        public TaxResultsUSA FindUseTaxRates(TaxSearchUSA searchBy)
        {
            _logger.Verbose($"{this.GetType().Name} IN FindUseTaxRates");

            return FindTaxRates(searchBy, _useTaxRatesRepositoryUSA);
        }

        public TaxResultsUSA FindSalesTaxRates(TaxSearchUSA searchBy)
        {
            _logger.Verbose($"{this.GetType().Name} IN FindSalesTaxRates");

            return FindTaxRates(searchBy, _salesTaxRatesRepositoryUSA);
        }
        public TaxResultsUSA FindTaxRates(TaxSearchUSA searchBy, ITaxRatesRepositoryUSA repo)
        {
            _logger.Verbose($"{this.GetType().Name} IN FindTaxRates");

            var check = searchBy.IsValid();
            if (!check.validStatus)
            {
                _logger.Error(check.whyNot);
                throw new ArgumentException(check.whyNot);
            }

            var zipcode = int.Parse(searchBy.Zipcode);
            var zipPlus4 = int.Parse(searchBy.ZipPlus4);

            var fetchedRecord = repo.FetchByZipcode(zipcode, zipPlus4, searchBy.OnDate);

            if(fetchedRecord == null)
            {
                var notFoundMsg = string.Format(
                        "TaxRates for {0}-{1} were not found"
                        , zipcode.ToString("00000")
                        , zipPlus4.ToString("0000")
                    );
                _logger.Error(notFoundMsg);
                throw new KeyNotFoundException(notFoundMsg);
            }

            var results = new TaxResultsUSA()
            {
                TaxRateState = fetchedRecord.TaxRateState
                , TaxRateCounty = fetchedRecord.TaxRateCounty
                , TaxRateCity = fetchedRecord.TaxRateCity
                , TaxRateLocal1 = fetchedRecord.TaxRateLocal1
                , TaxRateLocal2 = fetchedRecord.TaxRateLocal2
            };

            return results;
        }
    }
}