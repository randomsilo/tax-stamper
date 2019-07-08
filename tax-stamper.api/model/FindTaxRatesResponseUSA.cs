using System;
using tax_stamper.domain.model;



namespace tax_stamper.api.model
{
    public class FindTaxRatesResponseUSA
    {
        public TaxSearchUSA SearchCriteria { get; set; }
        public TaxResultsUSA SearchResults { get; set; }
        public bool ApiSuccess { get; set; }
        public string ApiMessage { get; set; }
    }
}