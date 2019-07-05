using System;
using System.Collections.Generic;
using Serilog;
using tax_stamper.domain.entity;
using tax_stamper.domain.model;
using tax_stamper.domain.repository;
using tax_stamper.domain.service;


namespace tax_stamper.infrastructure.service
{
    public class FindTaxRatesCAImpl : IFindTaxRatesCA
    {
        private ILogger _logger;
        private ITaxRatesRepositoryCA _useTaxRatesRepositoryCA;
        private ITaxRatesRepositoryCA _salesTaxRatesRepositoryCA;

        public FindTaxRatesCAImpl(ILogger logger, ITaxRatesRepositoryCA useTaxRatesRepositoryCA, ITaxRatesRepositoryCA salesTaxRatesRepositoryCA)
        {
            // logger
            _logger = logger;
            _logger.Verbose($"{this.GetType().Name} IN Constructor");

            // repos
            _useTaxRatesRepositoryCA = useTaxRatesRepositoryCA;
            _salesTaxRatesRepositoryCA = salesTaxRatesRepositoryCA;
        }

        public TaxResultsCA FindUseTaxRates(TaxSearchCA searchBy)
        {
            _logger.Verbose($"{this.GetType().Name} IN FindUseTaxRates");

            return FindTaxRates(searchBy, _useTaxRatesRepositoryCA);
        }

        public TaxResultsCA FindSalesTaxRates(TaxSearchCA searchBy)
        {
            _logger.Verbose($"{this.GetType().Name} IN FindSalesTaxRates");

            return FindTaxRates(searchBy, _salesTaxRatesRepositoryCA);
        }
        public TaxResultsCA FindTaxRates(TaxSearchCA searchBy, ITaxRatesRepositoryCA repo)
        {
            _logger.Verbose($"{this.GetType().Name} IN FindTaxRates");

            var check = searchBy.IsValid();
            if (!check.validStatus)
            {
                _logger.Error(check.whyNot);
                throw new ArgumentException(check.whyNot);
            }

            var fetchedRecord = repo.FetchByZipcode(searchBy.ForwardStationArea, searchBy.LocalDeliveryUnit, searchBy.OnDate);

            if(fetchedRecord == null)
            {
                var notFoundMsg = string.Format(
                        "TaxRates for {0} {1} were not found"
                        , searchBy.ForwardStationArea
                        , searchBy.LocalDeliveryUnit
                    );
                _logger.Error(notFoundMsg);
                throw new KeyNotFoundException(notFoundMsg);
            }

            var results = new TaxResultsCA()
            {
                TaxRateGST = fetchedRecord.TaxRateGST
                , TaxRatePST = fetchedRecord.TaxRatePST
                , TaxRateHST = fetchedRecord.TaxRateHST
            };

            return results;
        }
    }
}