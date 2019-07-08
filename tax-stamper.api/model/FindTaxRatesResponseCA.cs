using System;
using tax_stamper.domain.model;



namespace tax_stamper.api.model
{
    public class FindTaxRatesResponseCA
    {
        public TaxSearchCA SearchCriteria { get; set; }
        public TaxResultsCA SearchResults { get; set; }
        public bool ApiSuccess { get; set; }
        public string ApiMessage { get; set; }
    }
}